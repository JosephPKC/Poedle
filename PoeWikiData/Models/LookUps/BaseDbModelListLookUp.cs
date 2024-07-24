namespace PoeWikiData.Models.LookUps
{
    public abstract class BaseDbModelListLookUp<TDbModel>(IEnumerable<TDbModel> pModels) : BaseDbModelLookUp<TDbModel>(pModels) where TDbModel : BaseDbModel
    {
        protected static IEnumerable<TDbModel>? GetModels<TKey>(IDictionary<TKey, IList<TDbModel>> pLookUp, TKey pKey) where TKey : notnull
        {
            if (pLookUp.TryGetValue(pKey, out IList<TDbModel>? models)) return models;
            return null;
        }
    }
}
