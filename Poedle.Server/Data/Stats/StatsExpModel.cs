namespace Poedle.Server.Data.Stats
{
    public class StatsExpModel
    {
        // Basics
        public string AnswerName { get; set; } = string.Empty;
        public string Score {  get; set; } = string.Empty;
        public string TotalGames { get; set; } = string.Empty;
        // Best/Worst Answer
        public string BestSingleScore { get; set; } = string.Empty;
        public string WorstSingleScore { get;set; } = string.Empty;
        public string BestXAnswers { get; set; } = string.Empty;
        public string WorstXAnswers { get; set;} = string.Empty;
        public int BestWorstXThreshold { get; set; } = 0;
        // Averages
        public string TotalAverage { get; set; } = string.Empty;
        // Hints
        public string NbrHintsRemaining {  get; set; } = string.Empty;

        public IEnumerable<StatTableModel> StatsPerAnswer { get; set; } = [];
    }
}
