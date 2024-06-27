namespace BaseToolsUtils.Logging
{
    public enum LogLevel
    {
        DEBUG,  // DEBUG level logs always display, except when ErrorOnly is set.
        ERROR,  // ERROR level logs always display.
        VERBOSE // VERBOSE level logs only display when IsVerbose is set.
    }
}
