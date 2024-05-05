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

            db.ResetAll();

            FindUniqueByParamGameController game = new(db, log);

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
                    string guess = Console.ReadLine();
                    int guessInt;

                    FindUniqueByParamGameController.GuessResult? result;
                    if (int.TryParse(guess, out guessInt))
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
                        Console.WriteLine(FindUniqueByParamGameController.BuildResultString(result.Value.Params, result.Value.Result));
                        Console.WriteLine($"{guess} was correct! Congrats and thanks for playing!");
                        break;
                    }

                    // Wrong - Try Again
                    // Add to guessed list
                    Console.WriteLine(FindUniqueByParamGameController.BuildResultString(result.Value.Params, result.Value.Result));
                    Console.WriteLine();
                }

                Console.WriteLine("PLAY AGAIN?");
                string yesno = Console.ReadLine();
                if(yesno != "y")
                {
                    break;
                }
            }
        }
    }
}