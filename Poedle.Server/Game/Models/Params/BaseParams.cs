using static Poedle.Enums.MiscEnums;

namespace Poedle.Game.Models.Params
{
    public abstract class BaseParams
    {
        public string Name { get; set; } = "";
    }

    public abstract class BaseParamsResult
    {
        public ParamsResult Name { get; set; } = ParamsResult.CORRECT;
    }
}
