export enum ApiTypes {
    None = 0,
    SkillGems = 1,
    UniqueItems = 2
}

export function GetApiBaseUrl(apiType: ApiTypes): string {
    switch (apiType) {
        case ApiTypes.SkillGems:
            return "SkillGems";
        case ApiTypes.UniqueItems:
            return "UniqueItems";
        case ApiTypes.None:
        default:
            return "";
    }
}