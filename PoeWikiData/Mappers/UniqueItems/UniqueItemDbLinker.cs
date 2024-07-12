using PoeWikiData.Models.Links;
using PoeWikiData.Models.StaticData;
using PoeWikiData.Models.UniqueItems;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemDbLinker
    {
        public static void AddDropSources(UniqueItemDbModel pModel, IEnumerable<LinkDbModel> pLinks)
        {
            ICollection<StaticDataDbModel> data = [];
            foreach (LinkDbModel link in pLinks)
            {
                StaticDataDbModel? linkData = StaticDataMasterRef.DropSources.GetModelById(link.LinkId);
                if (linkData == null) continue;
                data.Add(linkData);
            }

            pModel.DropSources = data;
        }
    }
}
