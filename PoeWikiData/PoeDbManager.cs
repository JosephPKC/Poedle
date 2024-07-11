using BaseToolsUtils.Caching;
using BaseToolsUtils.Logging;
using PoeWikiApi;
using PoeWikiData.Endpoints.Leagues;
using PoeWikiData.Endpoints.StaticData;
using PoeWikiData.Endpoints.UniqueItems;
using PoeWikiData.Models;
using PoeWikiData.Utils;

namespace PoeWikiData
{
    /*  UPDATE flow
     *  Drop table
     *  Create table
     *  Get from API
     *  Transform API model into workable db model
     *  Transform db model into a list of strings and insert into table
     *  UpdateAll
     *      UpdateLeagues
     *      UpdateUniqueItems
     *      UpdateLinks
     *  RESET flow
     *  Drop table
     *  Create table
     *  Set defaults
     */

    /*  SELECT flow
     *  Select from table
     *  Transform reader data into a db model and return that
     */

    /*  Separation
     *  SQLiteDbWrapper - Wraps the db and allows for Select, Create, Drop, and Insert.
     *  Endpoints - Takes in the wrapper to do specific table modification for Updates, Resets, and Select
     *  Models - Db models for return. It should match the table(s)/view it is emulating exactly as much as possible
     *  Mappers - Transforms wiki api data into proper db models
     *  SQLiteMappers - Transforms db models into a list of strings for insertion
     *  Schemas - Reference data to use when doing queries, specifically for the Update. It is so we can store all of the schema structure and info in one place instead of it being scattered and repeated all over
     */
    public sealed class PoeDbManager
    {
        private readonly PoeDbHandler _db;
        private readonly ConsoleLogger _log;
        private readonly CacheHandler<string, BaseModel> _cache;
        private readonly PoeWikiManager _api;

        private readonly StaticDataDbEndpointGroup _staticData;
        private readonly LeagueDbEndpointGroup _league;
        private readonly UniqueItemDbEndpointGroup _uniqueItem;

        public PoeDbManager(string pDbFilePath, bool pIsNewDb, ConsoleLogger pLogger)
        {
            _cache = new();
            _api = new(pLogger);
            _log = pLogger;

            _db = new(pDbFilePath, pIsNewDb, _cache, pLogger);
            _staticData = new(_db, _cache, pLogger);
            _league = new(_db, _cache, pLogger);
            _uniqueItem = new(_db, _cache, pLogger);
        }

        public void UpdateData()
        {
            // Update static data.
            // Can use reflection on enums to get id and name to update the static data tables

            _league.Update.Update(_api);

            ReferenceDataModelGroup refData = new()
            {
                DropSources = _staticData.Select.SelectAllDropSources(),
                DropTypes = _staticData.Select.SelectAllDropTypes(),
                ItemAspects = _staticData.Select.SelectAllItemAspects(),
                ItemClasses = _staticData.Select.SelectAllItemClasses(),
                Leagues = _league.Select.SelectAll()
            };

            _uniqueItem.Update.Update(_api, refData);
        }

        public void ResetMetaData()
        {
            // Drop then Create
        }

    }
}
