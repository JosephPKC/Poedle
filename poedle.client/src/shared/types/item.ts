export interface NameItem {
    readonly id: number,
    readonly name: string
}

export interface AttrItem extends NameItem {
    readonly itemClass: string,
    readonly baseItem: string,
    readonly leaguesIntroduced: string,
    readonly itemAspects: string,
    readonly dropSources: string,
    readonly dropSourcesSpecific: string,
    readonly reqLvl: string,
    readonly reqDex: string,
    readonly reqInt: string,
    readonly reqStr: string
}
