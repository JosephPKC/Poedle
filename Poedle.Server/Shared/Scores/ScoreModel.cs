namespace Poedle.Server.Shared.Scores
{
    public class ScoreModel : IComparable<ScoreModel>, ICloneable
    {
        public uint Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int Score { get; set; } = 0;

        public object Clone()
        {
            return new ScoreModel()
            {
                Id = Id,
                Name = Name,
                Score = Score
            };
        }

        public int CompareTo(ScoreModel? other)
        {
            if (other == null) return 1;

            if (other.Score > Score) return -1;

            if (other.Score < Score) return 1;

            return string.Compare(Name, other.Name);
        }
    }
}
