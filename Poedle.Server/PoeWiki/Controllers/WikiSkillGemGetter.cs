using Poedle.PoeWiki.Models;
using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;

using static Poedle.PoeWiki.CargoQueryParams;

namespace Poedle.PoeWiki.Controllers
{
    public class WikiSkillGemGetter(HttpRetriever pHttp, CacheHandler<string, string> pCache, DebugLogger pLogger) : BaseWikiController(pHttp, pCache, pLogger)
    {
        public PoeWikiSkillGem? GetById(string pId)
        {
            return GetFirstDataModel<PoeWikiSkillGem>(_cargoTitle, CargoParamsMap[CargoParamTypes.SKILLGEMS].Tables, CargoParamsMap[CargoParamTypes.SKILLGEMS].Fields, $"{CargoParamsMap[CargoParamTypes.SKILLGEMS].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiSkillGem> GetAll()
        {
            return GetListWithBatching<PoeWikiSkillGem>(_cargoTitle, CargoParamsMap[CargoParamTypes.SKILLGEMS].Tables, CargoParamsMap[CargoParamTypes.SKILLGEMS].Fields, CargoParamsMap[CargoParamTypes.SKILLGEMS].Where, 500, 0);
        }
    }
}
