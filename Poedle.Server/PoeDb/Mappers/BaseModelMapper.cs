using Poedle.PoeDb.Models;
using Poedle.PoeWiki.Models;

namespace Poedle.PoeDb.Mappers
{
    public static class BaseModelMapper
    {
        public static void SetBasePoeFields(BaseDbPoeModel pModel, BasePoeWikiModel pApiModel)
        {
            pModel.Name = pApiModel.Name;
            pModel.PageName = pApiModel.PageName;
        }
    }
}
