using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;
using static PoeWikiApi.Endpoints.CargoQueryParams.CargoQueryParams;

namespace PoeWikiApi.Endpoints
{
    public sealed class UniqueItemEndpoint(HttpRetriever pHttp, CacheHandler<string, string> pCache, ConsoleLogger pLogger) : BaseEndpoint(pHttp, pCache, pLogger)
    {
        public UniqueItemModel? GetById(uint pId)
        {
            return GetFirstDataModel<UniqueItemModel>(_cargoTitle, CargoParamsMap[CargoTypes.UNIQUES].Tables, CargoParamsMap[CargoTypes.UNIQUES].Fields, $"{CargoParamsMap[CargoTypes.UNIQUES].Where} AND _ID=\"{pId}\"");
        }

        public List<UniqueItemModel> GetAll()
        {
            return GetListWithBatching<UniqueItemModel>(_cargoTitle, CargoParamsMap[CargoTypes.UNIQUES].Tables, CargoParamsMap[CargoTypes.UNIQUES].Fields, CargoParamsMap[CargoTypes.UNIQUES].Where, 250, 0);
        }
    }
}
