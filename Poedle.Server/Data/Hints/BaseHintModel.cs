namespace Poedle.Server.Data.Hints
{
    public abstract class BaseHintModel
    {

    }

    public abstract class BaseSingleHintModel : BaseHintModel
    {
        public string Hint { get; set; } = string.Empty;
    }

    public abstract class BaseListHintModel : BaseHintModel
    {
        public IEnumerable<string> Hint { get; set; } = [];
    }
}
