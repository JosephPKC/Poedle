using System.Collections;

namespace PoeWikiData.Models
{
    internal abstract class BaseDbLookUp<TDbModel, TKey, TVal, TRevKey, TRevVal> where TDbModel : BaseDbModel where TKey : notnull where TRevKey : notnull
    {
        protected readonly Dictionary<TKey, TDbModel> _modelLookUp = [];
        protected readonly Dictionary<TRevKey, TDbModel> _reverseModelLookUp = [];

        public BaseDbLookUp(IEnumerable<TDbModel> pModels)
        {
            foreach (TDbModel model in pModels)
            {
                ProcessModel(model);
            }
        }

        protected abstract void ProcessModel(TDbModel pModel);
        protected abstract TVal? GetVal(TKey pKey);
        protected abstract TRevVal? GetValRev(TRevKey pRevKey);

        protected bool HasKey(TKey pKey)
        {
            return _modelLookUp.ContainsKey(pKey);
        }

        protected bool HasRevKey(TRevKey pRevKey)
        {
            return _reverseModelLookUp.ContainsKey(pRevKey);
        }

        protected TDbModel? GetModel(TKey pKey)
        {
            if (_modelLookUp.TryGetValue(pKey, out TDbModel? value)) return value;
            return null;
        }

        protected TDbModel? GetRevModel(TRevKey pRevKey)
        {
            if (_reverseModelLookUp.TryGetValue(pRevKey, out TDbModel? value)) return value;
            return null;
        }

        public IEnumerable<TDbModel> GetAll()
        {
            return [.. _modelLookUp.Values];
        }
    }
}
