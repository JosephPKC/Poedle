namespace PoeWikiData.Models.Leagues
{
    /// <summary>
    /// Regular: Key (release version) -> Value (name)
    /// Reverse: Key (name) -> Value (release version)
    /// </summary>
    /// <param name="pModels"></param>
    internal class LeagueDbLookUp(IEnumerable<LeagueDbModel> pModels) : BaseDbLookUp<LeagueDbModel, string, IEnumerable<string>?, string, string>(pModels)
    {
        public bool HasReleaseVersion(string pVersion)
        {
            return HasKey(pVersion);
        }

        public string GetReleaseVersion(string pName)
        {
            return GetValRev(pName) ?? string.Empty;
        }

        public bool HasLeague(string pName)
        {
            return HasRevKey(pName);
        }

        public IEnumerable<string> GetLeagues(string pVersion)
        {
            return GetVal(pVersion) ?? [];
        }

        public IEnumerable<LeagueDbModel> GetModelsByVersion(string pVersion)
        {
            ICollection<LeagueDbModel> models = [];
            LeagueDbModel? model1 = GetModel(pVersion);
            if (model1 != null)
            {
                models.Add(model1);
            }

            LeagueDbModel? model2 = GetModel(GetAltVersion(pVersion));
            if (model2 != null)
            {
                models.Add(model2);
            }

            return models;
        }

        public LeagueDbModel? GetModelByName(string pName)
        {
            return GetRevModel(pName);
        }

        private static string GetAltVersion(string pVersion)
        {
            return $"{pVersion}.X";
        }

        protected override void ProcessModel(LeagueDbModel pModel)
        {
            if (_modelLookUp.TryGetValue(pModel.ReleaseVersion, out LeagueDbModel? model))
            {
                // Add a .1 to it as unfortunately in the past, each release version had two leagues.
                _modelLookUp.Add(GetAltVersion(pModel.ReleaseVersion), pModel);
            }
            else
            {
                _modelLookUp.Add(pModel.ReleaseVersion, pModel);
            }

            _reverseModelLookUp.Add(pModel.Name, pModel);
        }

        protected override IEnumerable<string>? GetVal(string pKey)
        {
            ICollection<string> result = [];
            if (_modelLookUp.TryGetValue(pKey, out LeagueDbModel? model1))
            {
                result.Add(model1.Name);
            }
            if (_modelLookUp.TryGetValue(GetAltVersion(pKey), out LeagueDbModel? model2))
            {
                result.Add(model2.Name);
            }
            return result;
        }

        protected override string? GetValRev(string pRevKey)
        {
            if (_reverseModelLookUp.TryGetValue(pRevKey, out LeagueDbModel? model)) return model.ReleaseVersion;
            return null;
        }
    }
}
