namespace BaseToolsUtils.Logging.Writers
{
    public class ConsoleWriter : ILogWriter
    {
        public void Write(string pMessage)
        {
            Console.Write(pMessage);
        }

        public void Write(string pFormat, params object[] pArgs)
        {
            Console.Write(pFormat, pArgs);
        }

        public void WriteLine(string pMessage)
        {
            Console.WriteLine(pMessage);
        }

        public void WriteLine(string pFormat, params object[] pArgs)
        {
            Console.WriteLine(pFormat, pArgs);
        }
    }
}
