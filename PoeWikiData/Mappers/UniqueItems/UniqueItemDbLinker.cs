using PoeWikiData.Models;
using PoeWikiData.Models.Links;
using PoeWikiData.Models.LookUps;

namespace PoeWikiData.Mappers.UniqueItems
{
    internal static class UniqueItemDbLinker
    {
        public static IEnumerable<TDataModel> GetStaticData<TDataModel>(IEnumerable<LinkDbModel> pLinks, IModelIdLookUp<TDataModel> pLookUp) where TDataModel : BaseDbModel
        {
            ICollection<TDataModel> data = [];
            foreach (LinkDbModel link in pLinks)
            {
                TDataModel? linkData = pLookUp.GetById(link.LinkId);
                if (linkData == null) continue;
                data.Add(linkData);
            }

            return data;
        }
       
        public static IEnumerable<string> GetTexts(IEnumerable<TextLinkDbModel> pLinks)
        {
            ICollection<string> data = [];
            foreach (TextLinkDbModel link in pLinks)
            {
                data.Add(link.Text);
            }

            return data;
        }
    }
}
