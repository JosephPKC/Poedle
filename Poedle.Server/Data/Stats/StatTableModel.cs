namespace Poedle.Server.Data.Stats
{
    public class StatTableModel: IComparable<StatTableModel>
    {
        public string Answer { get; set; } = string.Empty;
        public int BestScore { get; set; } = 0;
        public int WorstScore { get; set; } = 0;
        public double AverageScore { get; set; } = 0; 
        public int TotalGames { get; set; } = 0;

        public int CompareTo(StatTableModel? other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(Answer, other.Answer);
        }
    }
}
