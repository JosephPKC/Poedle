namespace Poedle.Server.Shared.Results
{
    public enum ResultStates
    {
        CORRECT,
        PARTIAL,
        WRONG
    }

    public class BaseResult
    {
        public uint Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public ResultStates NameResult { get; set; } = ResultStates.CORRECT;
    }
}
