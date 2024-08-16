using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using BaseToolsUtils.Logging.Writers;
using BaseToolsUtils.Logging;

namespace Poedle.Server.Api
{
    [ApiController]
    public abstract class BaseApi : Controller
    {
        private readonly ConsoleLogger _log = new(new ConsoleWriter());

        protected TReturn ProcessApi<TReturn>(string? pRoute, Func<TReturn> pProcessData)
        {
            Stopwatch timer = new();
            string fullRoute = pRoute ?? "";
            _log.TimeStartLog(timer, $"BEGIN: {fullRoute}");

            TReturn result = pProcessData();

            _log.TimeStopLogAndAppend(timer, $"END: {fullRoute}");

            return result;
        }

        protected void ProcessApi(string? pRoute, Action pProcessData)
        {
            Stopwatch timer = new();
            string fullRoute = pRoute ?? "";
            _log.TimeStartLog(timer, $"BEGIN: {fullRoute}");

            pProcessData();

            _log.TimeStopLogAndAppend(timer, $"END: {fullRoute}");
        }
    }
}
