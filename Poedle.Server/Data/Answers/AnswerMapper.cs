using PoeWikiData.Models;

namespace Poedle.Server.Data.Answers
{
    internal static class AnswerMapper
    {
        public static AnswerExpModel GetAnswer(BaseNamedDbModel pModel)
        {
            return new()
            {
                Value = pModel.Id,
                Label = pModel.DisplayName
            };
        }
    }
}
