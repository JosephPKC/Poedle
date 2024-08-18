using Poedle.Server.Data.Hints.Full;
using Poedle.Server.Data.Results.SkillGems;

namespace Poedle.Server.States.SkillGems
{
    internal class SkillGemsState : BaseState<SkillGemsResultExpModel>
    {
        // Specific Hints
        public FullSingleHintModel DescriptionHint { get; set; } = new();
    }
}
