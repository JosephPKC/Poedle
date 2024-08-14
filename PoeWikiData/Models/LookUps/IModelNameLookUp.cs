namespace PoeWikiData.Models.LookUps
{
    public interface IModelNameLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        bool HasName(string pName);
        TDbModel? GetByName(string pName);
    }
}
