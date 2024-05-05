using Poedle.PoeWiki.Models;
using Poedle.Utils.Cache;
using Poedle.Utils.Http;
using Poedle.Utils.Logger;

using static Poedle.PoeWiki.CargoQueryParams;

namespace Poedle.PoeWiki.Controllers
{
    public class WikiLeagueGetter(HttpRetriever pHttp, CacheHandler<string, string> pCache, DebugLogger pLogger) : BaseWikiController(pHttp, pCache, pLogger)
    {
        public PoeWikiLeague? GetById(uint pId)
        {
            return GetFirstDataModel<PoeWikiLeague>(_cargoTitle, CargoParamsMap[CargoParamTypes.LEAGUES].Tables, CargoParamsMap[CargoParamTypes.LEAGUES].Fields, $"{CargoParamsMap[CargoParamTypes.LEAGUES].Where} AND _ID=\"{pId}\"");
        }

        public List<PoeWikiLeague> GetAll()
        {
            return GetListWithBatching<PoeWikiLeague>(_cargoTitle, CargoParamsMap[CargoParamTypes.LEAGUES].Tables, CargoParamsMap[CargoParamTypes.LEAGUES].Fields, CargoParamsMap[CargoParamTypes.LEAGUES].Where, 500, 0);
        }
    }
}
