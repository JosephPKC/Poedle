namespace Poedle.Server.Shared.Stats
{
    public class StatsModel
    {
        public string Score { get; set; } = string.Empty;
        // The single (or multiple if there is a tie) answer that had the highest or lowest scores.
        public string BestScore { get; set; } = string.Empty;
        public string BestAnswers { get; set; } = string.Empty;
        public string WorstScore { get; set; } = string.Empty;  
        public string WorstAnswers { get; set; } = string.Empty;
        // The top X answers or the bottom X answers (variable scores).
        public string TopAnswers { get; set; } = string.Empty;
        public string BottomAnswers { get; set; } = string.Empty;
        // Averages
        public string TotalAverage { get; set; } = string.Empty;
        public IEnumerable<string> AveragesPerAnswer { get; set; } = [];
        public string TotalGames { get; set; } = string.Empty;
    }

    public class AnswerListWithStat
    {
        public IEnumerable<string> Answers { get; set; } = [];
        public int Stat { get; set; } = 0;
    }

    public class AnswerStat
    {
        public string Answer { get; set; } = string.Empty;
        public int Stat { get; set; } = 0;

        public override string ToString()
        {
            return $"{Answer} ({Stat})";
        }
    }
}
