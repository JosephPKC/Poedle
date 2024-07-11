using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;
using static PoeWikiApi.Endpoints.CargoQueryParams.WikiQueryParams;

namespace PoeWikiApi.Endpoints
{
    public sealed class UniqueItemWikiEndpoint(HttpRetriever pHttp, CacheHandler<string, string> pCache, ConsoleLogger pLogger) : BaseWikiEndpoint(pHttp, pCache, pLogger)
    {
        public UniqueItemWikiModel? GetById(uint pId)
        {
            return GetFirstDataModel<UniqueItemWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.UNIQUES].Tables, CargoParamsMap[CargoTypes.UNIQUES].Fields, $"{CargoParamsMap[CargoTypes.UNIQUES].Where} AND _ID=\"{pId}\"");
        }

        public List<UniqueItemWikiModel> GetAll()
        {
            return GetListWithBatching<UniqueItemWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.UNIQUES].Tables, CargoParamsMap[CargoTypes.UNIQUES].Fields, CargoParamsMap[CargoTypes.UNIQUES].Where, 250, 0);
        }
    }
}
