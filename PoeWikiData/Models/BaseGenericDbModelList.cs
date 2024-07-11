namespace PoeWikiData.Models
{
    internal abstract class BaseGenericDbModelList<M, K, V, Kr, Vr> : BaseDbModelList where M : BaseDbModel where K : notnull where Kr : notnull
    {
        protected readonly Dictionary<K, M> _modelLookUp = [];
        protected readonly Dictionary<Kr, M> _reverseModelLookUp = [];

        public BaseGenericDbModelList(IEnumerable<M> pModels)
        {
            foreach (M model in pModels)
            {
                ProcessModel(model);
            }
        }

        protected abstract void ProcessModel(M pModel);
        protected abstract V? GetVal(K pKey);
        protected abstract Vr? GetValRev(Kr pRevKey);

        protected bool HasKey(K pKey)
        {
            return _modelLookUp.ContainsKey(pKey);
        }

        protected bool HasKeyRev(Kr pRevKey)
        {
            return _reverseModelLookUp.ContainsKey(pRevKey);
        }

        protected M? GetModel(K pKey)
        {
            if (_modelLookUp.TryGetValue(pKey, out M? value)) return value;
            return null;
        }

        protected M? GetModelRev(Kr pRevKey)
        {
            if (_reverseModelLookUp.TryGetValue(pRevKey, out M? value)) return value;
            return null;
        }
    }
}
