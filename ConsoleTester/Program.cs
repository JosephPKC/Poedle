using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using PoeWikiApi;
using PoeWikiData.Utils;
using System.Data.SQLite;

namespace Poedle
{
    public class Program
    {
        public static void Main()
        {
            var logger = new ConsoleLogger(new ConsoleWriter());
            var api = new PoeWikiManager(logger);
            //var allItems = api.Uniques.GetAll();
            //foreach(var item in allItems)
            //{
            //    Console.WriteLine($"{item.Id}: {item.Name}");
            //}

            var db = new PoeDbHandler("../TestDb.db", false, logger);
            db.Test();
        }
    }
}