using Poedle.PoeWiki.Models;
using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;

using static Poedle.PoeWiki.CargoQueryParams;

namespace Poedle.PoeWiki.Controllers
{
    public class WikiPassiveGetter(HttpRetriever pHttp, CacheHandler<string, string> pCache, DebugLogger pLogger) : BaseWikiController(pHttp, pCache, pLogger)
    {
        public PoeWikiPassive? GetById(string pId)
        {
            return GetFirstDataModel<PoeWikiPassive>(_cargoTitle, CargoParamsMap[CargoParamTypes.PASSIVES].Tables, CargoParamsMap[CargoParamTypes.PASSIVES].Fields, $"{CargoParamsMap[CargoParamTypes.PASSIVES].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiPassive> GetAll()
        {
            return GetListWithBatching<PoeWikiPassive>(_cargoTitle, CargoParamsMap[CargoParamTypes.PASSIVES].Tables, CargoParamsMap[CargoParamTypes.PASSIVES].Fields, CargoParamsMap[CargoParamTypes.PASSIVES].Where, 500, 0);

        }
    }
}
