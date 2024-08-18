using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi.Models;
using PoeWikiApi.Utils;
using static PoeWikiApi.Endpoints.CargoQueryParams.WikiQueryParams;

namespace PoeWikiApi.Endpoints
{
    public sealed class SkillGemWikiEndpoint(HttpRetriever pHttp, CacheHandler<string, string> pCache, ConsoleLogger pLogger) : BaseWikiEndpoint(pHttp, pCache, pLogger)
    {
        public SkillGemWikiModel? GetById(uint pId)
        {
            return GetFirstDataModel<SkillGemWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.SKILLGEMS].Tables, CargoParamsMap[CargoTypes.SKILLGEMS].Fields, $"{CargoParamsMap[CargoTypes.SKILLGEMS].Where} AND _ID=\"{pId}\"");
        }

        public IEnumerable<SkillGemWikiModel> GetAll()
        {
            return GetListWithBatching<SkillGemWikiModel>(_cargoTitle, CargoParamsMap[CargoTypes.SKILLGEMS].Tables, CargoParamsMap[CargoTypes.SKILLGEMS].Fields, CargoParamsMap[CargoTypes.SKILLGEMS].Where, 250, 0);
        }
    }
}
