import { ResultTypes } from "./result-types.ts";

export interface Result {
    readonly value: string
    readonly result: ResultTypes
}

export interface SkillGemResult {
    readonly name: Result
    readonly primaryAttribute: Result
    readonly dexterityPercent: Result
    readonly intelligencePercent: Result
    readonly strengthPercent: Result
    readonly tagAmount: Result
    readonly gemTags: Result
}

export type NullableSkillGemResult = SkillGemResult | null;
export type NullableSkillGemResultList = SkillGemResult[] | null;

export interface UniqueItemResult {
    readonly name: Result
    readonly itemClass: Result
    readonly reqLvl: Result
    readonly reqDex: Result
    readonly reqInt: Result
    readonly reqStr: Result,
    readonly itemAspects: Result
    readonly leaguesIntroduced: Result
    readonly dropSources: Result
    readonly dropTypes: Result

}

export type NullableUniqueItemResult = UniqueItemResult | null;
export type NullableUniqueItemResultList = UniqueItemResult[] | null;