namespace PoeWikiData.Models.LookUps
{
    internal interface IModelIdLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        bool HasId(uint pId);
        TDbModel? GetById(uint pId);
    }
}
