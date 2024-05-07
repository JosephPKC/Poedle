using Poedle.Game.Controllers.Uniques;
using Poedle.Game.Mappers;
using Poedle.Game.Models.Params;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.Utils.Logger;
using Poedle.Utils.Logger.Writer;
using System.Linq;
using System.Text;

namespace Poedle
{
    public class Program
    {
        public static void Main()
        {
            DebugLogger log = new(new ConsoleWrapper());
            // Set db
            PoeDbManager db = new("..\\..\\..\\..\\Poedle.Server\\PoeDb\\PoeDb.db", log);

            FindUniqueByParamGameController game = new(db, log);

            while (true)
            {
                Console.Write(":::: ");
                string? cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd))
                {
                    continue;
                }

                if (cmd.Equals("LOAD", StringComparison.CurrentCultureIgnoreCase))
                {
                    db.ResetAll();
                }

                if (cmd.Equals("START", StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------------------------------");
                game.StartNewGame();

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------");
                    // Get a guess
                    Console.Write("GUESS: ");
                    string? guess = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(guess))
                    {
                        continue;
                    }

                    if (guess.Equals("HINT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        game.ToggleHints();
                        continue;
                    }

                    FindUniqueByParamGameController.GuessResult? result;
                    if (int.TryParse(guess, out int guessInt))
                    {
                         result = game.MakeGuess(guessInt);
                    }
                    else
                    {
                        result = game.MakeGuess(guess);
                    }

                    // Check if guess is valid
                    if (result == null)
                    {
                        Console.WriteLine($"{guess} is not a valid or was already gussed!");
                        continue;
                    }

                    if (result.Value.IsCorrect)
                    {
                        // Correct - You Win
                        Console.WriteLine(FindUniqueByParamGameController.BuildResultString(result.Value.Params, result.Value.Result, game.AreHintsEnabled));
                        Console.WriteLine($"{guess} was correct! Congrats and thanks for playing!");
                        break;
                    }

                    // Wrong - Try Again
                    // Add to guessed list
                    Console.WriteLine(FindUniqueByParamGameController.BuildResultString(result.Value.Params, result.Value.Result, game.AreHintsEnabled));
                    Console.WriteLine();
                }

                Console.WriteLine("PLAY AGAIN?");
                string? yesno = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(yesno) || yesno != "y")
                {
                    break;
                }
            }
        }
    }
}