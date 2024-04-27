using DebugLogger.Writer;

namespace DebugLogger
{
    public enum LogLevel
    {
        DEBUG, // DEBUG level logs always display, except when ErrorOnly is set.
        ERROR, // ERROR level logs always display.
        VERBOSE // VERBOSE level logs only display when IsVerbose is set.
    }

    public class DebugLogger
    {
        public DebugLoggerSettings Settings { get; private set; }
        protected readonly IConsoleWriter writer;

        public DebugLogger(IConsoleWriter pWriter)
        {
            writer = pWriter;
            Settings = new DebugLoggerSettings();
        }

        public DebugLogger(IConsoleWriter pWriter, DebugLoggerSettings pSettings)
        {
            writer = pWriter;
            Settings = pSettings;
        }

        public void Log(string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            var shouldWrite = false;
            var prefix = "";

            switch(pLogLevel)
            {
                case LogLevel.DEBUG:
                    {
                        shouldWrite = !Settings.IsErrorOnly;
                        prefix = "DEBUG";
                        break;
                    }
                case LogLevel.ERROR:
                    {
                        shouldWrite = true;
                        prefix = "ERROR";
                        break;
                    }
                case LogLevel.VERBOSE:
                    {
                        shouldWrite = Settings.IsVerbose;
                        prefix = "VERBOSE";
                        break;
                    }
            }

            if(shouldWrite)
            {
                if (Settings.IsAutoEndLine)
                {
                    writer.WriteLine($"{prefix}: {pMessage}");
                }
                else
                {
                    writer.Write($"{prefix}: {pMessage}");
                }
            }
        }
    }

    public class DebugLoggerSettings
    {
        public bool IsVerbose = false;
        public bool IsErrorOnly = false;
        public bool IsAutoEndLine = true;
    }
}
