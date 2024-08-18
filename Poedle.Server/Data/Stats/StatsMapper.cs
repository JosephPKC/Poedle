using MathNet.Numerics.Statistics;
using Poedle.Server.Data.Scores;

namespace Poedle.Server.Data.Stats
{
    internal static class StatsMapper
    {
        public static StatsExpModel GetStats(List<ScoreModel> pScores, ScoreModel pCurrentScore, uint pNbrGuessesRemaining)
        {
            if (pScores.Count == 0)
            {
                return new();
            }

            int bestWorstX = 5;
            StatCollection stats = GetStatsCollection(pScores);

            return new()
            {
                AnswerName = pCurrentScore.Name,
                Score = pCurrentScore.Score.ToString(),
                TotalGames = pScores.Count.ToString(),
                BestSingleScore = stats.ScoresSorted.First().Score.ToString(),
                WorstSingleScore = stats.ScoresSorted.Last().Score.ToString(),
                BestXAnswers = string.Join(", ", stats.ScoresSorted.Take(bestWorstX)),
                WorstXAnswers = string.Join(", ", stats.ScoresSorted.Reverse().Take(bestWorstX)),
                BestWorstXThreshold = bestWorstX,
                TotalAverage = stats.AverageScore.ToString(),
                NbrHintsRemaining = pNbrGuessesRemaining.ToString(),
                StatsPerAnswer = stats.StatTableModels
            };
        }

        private static StatCollection GetStatsCollection(List<ScoreModel> pScores)
        {
            // Pre sort the list
            // When we add the scores into answer buckets, the buckets will remain sorted.
            List<ScoreModel> scoresSorted = [];
            pScores.ForEach((x) => scoresSorted.Add((ScoreModel)x.Clone())); // Clone it to avoid manipulating the original list.
            scoresSorted.Sort();

            Dictionary<uint, LinkedList<ScoreModel>> scoresPerAnswer = [];
            Dictionary<uint, int> sumsPerAnswer = [];
            HashSet<ScoreModel> bestScores = [];
            HashSet<ScoreModel> worstScores = [];
            int bestScore = scoresSorted.First().Score, worstScore = scoresSorted.Last().Score, scoreSum = 0, totalCount = 0;

            // Create a look up that maps the score answer id to a sorted list of scores.
            foreach (ScoreModel score in scoresSorted)
            {
                if (scoresPerAnswer.TryGetValue(score.Id, out LinkedList<ScoreModel>? value))
                {
                    scoresPerAnswer[score.Id].AddLast(score);
                    sumsPerAnswer[score.Id] += score.Score;
                }
                else
                {
                    scoresPerAnswer.Add(score.Id, new([score]));
                    sumsPerAnswer.Add(score.Id, score.Score);
                }

                scoreSum += score.Score;
                totalCount++;
            }

            List<StatTableModel> statsPerAnswer = [];
            foreach (uint id in scoresPerAnswer.Keys)
            {
                LinkedList<ScoreModel> scores = scoresPerAnswer[id];
                statsPerAnswer.Add(new()
                {
                    Answer = scores.First().Name,
                    AverageScore = CalculateAverage(sumsPerAnswer[id], scores.Count),
                    BestScore = scores.First().Score,
                    WorstScore = scores.Last().Score,
                    TotalGames = scores.Count
                });
            }

            statsPerAnswer.Sort();

            return new()
            {
                StatTableModels = statsPerAnswer,
                ScoresSorted = scoresSorted,
                AverageScore = CalculateAverage(scoreSum, scoresSorted.Count),
                Count = totalCount
            };
        }

        private static double CalculateAverage(int pSum, int pCount)
        {
            return Math.Round((double)pSum / pCount, 2, MidpointRounding.ToZero);
        }
    }
}
