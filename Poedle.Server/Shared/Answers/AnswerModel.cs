using PoeWikiData.Models.Common;

namespace Poedle.Server.Shared.Answers
{
    public class FullAnswerModel
    {
        public uint Value { get; set; } = 0;
        public string Label { get; set; } = string.Empty;
        public string HintName { get; set; } = string.Empty;
    }

    public class LiteAnswerModel
    {
        public uint Value { get; set; } = 0;
        public string Label { get; set; } = string.Empty;
    }
}
