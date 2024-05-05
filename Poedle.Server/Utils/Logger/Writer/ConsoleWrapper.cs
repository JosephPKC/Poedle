namespace Poedle.Utils.Logger.Writer
{
    public class ConsoleWrapper : IConsoleWriter
    {
        public void Write(string pFormat, params object[] pArgs)
        {
            Console.Write(pFormat, pArgs);
        }

        public void Write(string pMessage)
        {
            Console.Write(pMessage);
        }

        public void WriteLine(string pFormat, params object[] pArgs)
        {
            Console.WriteLine(pFormat, pArgs);
        }

        public void WriteLine(string pMessage)
        {
            Console.WriteLine(pMessage);
        }
    }
}
