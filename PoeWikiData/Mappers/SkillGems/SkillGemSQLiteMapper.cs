using System.Data;

using PoeWikiData.Models;
using PoeWikiData.Models.UniqueItems;
using PoeWikiData.Utils.SQLite;

namespace PoeWikiData.Mappers.SkillGems
{
    internal static class SkillGemSQLiteMapper
    {
        public static SQLiteValues Map(SkillGemDbModel pModel)
        {
            List<string> values =
            [
                pModel.Id.ToString(),
                SQLiteUtils.SQLiteString(pModel.Name),
                SQLiteUtils.SQLiteString(pModel.DisplayName),
                SQLiteUtils.SQLiteString(pModel.GemDescription),
                SQLiteUtils.SQLiteString(pModel.PrimaryAttribute),
                pModel.DexterityPercent.ToString(),
                pModel.IntelligencePercent.ToString(),
                pModel.StrengthPercent.ToString()
            ];
            return new(values);
        }

        public static IEnumerable<SkillGemDbModel> Read(IDataReader pReader)
        {
            ICollection<SkillGemDbModel> models = [];
            while (pReader.Read())
            {
                SkillGemDbModel model = new()
                {
                    Id = (uint)pReader.GetInt32(0),
                    Name = pReader.GetString(1),
                    DisplayName = pReader.GetString(2),
                    GemDescription = pReader.GetString(3),
                    PrimaryAttribute = pReader.GetString(4),
                    DexterityPercent = (uint)pReader.GetInt32(5),
                    IntelligencePercent = (uint)pReader.GetInt32(6),
                    StrengthPercent = (uint)pReader.GetInt32(7)
                };
                models.Add(model);
            }
            return models;
        }
    }
}
