using PoeWikiData.Models.Common;
using PoeWikiData.Models.LookUps;
using System.Reflection;

namespace PoeWikiData.Models.Leagues
{
    internal class LeagueDbLookUp(IEnumerable<LeagueDbModel> pModels) : BaseDbModelListLookUp<LeagueDbModel>(pModels), IModelIdLookUp<LeagueDbModel>, IModelNameLookUp<LeagueDbModel>
    {
        private readonly Dictionary<uint, LeagueDbModel> _idLookUp = [];
        private readonly Dictionary<string, LeagueDbModel> _nameLookUp = [];
        private readonly Dictionary<string, IList<LeagueDbModel>> _versionLookUp = [];

        public override IList<LeagueDbModel> GetAll(bool pIsSorted)
        {
            List<LeagueDbModel> models = [.. _idLookUp.Values];
            if (pIsSorted)
            {
                models.Sort();
            }
            return models;
        }

        public LeagueDbModel? GetById(uint pId)
        {
            return GetModel(_idLookUp, pId);
        }

        public LeagueDbModel? GetByName(string pName)
        {
            return GetModel(_nameLookUp, pName);
        }

        public IEnumerable<LeagueDbModel>? GetByVersion(DbVersion pVersion, bool pIsMajorMinorOnly = false)
        {
            return GetModels(_versionLookUp, pIsMajorMinorOnly ? pVersion.MajorMinorText : pVersion.FullText);
        }

        public uint? GetId(string pName)
        {
            return GetByName(pName)?.Id;
        }

        public IEnumerable<uint>? GetId(DbVersion pVersion)
        {
            return GetByVersion(pVersion)?.Select(x => x.Id);
        }

        public string? GetName(uint pId)
        {
            return GetById(pId)?.Name;
        }

        public IEnumerable<string>? GetName(DbVersion pVersion)
        {
            return GetByVersion(pVersion)?.Select(x => x.Name);
        }

        public DbVersion? GetVersion(uint pId)
        {
            return GetById(pId)?.ReleaseVersion;
        }

        public DbVersion? GetVersion(string pName)
        {
            return GetByName(pName)?.ReleaseVersion;
        }

        public bool HasId(uint pId)
        {
            return _idLookUp.ContainsKey(pId);
        }

        public bool HasName(string pName)
        {
            return _nameLookUp.ContainsKey(pName);
        }

        public bool HasVersion(string pVersion)
        {
            return _versionLookUp.ContainsKey(pVersion);
        }

        protected override void ProcessModel(LeagueDbModel pModel)
        {
            _idLookUp.Add(pModel.Id, pModel);
            _nameLookUp.Add(pModel.Name, pModel);
            if (_versionLookUp.TryGetValue(pModel.ReleaseVersion.MajorMinorText, out IList<LeagueDbModel>? value))
            {
                value.Add(pModel);
            }
            else
            {
                _versionLookUp.Add(pModel.ReleaseVersion.MajorMinorText, [pModel]);
            }
        }
    }
}
