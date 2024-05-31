using Poedle.Game.Models.Params;
using Poedle.PoeDb.Models;

namespace Poedle.Game.Mappers
{
    public static class UniqueParamsMapper
    {
        public static UniqueParams GetParams(DbUnique? pUnique)
        {
            if (pUnique == null) return new();
            return new()
            {
                ItemClass = pUnique.ItemClass,
                BaseItem = pUnique.BaseItem,
                LeaguesIntroduced = pUnique.LeaguesIntroduced,
                Qualities = pUnique.Qualities,
                DropSources = pUnique.DropSources,
                DropSourcesSpecific = pUnique.DropSourcesSpecific,
                ReqLvl = pUnique.ReqLvl,
                ReqDex = pUnique.ReqDex,
                ReqInt = pUnique.ReqInt,
                ReqStr = pUnique.ReqStr
            };
        }
    }
}
