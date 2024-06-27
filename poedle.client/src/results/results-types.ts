import { AttrItem, NameItem } from "../shared/types/item.ts";

export enum ResultType {
    Correct = 0,
    Partial = 1,
    Wrong = 2
}

interface AttrResult {
    readonly name: ResultType,
    readonly itemClass: ResultType,
    readonly baseItem: ResultType,
    readonly leaguesIntroduced: ResultType,
    readonly itemAspects: ResultType,
    readonly dropSources: ResultType,
    readonly dropSourcesSpecific: ResultType,
    readonly reqLvl: ResultType,
    readonly reqDex: ResultType,
    readonly reqInt: ResultType,
    readonly reqStr: ResultType
}

export interface AttrGuessResult {
    readonly attributes: AttrItem,
    readonly results: AttrResult
}

interface NameResult {
    readonly name: ResultType
}

export interface NameGuessResult {
    readonly attributes: NameItem,
    readonly results: NameResult
}