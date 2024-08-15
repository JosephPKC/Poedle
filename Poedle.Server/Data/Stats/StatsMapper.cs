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

            List<ScoreModel> scoresSorted = [];
            pScores.ForEach((x) => scoresSorted.Add((ScoreModel)x.Clone()));
            scoresSorted.Sort();

            AnswerListAndStat bestSingleAnswer = GetBestOrWorstSingleAnswer(scoresSorted, true);
            AnswerListAndStat worstSingleAnswer = GetBestOrWorstSingleAnswer(scoresSorted, false);
            int bestWorstX = 5;
            IEnumerable<AnswerAndStat> bestXAnswers = GetBestOrWorstXAnswers(scoresSorted, true, bestWorstX);
            IEnumerable<AnswerAndStat> worstXAnswers = GetBestOrWorstXAnswers(scoresSorted, false, bestWorstX);
            Dictionary<string, double> averagesPerAnswer = GetAverageByAnswer(scoresSorted);

            return new()
            {
                AnswerName = pCurrentScore.Name,
                Score = pCurrentScore.Score.ToString(),
                TotalGames = pScores.Count.ToString(),
                BestSingleScore = bestSingleAnswer.Stat.ToString(),
                BestSingleAnswer = string.Join(", ", bestSingleAnswer.Answers),
                WorstSingleScore = worstSingleAnswer.Stat.ToString(),
                WorstSingleAnswer = string.Join(", ", worstSingleAnswer.Answers),
                BestXAnswers = string.Join(", ", bestXAnswers),
                WorstXAnswers = string.Join(", ", worstXAnswers),
                BestWorstXThreshold = bestWorstX,
                TotalAverage = GetTotalAverage(scoresSorted).ToString(),
                AveragesPerAnswer = averagesPerAnswer.Select((x) => $"{x.Key} ({x.Value})"),
                NbrHintsRemaining = pNbrGuessesRemaining.ToString()
            };
        }

        private static AnswerListAndStat GetBestOrWorstSingleAnswer(List<ScoreModel> pScores, bool pIsBest)
        {
            // Sorted from highest to lowest
            int score = pIsBest ? pScores.First().Score : pScores.Last().Score;

            return new()
            {
                Answers = pScores.FindAll((x) => x.Score == score).Select((x) => x.Name),
                Stat = score
            };
        }

        private static IEnumerable<AnswerAndStat> GetBestOrWorstXAnswers(List<ScoreModel> pScores, bool pIsTop, int pAmount)
        {
            if (pAmount == 0) return [];

            if (pAmount > pScores.Count)
            {
                pAmount = pScores.Count;
            }

            ICollection<AnswerAndStat> answers = [];
            if (pIsTop)
            {
                for (int i = 0; i < pAmount; i++)
                {
                    answers.Add(GetAnswerStat(pScores[i]));
                }
            }
            else
            {
                for (int i = pScores.Count - 1, c = 0; i >= 0 && c < pAmount; i--, c++)
                {
                    answers.Add(GetAnswerStat(pScores[i]));
                }
            }

            return answers;
        }

        private static AnswerAndStat GetAnswerStat(ScoreModel pScore)
        {
            return new()
            {
                Answer = pScore.Name,
                Stat = pScore.Score
            };
        }

        private static double GetTotalAverage(List<ScoreModel> pScores)
        {
            IEnumerable<double> scores = pScores.Select((x) => (double)x.Score);

            return Math.Round(scores.Mean(), 2, MidpointRounding.ToZero);
        }

        private static Dictionary<string, double> GetAverageByAnswer(List<ScoreModel> pScores)
        {
            Dictionary<string, List<double>> scores = [];
            foreach (ScoreModel score in pScores)
            {
                if (scores.TryGetValue(score.Name, out List<double>? value))
                {
                    value.Add(score.Score);
                }
                else
                {
                    scores.Add(score.Name, [score.Score]);
                }
            }

            Dictionary<string, double> means = [];
            foreach (KeyValuePair<string, List<double>> pair in scores)
            {
                means.Add(pair.Key, Math.Round(pair.Value.Mean(), 2, MidpointRounding.ToZero));
            }
            return means;
        }
    }
}
