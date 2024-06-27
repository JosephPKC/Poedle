namespace BaseToolsUtils.Logging.Writers
{
    public interface ILogWriter
    {
        void Write(string pMessage);
        void Write(string pFormat, params object[] pArgs);
        void WriteLine(string pMessage);
        void WriteLine(string pFormat, params object[] pArgs);
    }
}
