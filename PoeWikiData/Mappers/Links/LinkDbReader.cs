using System.Data;
using PoeWikiData.Models.Links;

namespace PoeWikiData.Mappers.Links
{
    internal class LinkDbReader
    {
        public static IEnumerable<LinkDbModel> Read(IDataReader pReader)
        {
            ICollection<LinkDbModel> models = [];
            while (pReader.Read())
            {
                LinkDbModel model = new()
                {
                    Id = (uint)pReader.GetInt32(0),
                    LinkId = (uint)pReader.GetInt32(1)
                };
                models.Add(model);
            }
            return models;
        }

        public static IEnumerable<TextLinkDbModel> ReadText(IDataReader pReader)
        {
            ICollection<TextLinkDbModel> models = [];
            while (pReader.Read())
            {
                TextLinkDbModel model = new()
                {
                    Id = (uint)pReader.GetInt32(0),
                    Text = pReader.GetString(1),
                    Order = (ushort)pReader.GetInt16(2)
                };
                models.Add(model);
            }
            return models;
        }
    }
}
