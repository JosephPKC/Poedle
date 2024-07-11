using PoeWikiData.Models.Leagues;
using PoeWikiData.Models.StaticData;

namespace PoeWikiData.Models
{
    internal class ReferenceDataModelGroup
    {
        public StaticDataDbModelList? DropSources { get; set; }
        public StaticDataDbModelList? DropTypes { get; set; }
        public StaticDataDbModelList? ItemAspects { get; set; }
        public StaticDataDbModelList? ItemClasses { get; set; }
        public LeagueDbModelList? Leagues { get; set; }
    }
}
