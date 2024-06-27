using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;
using static PoeWikiApi.Endpoints.CargoQueryParams.CargoQueryParams;

namespace PoeWikiApi.Endpoints
{
    public class LeagueEndpoint(HttpRetriever pHttp, CacheHandler<string, string> pCache, ConsoleLogger pLogger) : BaseEndpoint(pHttp, pCache, pLogger)
    {
        public LeagueModel? GetById(uint pId)
        {
            return GetFirstDataModel<LeagueModel>(_cargoTitle, CargoParamsMap[CargoTypes.LEAGUES].Tables, CargoParamsMap[CargoTypes.LEAGUES].Fields, $"{CargoParamsMap[CargoTypes.LEAGUES].Where} AND _ID=\"{pId}\"");
        }

        public List<LeagueModel> GetAll()
        {
            return GetListWithBatching<LeagueModel>(_cargoTitle, CargoParamsMap[CargoTypes.LEAGUES].Tables, CargoParamsMap[CargoTypes.LEAGUES].Fields, CargoParamsMap[CargoTypes.LEAGUES].Where, 500, 0);
        }
    }
}
