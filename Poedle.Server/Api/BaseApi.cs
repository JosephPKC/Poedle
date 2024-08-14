using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BaseToolsUtils.Logging.Writers;
using BaseToolsUtils.Logging;

namespace Poedle.Server.Api
{
    [ApiController]
    public abstract class BaseApi
    {
        private readonly ConsoleLogger _log = new(new ConsoleWriter());

        protected TReturn ProcessApi<TReturn>(HttpContext? pContext, Func<TReturn> pProcessData)
        {
            Stopwatch timer = new();
            string fullRoute = pContext?.Request.Path.Value ?? "";
            _log.TimeStartLog(timer, $"BEGIN: {fullRoute}");

            TReturn result = pProcessData();

            _log.TimeStopLogAndAppend(timer, $"END: {fullRoute}");

            return result;
        }

        protected void ProcessApi(HttpContext? pContext, Action pProcessData)
        {
            Stopwatch timer = new();
            string fullRoute = pContext?.Request.Path.Value ?? "";
            _log.TimeStartLog(timer, $"BEGIN: {fullRoute}");

            pProcessData();

            _log.TimeStopLogAndAppend(timer, $"END: {fullRoute}");
        }
    }
}
