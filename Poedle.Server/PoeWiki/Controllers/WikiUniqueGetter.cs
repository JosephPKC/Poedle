using Poedle.PoeWiki.Models;
using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;

using static Poedle.PoeWiki.CargoQueryParams;

namespace Poedle.PoeWiki.Controllers
{
    public class WikiUniqueGetter(HttpRetriever pHttp, CacheHandler<string, string> pCache, DebugLogger pLogger) : BaseWikiController(pHttp, pCache, pLogger)
    {
        public PoeWikiUnique? GetById(uint pId)
        {
            return GetFirstDataModel<PoeWikiUnique>(_cargoTitle, CargoParamsMap[CargoParamTypes.UNIQUES].Tables, CargoParamsMap[CargoParamTypes.UNIQUES].Fields, $"{CargoParamsMap[CargoParamTypes.UNIQUES].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiUnique> GetAll()
        {
            return GetListWithBatching<PoeWikiUnique>(_cargoTitle, CargoParamsMap[CargoParamTypes.UNIQUES].Tables, CargoParamsMap[CargoParamTypes.UNIQUES].Fields, CargoParamsMap[CargoParamTypes.UNIQUES].Where, 250, 0);
        }
    }
}
