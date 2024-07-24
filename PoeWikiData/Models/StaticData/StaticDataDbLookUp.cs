using PoeWikiData.Models.LookUps;
using System.Reflection;

namespace PoeWikiData.Models.StaticData
{
    /// <summary>
    /// Regular: Key (id) -> Value (name)
    /// Reverse: Key (name) -> Value (id)
    /// </summary>
    internal class StaticDataDbLookUp(IEnumerable<StaticDataDbModel> pModels) : BaseDbModelLookUp<StaticDataDbModel>(pModels), IModelIdLookUp<StaticDataDbModel>, IModelNameLookUp<StaticDataDbModel>
    {
        private readonly Dictionary<uint, StaticDataDbModel> _idLookUp = [];
        private readonly Dictionary<string, StaticDataDbModel> _nameLookUp = [];

        public override IList<StaticDataDbModel> GetAll(bool pIsSorted)
        {
            List<StaticDataDbModel> models = [.. _idLookUp.Values];
            if (pIsSorted)
            {
                models.Sort();
            }
            return models;
        }

        public StaticDataDbModel? GetById(uint pId)
        {
            return GetModel(_idLookUp, pId);
        }

        public StaticDataDbModel? GetByName(string pName)
        {
            return GetModel(_nameLookUp, pName);
        }

        public uint? GetId(string pName)
        {
            return GetByName(pName)?.Id;
        }

        public string? GetName(uint pId)
        {
            return GetById(pId)?.Name;
        }

        public bool HasId(uint pId)
        {
            return _idLookUp.ContainsKey(pId);
        }

        public bool HasName(string pName)
        {
            return _nameLookUp.ContainsKey(pName);
        }

        protected override void ProcessModel(StaticDataDbModel pModel)
        {
            _idLookUp.Add(pModel.Id, pModel);
            _nameLookUp.Add(pModel.Name, pModel);
        }
    }
}
