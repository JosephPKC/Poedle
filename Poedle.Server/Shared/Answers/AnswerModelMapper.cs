using PoeWikiData.Models;

namespace Poedle.Server.Shared.Answers
{
    public static class AnswerModelMapper
    {
        public static LiteAnswerModel GetLiteAnswer(BaseNamedDbModel pModel)
        {
            return new()
            {
                Value = pModel.Id,
                Label = pModel.DisplayName
            };
        }

        public static FullAnswerModel GetFullAnswer(BaseNamedDbModel pModel)
        {
            return new()
            {
                Value = pModel.Id,
                Label = pModel.DisplayName,
                HintName = pModel.Name
            };
        }

        public static LiteAnswerModel CondenseAnswer(FullAnswerModel pModel)
        {
            return new()
            {
                Value = pModel.Value,
                Label = pModel.Label
            };
        }
    }
}
