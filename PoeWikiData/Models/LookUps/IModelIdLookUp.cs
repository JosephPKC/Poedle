namespace PoeWikiData.Models.LookUps
{
    public interface IModelIdLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        bool HasId(uint pId);
        TDbModel? GetById(uint pId);
    }
}
