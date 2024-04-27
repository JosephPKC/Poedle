namespace DebugLogger.Writer
{
    public interface IConsoleWriter
    {
        void Write(string pMessage);
        void WriteLine(string pMessage);
        void Write(string pFormat, params object[] pArgs);
        void WriteLine(string pFormat, params object[] pArgs);
    }
}
