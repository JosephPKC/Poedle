using System.Diagnostics;

namespace BaseToolsUtils.Logging
{
    public class ConsoleLogger
    {
        public ConsoleLoggerSettings Settings { get; private set; }
        protected readonly Writers.ILogWriter writer;

        public ConsoleLogger(Writers.ILogWriter pWriter)
        {
            writer = pWriter;
            Settings = new ConsoleLoggerSettings();
        }

        public ConsoleLogger(Writers.ILogWriter pWriter, ConsoleLoggerSettings pSettings)
        {
            writer = pWriter;
            Settings = pSettings;
        }

        public void Log(string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            if (!Settings.IsEnableLogging) return;
            if (!IsShouldWriteLog(pLogLevel)) return;

            string prefix = GetPrefix(pLogLevel);
            string logMsg = $"{prefix}: {pMessage}";

            if (Settings.IsAutoEndLine)
            {
                writer.WriteLine(logMsg);
            }
            else
            {
                writer.Write(logMsg);
            }
        }

        public void TimeStartLog(Stopwatch pTimer, string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            if (!Settings.IsEnableLogging) return;

            pTimer.Start();
            Log(pMessage, pLogLevel);
        }

        public void TimeStopLogAndAppend(Stopwatch pTimer, string pMessage, LogLevel pLogLevel = LogLevel.DEBUG)
        {
            if (!Settings.IsEnableLogging) return;

            pTimer.Stop();
            Log($"{pMessage} ELAPSED: {pTimer.ElapsedMilliseconds / 1000.0} ms.", pLogLevel);
        }

        private string GetPrefix(LogLevel pLogLevel)
        {
            string prefix = pLogLevel switch
            {
                LogLevel.DEBUG => "DEBUG",
                LogLevel.ERROR => "ERROR",
                LogLevel.VERBOSE => "VERBOSE",
                _ => string.Empty,
            };

            if (Settings.MakeTimeStamp)
            {
                prefix = $"{DateTime.Now:HH:mm:ss:ffff}: {prefix}";
            }

            return prefix ;
        }

        private bool IsShouldWriteLog(LogLevel pLogLevel)
        {
            return pLogLevel switch
            {
                LogLevel.DEBUG => !Settings.IsErrorOnly,
                LogLevel.ERROR => true,
                LogLevel.VERBOSE => Settings.IsVerbose,
                _ => false,
            };
        }
    }

    public class ConsoleLoggerSettings
    {
        public bool IsEnableLogging = true;
        public bool IsVerbose = false;
        public bool IsErrorOnly = false;
        public bool IsAutoEndLine = true;
        public bool MakeTimeStamp = true;
    }
}
