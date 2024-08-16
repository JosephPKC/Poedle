using BaseToolsUtils.Utils;

namespace Poedle.Server.Data.Hints.Full
{
    internal class FullSingleHintModel : BaseSingleHintModel, IFullHint
    {
        public IList<string> HintElements { get; set; } = [];
        public double RevealCutOff { get; set; } = 0;
        public Queue<int> RevealQueue { get; set; } = [];

        public bool IsComplete
        {
            get
            {
                return RevealQueue.Count == 0;
            }
        }

        public string FullReveal
        {
            get
            {
                return GeneralUtils.DisplayText(string.Join("", HintElements));
            }
        }
    }

    internal class FullListHintModel : BaseListHintModel, IFullHint
    {
        public IList<string> HintElements { get; set; } = [];

        public double RevealCutOff { get; set; } = 0;
        public Queue<int> RevealQueue { get; set; } = [];

        public bool IsComplete
        {
            get
            {
                return RevealQueue.Count == 0;
            }
        }

        public IList<string> FullReveal
        {
            get
            {
                return HintElements;
            }
        }
    }

    internal interface IFullHint
    {
        IList<string> HintElements { get; set; }
        double RevealCutOff { get; set; }
        Queue<int> RevealQueue { get; set; }
        bool IsComplete { get; }
    }
}
