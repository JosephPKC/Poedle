namespace Poedle.Server.Data.Results
{
    public abstract class BaseResultExpModel
    {
        public uint Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public ResultStates NameResult { get; set; } = ResultStates.Correct;
    }
}
