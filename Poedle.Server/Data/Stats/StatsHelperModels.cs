namespace Poedle.Server.Data.Stats
{
    internal class AnswerListAndStat
    {
        public IEnumerable<string> Answers { get; set; } = [];
        public int Stat { get; set; } = 0;
    }

    internal class AnswerAndStat
    {
        public string Answer { get; set; } = string.Empty;
        public int Stat { get; set; } = 0;

        public override string ToString()
        {
            return $"{Answer} ({Stat})";
        }
    }
}
