namespace PoeWikiData.Models.LookUps
{
    internal interface IModelNameListLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        bool HasName(string pName);
        IEnumerable<TDbModel>? GetByName(string pName);
    }
}
