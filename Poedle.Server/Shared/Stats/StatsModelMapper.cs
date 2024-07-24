using MathNet.Numerics.Statistics;
using Poedle.Server.Shared.Scores;

namespace Poedle.Server.Shared.Stats
{
    public static class StatsModelMapper
    {
        public static StatsModel GetStats(List<ScoreModel> pScores)
        {
            if (pScores.Count == 0) return new();

            List<ScoreModel> scoresSorted = [];
            pScores.ForEach((x) => scoresSorted.Add((ScoreModel)x.Clone()));
            scoresSorted.Sort();

            AnswerListWithStat bestAnswers = GetBestOrWorstAnswer(scoresSorted, true);
            AnswerListWithStat worstAnswers = GetBestOrWorstAnswer(scoresSorted, false);
            IEnumerable<AnswerStat> topAnswers = GetTopOrBottomAnswers(scoresSorted, true, 5);
            IEnumerable<AnswerStat> bottomAnswers = GetTopOrBottomAnswers(scoresSorted, false, 5);
            Dictionary<string, double> averagesPerAnswer = GetAverageByAnswer(scoresSorted);

            return new()
            {
                BestScore = bestAnswers.Stat.ToString(),
                BestAnswers = string.Join(", ", bestAnswers.Answers),
                WorstScore = worstAnswers.Stat.ToString(),
                WorstAnswers = string.Join(", ", worstAnswers.Answers),
                TopAnswers = string.Join(", ", topAnswers),
                BottomAnswers = string.Join(", ", bottomAnswers),
                TotalAverage = GetTotalAverage(scoresSorted).ToString(),
                AveragesPerAnswer = averagesPerAnswer.Select((x) => $"{x.Key} ({x.Value})"),
                TotalGames = pScores.Count.ToString()
            };
        }

        private static AnswerListWithStat GetBestOrWorstAnswer(List<ScoreModel> pScores, bool pIsBest)
        {
            // Sorted from highest to lowest
            int score = pIsBest ? pScores.First().Score : pScores.Last().Score;

            return new()
            {
                Answers = pScores.FindAll((x) => x.Score == score).Select((x) => x.Name),
                Stat = score
            };
        }

        private static IEnumerable<AnswerStat> GetTopOrBottomAnswers(List<ScoreModel> pScores, bool pIsTop, int pAmount)
        {
            if (pAmount == 0) return [];

            if (pAmount > pScores.Count)
            {
                pAmount = pScores.Count;
            }

            ICollection<AnswerStat> answers = [];
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

        private static AnswerStat GetAnswerStat(ScoreModel pScore)
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
