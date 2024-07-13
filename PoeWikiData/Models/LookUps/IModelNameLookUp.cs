namespace PoeWikiData.Models.LookUps
{
    internal interface IModelNameLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        bool HasName(string pName);
        TDbModel? GetByName(string pName);
    }
}
