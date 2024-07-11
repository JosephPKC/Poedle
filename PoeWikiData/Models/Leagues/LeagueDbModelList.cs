namespace PoeWikiData.Models.Leagues
{
    /// <summary>
    /// Regular: Key (release version) -> Value (name)
    /// Reverse: Key (name) -> Value (release version)
    /// </summary>
    /// <param name="pModels"></param>
    internal class LeagueDbModelList(IEnumerable<LeagueDbModel> pModels) : BaseGenericDbModelList<LeagueDbModel, string, List<string>, string, string>(pModels)
    {
        public bool HasReleaseVersion(string pVersion)
        {
            return HasKey(pVersion);
        }

        public string GetReleaseVersion(string pName)
        {
            return GetValRev(pName) ?? "";
        }

        public bool HasLeague(string pName)
        {
            return HasKeyRev(pName);
        }

        public List<string> GetLeagues(string pVersion)
        {
            return GetVal(pVersion) ?? [];
        }

        public List<LeagueDbModel> GetModelsByVersion(string pVersion)
        {
            List<LeagueDbModel> models = [];
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
            return GetModelRev(pName);
        }

        private static string GetAltVersion(string pVersion)
        {
            return $"{pVersion}.1";
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

        protected override List<string>? GetVal(string pKey)
        {
            List<string> result = [];
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
