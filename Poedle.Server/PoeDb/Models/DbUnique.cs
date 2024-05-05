using Poedle.Enums;

namespace Poedle.PoeDb.Models
{
    public class DbUnique : BaseDbPoeModel
    {
        public ItemClassesEnum.ItemClasses ItemClass { get; set; } = ItemClassesEnum.ItemClasses.NONE;
        public string BaseItem { get; set; } = "";
        public List<LeaguesEnum.Leagues> LeaguesIntroduced { get; set; } = [];
        public List<string> FlavourText { get; set; } = [];
        public List<string> StatText { get; set; } = [];

        public ushort ReqLvl { get; set; }
        public ushort ReqDex { get; set; }
        public ushort ReqInt { get; set; }
        public ushort ReqStr { get; set; }

        // Corrupted, Fractured, Replica, Synthesised, Veiled, Eater Influence, Exarch Influence
        public List<QualitiesEnum.Qualities> Qualities { get; set; } = [];
        // Boss Drop, Temple of Atzoatl, Corruption Altar, Vendor Recipe, Labyrinth, Abyssal Depths, Blighted Maps, Breach, Simulacrum, Expedition, Harbinger, Heist Curio, Domain of TImeless Conflict, Ritual Altar, Sanctum, Synthesis Map, Ultimatum Trial, Warband, Fishing
        public List<DropSourcesEnum.DropSources> DropSources { get; set; } = [];
    }
}
