using PoeWikiData.Models;

namespace Poedle.Server.Data.Answers
{
    internal static class AnswerMapper
    {
        public static AnswerExpModel GetAnswer(BaseNamedDbModel pModel, string pSecondaryName = "")
        {
            string label = pModel.DisplayName;
            if (!string.IsNullOrWhiteSpace(pSecondaryName))
            {
                label += ", " + pSecondaryName;
            }
            return new()
            {
                Value = pModel.Id,
                Label = label,
                AbbrName = pModel.DisplayName
            };
        }
    }
}
    