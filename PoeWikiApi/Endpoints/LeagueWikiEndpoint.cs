using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;
using static PoeWikiApi.Endpoints.CargoQueryParams.WikiQueryParams;

namespace PoeWikiApi.Endpoints
{
    public class LeagueWikiEndpoint(HttpRetriever pHttp, CacheHandler<string, string> pCache, ConsoleLogger pLogger) : BaseWikiEndpoint(pHttp, pCache, pLogger)
    {
        public LeagueWikiModel? GetById(uint pId)
        {
            return GetFirstDataModel<LeagueWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.LEAGUES].Tables, CargoParamsMap[CargoTypes.LEAGUES].Fields, $"{CargoParamsMap[CargoTypes.LEAGUES].Where} AND _ID=\"{pId}\"");
        }

        public List<LeagueWikiModel> GetAll()
        {
            return GetListWithBatching<LeagueWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.LEAGUES].Tables, CargoParamsMap[CargoTypes.LEAGUES].Fields, CargoParamsMap[CargoTypes.LEAGUES].Where, 500, 0);
        }
    }
}
