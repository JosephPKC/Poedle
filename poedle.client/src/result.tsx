import { DefaultLoadingText } from "./shared/comps/default-loading-text.tsx";
import { TableRowCol } from "./shared/comps/table-row-col.tsx";
import { TableHeader } from "./shared/comps/table-header.tsx";
export enum ResultType {
    Correct = 0,
    Partial = 1,
    Wrong = 2,
    BitHigh = 3,
    BitLow = 4,
    TooHigh = 5,
    TooLow = 6
}

export interface UniqueItemResult {
    readonly id: number,
    readonly name: string,
    readonly itemClass: string,
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
    readonly leaguesIntroducedResult: ResultType,
    readonly itemAspectsResult: ResultType,
    readonly dropSourcesResult: ResultType,
    readonly dropTypesResult: ResultType,
    readonly reqLvlResult: ResultType,
    readonly reqDexResult: ResultType,
    readonly reqIntResult: ResultType,
    readonly reqStrResult: ResultType
}

export interface SkillGemResult {
    readonly id: number,
    readonly name: string,
    readonly primaryAttribute: string,
    readonly dexterityPercent: string,
    readonly intelligencePercent: string,
    readonly strengthPercent: string,
    readonly gemTags: string
    readonly tagAmount: string

    readonly nameResult: ResultType,
    readonly primaryAttributeResult: ResultType,
    readonly dexterityPercentResult: ResultType,
    readonly intelligencePercentResult: ResultType,
    readonly strengthPercentResult: ResultType,
    readonly gemTagsResult: ResultType,
    readonly tagAmountResult: ResultType
}

export type NullableUniqueItemResult = UniqueItemResult | null;
export type NullableUniqueItemResultList = UniqueItemResult[] | null;
export type NullableSkillGemResult = SkillGemResult | null;
export type NullableSkillGemResultList = SkillGemResult[] | null;

const correctClassName: string = "td-green";
const partialClassName: string = "td-orange";
const wrongClassName: string = "td-red";
const bitHighClassName: string = "td-orange td-high";
const bitLowClassName: string = "td-orange td-low";
const tooHighClassName: string = "td-red td-high";
const tooLowClassName: string = "td-red td-low";

export function getColorClassFromResult(pResult: ResultType) {
    switch (pResult) {
        case ResultType.Correct:
            return correctClassName;
        case ResultType.Partial:
            return partialClassName;
        case ResultType.BitHigh:
            return bitHighClassName;
        case ResultType.BitLow:
            return bitLowClassName;
        case ResultType.TooHigh:
            return tooHighClassName;
        case ResultType.TooLow:
            return tooLowClassName;
        case ResultType.Wrong:
        default:
            return wrongClassName;
    }
}

interface ResultsTableDataProps {
    results: UniqueItemResult[]
}

export function ResultsTableData({ results }: ResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: UniqueItemResult) =>
                <tr key={r.id}>
                    <TableRowCol customClassName={getColorClassFromResult(r.nameResult)} text={r.name} />
                    <TableRowCol customClassName={getColorClassFromResult(r.itemClassResult)} text={r.itemClass} />
                    <TableRowCol customClassName={getColorClassFromResult(r.reqLvlResult)} text={r.reqLvl} />
                    <TableRowCol customClassName={getColorClassFromResult(r.reqDexResult)} text={r.reqDex} />
                    <TableRowCol customClassName={getColorClassFromResult(r.reqIntResult)} text={r.reqInt} />
                    <TableRowCol customClassName={getColorClassFromResult(r.reqStrResult)} text={r.reqStr} />
                    <TableRowCol customClassName={getColorClassFromResult(r.itemAspectsResult)} text={r.itemAspects} />
                    <TableRowCol customClassName={getColorClassFromResult(r.leaguesIntroducedResult)} text={r.leaguesIntroduced} />
                    <TableRowCol customClassName={getColorClassFromResult(r.dropSourcesResult)} text={r.dropSources} />
                    <TableRowCol customClassName={getColorClassFromResult(r.dropTypesResult)} text={r.dropTypes} />
                </tr>)}
        </tbody>
    );
}

interface ResultsTableProps {
    headers: string[],
    results: UniqueItemResult[]
}

export function ResultsTable({ headers, results }: ResultsTableProps) {
    return (
        <div className="div-table">
            <table className="table">
                <TableHeader headers={headers} />
                <ResultsTableData results={results!} />
            </table>
        </div>
    );
}

interface ResultsAreaProps {
    headers: string[],
    results: NullableUniqueItemResultList
}

export function ResultsArea({ headers, results }: ResultsAreaProps) {
    if (results == null) {
        return (
            <DefaultLoadingText />
        );
    }

    return (
        <div id= "div-results-area" >
        <ResultsTable headers={ headers } results = { results! } />
            </div>
    );
}

interface ResultsTableDataSkillGemProps {
    results: SkillGemResult[]
}

export function ResultsTableDataSkillGem({ results }: ResultsTableDataSkillGemProps) {
    return (
        <tbody>
            {results.map((r: SkillGemResult) =>
                <tr key={r.id}>
                    <TableRowCol customClassName={getColorClassFromResult(r.nameResult)} text={r.name} />
                    <TableRowCol customClassName={getColorClassFromResult(r.primaryAttributeResult)} text={r.primaryAttribute} />
                    <TableRowCol customClassName={getColorClassFromResult(r.dexterityPercentResult)} text={r.dexterityPercent} />
                    <TableRowCol customClassName={getColorClassFromResult(r.intelligencePercentResult)} text={r.intelligencePercent} />
                    <TableRowCol customClassName={getColorClassFromResult(r.strengthPercentResult)} text={r.strengthPercent} />
                    <TableRowCol customClassName={getColorClassFromResult(r.tagAmountResult)} text={r.tagAmount} />
                    <TableRowCol customClassName={getColorClassFromResult(r.gemTagsResult)} text={r.gemTags} />

                </tr>)}
        </tbody>
    );
}

interface ResultsTableSkillGemProps {
    headers: string[],
    results: SkillGemResult[]
}

export function ResultsTableSkillGem({ headers, results }: ResultsTableSkillGemProps) {
    return (
        <div className="div-table">
            <table className="table">
                <TableHeader headers={headers} />
                <ResultsTableDataSkillGem results={results!} />
            </table>
        </div>
    );
}

interface ResultsAreaSkillGemProps {
    headers: string[],
    results: NullableSkillGemResultList
}

export function ResultsAreaSkillGem({ headers, results }: ResultsAreaSkillGemProps) {
    if (results == null) {
        return (
            <DefaultLoadingText />
        );
    }

    return (
        <div id="div-results-area" >
            <ResultsTableSkillGem headers={headers} results={results!} />
        </div>
    );
}