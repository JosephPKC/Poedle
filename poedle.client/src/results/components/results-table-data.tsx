// Utils
import { AttrResult, NameResult, ResultType } from "../results-types.ts";
import { getColorClassFromResult } from "../utils/results-table-coloring.ts";
// Styles
import "../styles/results-table-data.css";

// Table Row Helpers
interface ResultsTableRowProps {
    guessResult: ResultType,
    text: string | number
}

function ResultsTableRow({ guessResult, text }: ResultsTableRowProps) {
    return (
        <>
            <td className={getColorClassFromResult(guessResult)}>{text}</td>
        </>
    );
}

// Table Data
interface AttrResultsTableDataProps {
    results: AttrResult[]
}

export function AttrResultsTableData({ results }: AttrResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: AttrResult) =>
                <tr key={r.id}>
                    <ResultsTableRow guessResult={r.nameResult} text={r.name} />
                    <ResultsTableRow guessResult={r.itemClassResult} text={r.itemClass} />
                    <ResultsTableRow guessResult={r.baseItemResult} text={r.baseItem} />
                    <ResultsTableRow guessResult={r.leaguesIntroducedResult} text={r.leaguesIntroduced} />
                    <ResultsTableRow guessResult={r.itemAspectsResult} text={r.itemAspects} />
                    <ResultsTableRow guessResult={r.dropSourcesResult} text={r.dropSources} />
                    <ResultsTableRow guessResult={r.dropTypesResult} text={r.dropTypes} />
                    <ResultsTableRow guessResult={r.reqLvlResult} text={r.reqLvl} />
                    <ResultsTableRow guessResult={r.reqDexResult} text={r.reqDex} />
                    <ResultsTableRow guessResult={r.reqIntResult} text={r.reqInt} />
                    <ResultsTableRow guessResult={r.reqStrResult} text={r.reqStr} />
                </tr>)}
        </tbody>
    );
}

interface NameResultsTableDataProps {
    results: NameResult[]
}

export function NameResultsTableData({ results }: NameResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: NameResult) =>
                <tr key={r.id}>
                    <ResultsTableRow guessResult={r.nameResult} text={r.name} />
                </tr>)}
        </tbody>
    );
}