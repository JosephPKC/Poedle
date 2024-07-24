namespace PoeWikiData.Models.LookUps
{
    public abstract class BaseDbModelLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        public BaseDbModelLookUp(IEnumerable<TDbModel> pModels)
        {
            foreach (TDbModel model in pModels)
            {
                ProcessModel(model);
            }
        }

        public abstract IEnumerable<TDbModel> GetAll(bool pIsSorted);
        protected abstract void ProcessModel(TDbModel pModel);

        protected static TDbModel? GetModel<TKey>(IDictionary<TKey, TDbModel> pLookUp, TKey pKey) where TKey : notnull
        {
            if (pLookUp.TryGetValue(pKey, out TDbModel? model)) return model;
            return null;
        }
    }
}
