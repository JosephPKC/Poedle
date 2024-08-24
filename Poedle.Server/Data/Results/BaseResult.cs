namespace Poedle.Server.Data.Results
{
    public class BaseResult
    {
        public string Value { get; set; } = string.Empty;
        public ResultStates Result { get; set; } = ResultStates.Correct;

        public BaseResult() { }
        public BaseResult(string pValue, ResultStates pResult)
        {
            Value = pValue;
            Result = pResult;
        }
    }
}
