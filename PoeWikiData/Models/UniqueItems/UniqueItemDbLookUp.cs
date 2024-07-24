using PoeWikiData.Models.LookUps;

namespace PoeWikiData.Models.UniqueItems
{
    public class UniqueItemDbLookUp(IEnumerable<UniqueItemDbModel> pModels) : BaseDbModelListLookUp<UniqueItemDbModel>(pModels), IModelIdLookUp<UniqueItemDbModel>, IModelNameListLookUp<UniqueItemDbModel>
    {
        private readonly Dictionary<uint, UniqueItemDbModel> _idLookUp = [];
        private readonly Dictionary<string, IList<UniqueItemDbModel>> _nameLookUp = [];

        public override IList<UniqueItemDbModel> GetAll(bool pIsSorted)
        {
            List<UniqueItemDbModel> models = [.. _idLookUp.Values];
            if (pIsSorted)
            {
                models.Sort();
            }
            return models;
        }

        public UniqueItemDbModel? GetById(uint pId)
        {
            return GetModel(_idLookUp, pId);
        }

        public IEnumerable<UniqueItemDbModel>? GetByName(string pName)
        {
            return GetModels(_nameLookUp, pName);
        }

        public IEnumerable<uint>? GetId(string pName)
        {
            return GetByName(pName)?.Select(x => x.Id);
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

        protected override void ProcessModel(UniqueItemDbModel pModel)
        {
            _idLookUp.Add(pModel.Id, pModel);
            if (_nameLookUp.TryGetValue(pModel.Name, out IList<UniqueItemDbModel>? value))
            {
                value.Add(pModel);
            }
            else
            {
                _nameLookUp.Add(pModel.Name, [pModel]);
            }
        }
    }
}
