using System.Text;

using Poedle.Game.Models.Params;

using static Poedle.Enums.MiscEnums;

namespace Poedle.Game.Utils
{
    public class ParamsStringBuilder
    {
        public static string BuildParamString(UniqueParams pParams, bool pEnableHints)
        {

            StringBuilder builder = new();
            // Base Item
            builder.Append($"Base: {pParams.BaseItem} / ");
            // Leagues Introduced
            builder.Append($"Leagues: {GetListString(pParams.LeaguesIntroduced)} / ");
            // Req Stats
            builder.Append($"ReqLvl: {pParams.ReqLvl} / ");
            builder.Append($"ReqDex: {pParams.ReqDex} / ");
            builder.Append($"ReqInt: {pParams.ReqInt} / ");
            builder.Append($"ReqStr: {pParams.ReqStr} / ");
            // Qualities
            builder.Append($"Quals: {GetListString(pParams.Qualities)} / ");
            // Drops
            builder.Append($"Drops: {GetListString(pParams.DropSources)} / ");

            if (pEnableHints)
            {
                builder.Append($"Specifics: {GetListString(pParams.DropSourcesSpecific)} / ");
            }

            return builder.ToString();
        }

        public static string BuildResultString(UniqueParams pParams, UniqueParamsResult pResult, bool pEnableHints)
        {

            StringBuilder builder = new();
            // Base Item
            builder.Append($"Base: {pParams.BaseItem} ({GetParamResultStatus(pResult.BaseItem)}) / ");
            // Leagues Introduced
            builder.Append($"Leagues: {GetListString(pParams.LeaguesIntroduced)} ({GetParamResultStatus(pResult.LeaguesIntroduced)}) / ");
            // Req Stats
            builder.Append($"ReqLvl: {pParams.ReqLvl} ({GetParamResultStatus(pResult.ReqLvl)}) / ");
            builder.Append($"ReqDex: {pParams.ReqDex} ({GetParamResultStatus(pResult.ReqDex)}) / ");
            builder.Append($"ReqInt: {pParams.ReqInt} ({GetParamResultStatus(pResult.ReqInt)}) / ");
            builder.Append($"ReqStr: {pParams.ReqStr} ({GetParamResultStatus(pResult.ReqStr)}) / ");
            // Qualities
            builder.Append($"Quals: {GetListString(pParams.Qualities)} ({GetParamResultStatus(pResult.Qualities)}) / ");
            // Drops
            builder.Append($"Drops: {GetListString(pParams.DropSources)} ({GetParamResultStatus(pResult.DropSources)}) / ");

            if (pEnableHints)
            {
                builder.Append($"Specifics: {GetListString(pParams.DropSourcesSpecific)} ({GetParamResultStatus(pResult.DropSourcesSpecific)}) / ");
            }

            return builder.ToString();
        }

        private static string GetListString<T>(List<T> pList)
        {
            if (pList.Count == 0)
            {
                return "None";
            }
            return $"{string.Join(",", pList)}";
        }

        private static string GetParamResultStatus(ParamsResult pResult)
        {
            return $"{pResult.ToString()[0]}";
        }
    }
}
