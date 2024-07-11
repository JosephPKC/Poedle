using PoeWikiData.Models.UniqueItems;

namespace PoeWikiData.Models.StaticData
{
    /// <summary>
    /// Regular: Key (id) -> Value (name)
    /// Reverse: Key (name) -> Value (id)
    /// </summary>
    internal class UniqueItemDbModelList(IEnumerable<UniqueItemDbModel> pModels) : BaseGenericDbModelList<UniqueItemDbModel, uint, string, string, uint?>(pModels)
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

        public UniqueItemDbModel? GetModelById(uint pId)
        {
            return GetModel(pId);
        }

        public UniqueItemDbModel? GetModelByName(string pName)
        {
            return GetModelRev(pName);
        }

        protected override void ProcessModel(UniqueItemDbModel pModel)
        {
            _modelLookUp.Add(pModel.Id, pModel);
            _reverseModelLookUp.Add(pModel.Name, pModel);
        }

        protected override string? GetVal(uint pKey)
        {
            if (_modelLookUp.TryGetValue(pKey, out UniqueItemDbModel? model)) return model.Name;
            return null;
        }

        protected override uint? GetValRev(string pRevKey)
        {
            if (_reverseModelLookUp.TryGetValue(pRevKey, out UniqueItemDbModel? model)) return model.Id;
            return null;
        }
    }
}
