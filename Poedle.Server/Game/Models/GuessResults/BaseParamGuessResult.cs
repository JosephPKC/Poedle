using Poedle.Game.Models.Params;

namespace Poedle.Game.Models.GuessResults
{
    public class BaseParamGuessResult<T, U> : BaseGuessResult where T : BaseParams where U : BaseParamsResult
    {
        public T? Params { get; set; }
        public U? Result { get; set; }
    }
}
