using BaseToolsUtils.Logging;
using BaseToolsUtils.Logging.Writers;
using PoeWikiData;

namespace PoeDbUpdate
{
    public class Program
    {
        public static void Main(string[] pArgs)
        {
            if (pArgs.Length == 0)
            {
                Console.WriteLine("No args found. Please put either Update, Reset, or Help.");
                return;
            }

            ConsoleLogger log = new(new ConsoleWriter());
            PoeDbManager db;
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"PoeDb.db");
            string cmd = pArgs[0];
            switch (cmd.ToUpper())
            {
                case "U":
                case "UPDATE":
                    Console.WriteLine($"Updating Db Values for {filePath}");
                    db = new(filePath, true, log);
                    db.UpdateData();
                    Console.WriteLine("Done!");
                    break;
                case "R":
                case "RESET":
                    Console.WriteLine($"Resetting Meta Data for {filePath}");
                    db = new(filePath, false, log);
                    db.ResetMetaData();
                    Console.WriteLine("Done!");
                    break;
                case "H":
                case "HELP":
                    Console.WriteLine("[U]pdate: Fully updates the wiki data values in the database. This does not affect the static data values that are not retrived via the wiki.");
                    Console.WriteLine("[R]eset: Fully resets the meta data in the database. This includes score and statistics, but does not affect the static data values nor the wiki data.");
                    Console.WriteLine("[H]elp: Generates the help menu.");
                    break;
                case "T":
                case "TEST":
                    Console.WriteLine($"Running Tests for {filePath}");
                    db = new(filePath, false, log);
                    RunTests(db);
                    Console.WriteLine("Done!");
                    break;
                default:
                    Console.WriteLine($"'{cmd}' not recognized as a command argument. Please put either Update, Reset, or Help.");
                    break;
            }
        }

        private static void RunTests(PoeDbManager pDb)
        {
            pDb.UpdateData();
            pDb.ResetMetaData();
        }
    }
} 