﻿using BaseToolsUtils.Logging;
using PoeWikiApi.Endpoints;
using PoeWikiApi.Utils;

namespace PoeWikiApi
{
    public class PoeWikiManager
    {
        private static readonly long _cacheSizeLimit = 1024;
        private readonly HttpRetriever _http;
        private readonly CacheHandler<string, string> _cache;
        private readonly ConsoleLogger _log;

        #region "Endpoints"
        public UniqueItemEndpoint Uniques { get; private set; }
        #endregion

        public PoeWikiManager(ConsoleLogger pLogger)
        {
            _http = new();
            _cache = new(_cacheSizeLimit);
            _log = pLogger;

            Uniques = new(_http, _cache, _log);
        }

        public void FlushCache()
        {
            _cache.Flush(_cacheSizeLimit);
        }
    }
}
