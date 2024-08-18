using Poedle.Server.Data.Scores;

namespace Poedle.Server.Data.Stats
{
    internal class StatCollection
    {
        public IEnumerable<StatTableModel> StatTableModels { get; set; } = [];
        public IEnumerable<ScoreModel> ScoresSorted { get; set; } = [];
        public double AverageScore { get; set; } = 0;
        public int Count { get; set; } = 0;
    }
}
