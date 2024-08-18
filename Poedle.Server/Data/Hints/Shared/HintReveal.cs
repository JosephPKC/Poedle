namespace Poedle.Server.Data.Hints.Shared
{
    internal class HintReveal : IComparable<HintReveal>
    {
        public int ScoreMilestone { get; set; } = 0;
        public HintTypes HintType { get; set; } = HintTypes.None;

        public int CompareTo(HintReveal? other)
        {
            if (other == null)
            {
                return 1;
            }

            if (ScoreMilestone == other.ScoreMilestone)
            {
                return 0;
            }

            return ScoreMilestone > other.ScoreMilestone ? 1 : -1;
        }
    }
}
