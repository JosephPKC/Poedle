namespace Poedle.Server.Data.Hints
{
    internal class HintScoreMilestones
    {
        public int StartingScore { get; set; } = 0;
        public int ScoreStep { get; set; } = 0;
        public int Count { get; set; } = 0;
        public HashSet<int> Exceptions { get; set; } = [];

        // Returns an ordered enumerable from starting until count, incrementing via the step and excluding all exceptions
        public IEnumerable<int> ToEnumerable()
        {
            ICollection<int> orderedList = [];
            for (int i = StartingScore, j = 0; j < Count; i += ScoreStep, j++)
            {
                if (!Exceptions.Contains(i))
                {
                    orderedList.Add(i);
                }
                else
                {
                    j--;
                }
            }

            return orderedList;
        }
    }
}
