using Poedle.Game.Models.Params;
using Poedle.PoeDb.Models;

namespace Poedle.Game.Mappers
{
    public static class UniqueParamsMapper
    {
        public static UniqueParams GetParams(DbUnique pUnique)
        {
            return new()
            {
                ItemClass = pUnique.ItemClass,
                LeaguesIntroduced = pUnique.LeaguesIntroduced,
                Qualities = pUnique.Qualities,
                DropSources = pUnique.DropSources,
                ReqLvl = pUnique.ReqLvl,
                ReqDex = pUnique.ReqDex,
                ReqInt = pUnique.ReqInt,
                ReqStr = pUnique.ReqStr
            };
        }
    }
}
