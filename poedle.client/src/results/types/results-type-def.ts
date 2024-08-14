export enum ResultType {
    Correct = 0,
    Partial = 1,
    Wrong = 2
}

export interface AttrResult {
    readonly id: number,
    readonly name: string,
    readonly itemClass: string,
    readonly baseItem: string,
    readonly leaguesIntroduced: string,
    readonly itemAspects: string,
    readonly dropSources: string,
    readonly dropTypes: string,
    readonly reqLvl: string,
    readonly reqDex: string,
    readonly reqInt: string,
    readonly reqStr: string

    readonly nameResult: ResultType,
    readonly itemClassResult: ResultType,
    readonly baseItemResult: ResultType,
    readonly leaguesIntroducedResult: ResultType,
    readonly itemAspectsResult: ResultType,
    readonly dropSourcesResult: ResultType,
    readonly dropTypesResult: ResultType,
    readonly reqLvlResult: ResultType,
    readonly reqDexResult: ResultType,
    readonly reqIntResult: ResultType,
    readonly reqStrResult: ResultType
}

export interface NameResult {
    readonly id: number,
    readonly name: string
    readonly nameResult: ResultType
}

export type NullableAttrResult = AttrResult | null;
export type NullableNameResult = NameResult | null;
export type NullableAttrResultList = AttrResult[] | null;
export type NullableNameResultList = NameResult[] | null;