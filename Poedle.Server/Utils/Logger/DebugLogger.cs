using System.Diagnostics;

using Poedle.Utils.Logger.Writer;

namespace Poedle.Utils.Logger
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
                if (Settings.MakeTimeStamp)
                {
                    string timestamp = DateTime.Now.ToString("HH:mm:ss:ffff");
                    prefix = $"{timestamp}: {prefix}";
                }

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

        public void TimeStartLog(Stopwatch pTimer, string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            pTimer.Start();
            Log(pMessage, pLogLevel);
        }

        public void TimeStopLogAndAppend(Stopwatch pTimer, string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            pTimer.Stop();
            Log($"{pMessage} ELAPSED: {pTimer.ElapsedMilliseconds / 1000.0} ms.", pLogLevel);
        }
    }

    public class DebugLoggerSettings
    {
        public bool IsVerbose = false;
        public bool IsErrorOnly = false;
        public bool IsAutoEndLine = true;
        public bool MakeTimeStamp = true;
    }
}
