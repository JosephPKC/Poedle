namespace Poedle.Server.Data.Results.SkillGems
{
    public class SkillGemsResultExpModel : BaseResultExpModel
    {
        public string PrimaryAttribute { get; set; } = string.Empty;
        public ResultStates PrimaryAttributeResult { get; set; }
        public string DexterityPercent { get; set; } = string.Empty;
        public ResultStates DexterityPercentResult { get; set; }
        public string IntelligencePercent { get; set; } = string.Empty;
        public ResultStates IntelligencePercentResult { get; set; }
        public string StrengthPercent { get; set; } = string.Empty;
        public ResultStates StrengthPercentResult { get; set; }
        public string GemTags { get; set; } = string.Empty;
        public ResultStates GemTagsResult { get; set; }
        public string TagAmount { get; set; } = string.Empty;
        public ResultStates TagAmountResult { get; set; }
    }
}
