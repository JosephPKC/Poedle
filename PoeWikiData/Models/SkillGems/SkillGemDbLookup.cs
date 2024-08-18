using PoeWikiData.Models.LookUps;

namespace PoeWikiData.Models.UniqueItems
{
    public class SkillGemDbLookup(IEnumerable<SkillGemDbModel> pModels) : BaseDbModelListLookUp<SkillGemDbModel>(pModels), IModelIdLookUp<SkillGemDbModel>, IModelNameListLookUp<SkillGemDbModel>
    {
        private readonly Dictionary<uint, SkillGemDbModel> _idLookUp = [];
        private readonly Dictionary<string, IList<SkillGemDbModel>> _nameLookUp = [];

        public override IList<SkillGemDbModel> GetAll(bool pIsSorted)
        {
            List<SkillGemDbModel> models = [.. _idLookUp.Values];
            if (pIsSorted)
            {
                models.Sort();
            }
            return models;
        }

        public SkillGemDbModel? GetById(uint pId)
        {
            return GetModel(_idLookUp, pId);
        }

        public IEnumerable<SkillGemDbModel>? GetByName(string pName)
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

        protected override void ProcessModel(SkillGemDbModel pModel)
        {
            _idLookUp.Add(pModel.Id, pModel);
            if (_nameLookUp.TryGetValue(pModel.Name, out IList<SkillGemDbModel>? value))
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
