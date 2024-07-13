namespace PoeWikiData.Models.LookUps
{
    internal abstract class BaseDbModelLookUp<TDbModel> where TDbModel : BaseDbModel
    {
        public BaseDbModelLookUp(IEnumerable<TDbModel> pModels)
        {
            foreach (TDbModel model in pModels)
            {
                ProcessModel(model);
            }
        }

        public abstract IEnumerable<TDbModel> GetAll();
        protected abstract void ProcessModel(TDbModel pModel);

        protected static TDbModel? GetModel<TKey>(IDictionary<TKey, TDbModel> pLookUp, TKey pKey) where TKey : notnull
        {
            if (pLookUp.TryGetValue(pKey, out TDbModel? model)) return model;
            return null;
        }
    }
}
