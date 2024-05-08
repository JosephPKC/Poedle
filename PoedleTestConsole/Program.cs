using LiteDB;
using Poedle.Game.Controllers.Uniques;
using Poedle.Game.Mappers;
using Poedle.Game.Models.Params;
using Poedle.PoeDb;
using Poedle.PoeDb.Models;
using Poedle.PoeWiki;
using Poedle.Utils.Logger;
using Poedle.Utils.Logger.Writer;
using Poedle.Utils.Strings;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Poedle
{
    public class Program
    {
        public static void Main()
        {

            DebugLogger log = new(new ConsoleWrapper());

            while (true)
            {
                Console.WriteLine("TEST STAT TEXT?");
                string yn = Console.ReadLine();
                if (yn.ToLower() != "y") break;
                ReviewStatText(log);
                Console.WriteLine("READY?");
                string r = Console.ReadLine();
                break;
            }

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

                if (cmd.Equals("TEST", StringComparison.CurrentCultureIgnoreCase))
                {
                    var r = "<th class=\"mw-customtoggle-31\">[\\w\\d\\s\"-=\\>\\<]+</th>";
                    var s = Regex.Match("<table class=\"random-modifier-stats mw-collapsed\" style=\"text-align: left\"><tr><th class=\"mw-customtoggle-31\"><Random Intelligence aura> has no Reservation</th></tr><tr class=\"mw-collapsible mw-collapsed\" id=\"mw-customcollapsible-31\"><td>Clarity has no Reservation<hr style=\"width: 20%\">Discipline has no Reservation<hr style=\"width: 20%\">Malevolence has no Reservation<hr style=\"width: 20%\">Purity of Elements has no Reservation<hr style=\"width: 20%\">Purity of Lightning has no Reservation<hr style=\"width: 20%\">Wrath has no Reservation<hr style=\"width: 20%\">Zealotry has no Reservation</td></tr></table>", r);
                    log.Log(s.Groups[0].Value);

                    s = Regex.Match("(30-50)% increased Spell Damage\",\"(180-220)% increased Energy Shield\",\"Spectres have (50-100)% increased maximum Life\",\"Gain Arcane Surge when you deal a Critical Strike\",\"Your Raised Spectres also gain Arcane Surge when you do\",\"(40-50)% increased Critical Strike Chance for Spells per Raised Spectre\",\"<table class=\"random-modifier-stats mw-collapsed\" style=\"text-align: left\"><tr><th class=\"mw-customtoggle-31\"><span class=\"veiled -prefix\">Veiled Prefix</span></th></tr><tr class=\"mw-collapsible mw-collapsed\" id=\"mw-customcollapsible-31\"><td>Adds (14-16) to (20-22) Cold Damage\",\"Adds (14-16) to (20-22) Lightning Damage<hr style=\"width: 20%\">Adds (14-16) to (20-22) Fire Damage\",\"Adds (14-16) to (20-22) Cold Damage<hr style=\"width: 20%\">Adds (14-16) to (20-22) Fire Damage\",\"Adds (14-16) to (20-22) Lightning Damage<hr style=\"width: 20%\">(24-28)% increased Energy Shield\",\"+(19-22) to maximum Life<hr style=\"width: 20%\">+2 to Level of Socketed Support Gems\",\"+(5-8)% to Quality of Socketed Support Gems<hr style=\"width: 20%\">+(3201-4000) to Armour during Soul Gain Prevention</td></tr></table>", r);
                    log.Log(s.Groups[0].Value);
                }

                if (cmd.Equals("REVIEW", StringComparison.CurrentCultureIgnoreCase))
                {
                    List<DbUnique> MarkForReview(ILiteCollection<DbUnique> pCol)
                    {
                        List<DbUnique> allToReview = pCol.Query().Where(x => x.ExplicitStatText.Count > 0).ToList();
                        log.Log($"FOUND {allToReview.Count} docs to review.");
                        List<DbUnique> toMark = [];
                        foreach (var x in allToReview)
                        {
                            log.Log($"Reviewing {x.Name}.");
                            foreach (var s in x.ExplicitStatText)
                            {
                                if (s == null)
                                {
                                    continue;
                                }
                                if (s.Contains("<") || s.Contains(">") || s.Contains(";"))
                                {
                                    log.Log($"FOUND STAT TEXT TO REVIEW.");
                                    toMark.Add(x);
                                }
                            }
                        }

                        return toMark;
                    }
                    db.InternalMarkForReview<DbUnique>("uniques", MarkForReview);
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

        private static void ReviewStatText(DebugLogger log)
        {
            var api = new PoeWikiApi(log);
            var allU = api.Unique.GetAll();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            foreach (var u in allU) 
            {
                string text = GetCleanedStatText(u.ExplicitStatText);
                Console.WriteLine("----------------------");
                Console.WriteLine(u.Name);
                if (ContainsTags(text))
                {
                    Console.WriteLine($"{text}");
                    Console.ReadLine();
                }
            }
        }

        private static string GetCleanedStatText(string pStatText)
        {
            // Clean up [[word1|word2].
            string cleanedText = HTMLTagCleaner.ReplaceBracketGroupWithSecond(pStatText);
            // Clean up <hr style="width: 20%">
            cleanedText = cleanedText.Replace("<hr style=\"width: 20%\">", "");
            // Do generic parsing after as it is easier to find the above variants with the prefix '[['
            cleanedText = HTMLTagCleaner.ParseBasicHTMLTags(cleanedText);
            // Clean up <span class="item-stat-separator -unique">...</span>
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "item-stat-separator -unique", "\n");
            // Clean up <span class="hoverbox c-tooltip"></span>
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox__activator c-tooltip__activator", "");
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox__display c-tooltip__display", "");
            cleanedText = HTMLTagCleaner.ReplaceSpanClass(cleanedText, "hoverbox c-tooltip", "");
            // Clean up <span class="tc -default">...</span> and "tc -value", which is used to set a font for certain text.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -default");
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -value");
            // Clean up <span class="tc -corrupted">...</span>. Usually for the Forbidden jewels.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "tc -corrupted");
            // Clean up <em class="tc -corrupted">Corrupted</em>. This adds the Corrupted line to the stat text, but it is not needed.
            cleanedText = cleanedText.Replace("<em class=\"tc -corrupted \">Corrupted</em>", "");
            // Clean up <table class="...">...</table> with its header inner text. Usually, for the mod lines that can have multiple options.
            // Clean up multi mod lines (i.e. Aul's Uprising has a line that can be one of many different mods).
            cleanedText = HTMLTagCleaner.ReplaceTableClassWithHeader(cleanedText);
            // Clean up <span class="veiled -suffix"></span>. For veiled mods.
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -prefix");
            cleanedText = HTMLTagCleaner.ReplaceSpanClassWithInnerText(cleanedText, "veiled -suffix");
            // Clean up <abbr title="...">...</abbr> tags
            cleanedText = HTMLTagCleaner.ReplaceAbbrWithInnerText(cleanedText);
            // Clean up <veiled mod pool> that gets left behind sometimes
            cleanedText = cleanedText.Replace("<veiled mod pool>", "");
            //// Clean up double bracket tags <<...>>
            cleanedText = HTMLTagCleaner.ParseDoubleBracketTags(cleanedText);

            return cleanedText;
        }

        private static bool ContainsTags(string str)
        {
            return str.Contains("<span") || str.Contains("<table") || str.Contains("<abbr");
        }
    }
}