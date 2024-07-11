namespace PoeWikiData.Models.StaticData
{
    /// <summary>
    /// Regular: Key (id) -> Value (name)
    /// Reverse: Key (name) -> Value (id)
    /// </summary>
    internal class StaticDataDbModelList(IEnumerable<StaticDataDbModel> pModels) : BaseGenericDbModelList<StaticDataDbModel, uint, string, string, uint?>(pModels)
    {
        public bool HasId(uint pId)
        {
            return HasKey(pId);
        }

        public uint? GetId(string pName)
        {
            return GetValRev(pName);
        }

        public bool HasName(string pName)
        {
            return HasKeyRev(pName);
        }

        public string GetName(uint pId)
        {
            return GetVal(pId) ?? "";
        }

        public StaticDataDbModel? GetModelById(uint pId)
        {
            return GetModel(pId);
        }

        public StaticDataDbModel? GetModelByName(string pName)
        {
            return GetModelRev(pName);
        }

        protected override void ProcessModel(StaticDataDbModel pModel)
        {
            _modelLookUp.Add(pModel.Id, pModel);
            _reverseModelLookUp.Add(pModel.Name, pModel);
        }

        protected override string? GetVal(uint pKey)
        {
            if (_modelLookUp.TryGetValue(pKey, out StaticDataDbModel? model)) return model.Name;
            return null;
        }

        protected override uint? GetValRev(string pRevKey)
        {
            if (_reverseModelLookUp.TryGetValue(pRevKey, out StaticDataDbModel? model)) return model.Id;
            return null;
        }
    }
}
