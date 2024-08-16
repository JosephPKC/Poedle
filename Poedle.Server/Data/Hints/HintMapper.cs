using Poedle.Server.Data.Hints.Exp;
using Poedle.Server.Data.Hints.Full;

namespace Poedle.Server.Data.Hints
{
    internal static class HintMapper
    {
        public static SingleHintExpModel Map(FullSingleHintModel pModel)
        {
            return new()
            {
                Hint = pModel.Hint
            };
        }

        public static ListHintExpModel Map(FullListHintModel pModel)
        {
            return new()
            {
                Hint = pModel.Hint
            };
        }

        public static StatExpModel Map(FullStatHintModel pModel)
        {
            return new()
            {
                Hint = pModel.Hint,
                NbrImplicits = pModel.NbrImplicits
            };
        }
    }
}
