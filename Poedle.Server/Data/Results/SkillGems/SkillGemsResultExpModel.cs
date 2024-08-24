namespace Poedle.Server.Data.Results.SkillGems
{
    public class SkillGemsResultExpModel : BaseResultExpModel
    {
        public BaseResult PrimaryAttribute { get; set; } = new();
        public BaseResult DexterityPercent { get; set; } = new();
        public BaseResult IntelligencePercent { get; set; } = new();
        public BaseResult StrengthPercent { get; set; } = new();
        public BaseResult GemTags { get; set; } = new();
        public BaseResult TagAmount { get; set; } = new();
    }
}
