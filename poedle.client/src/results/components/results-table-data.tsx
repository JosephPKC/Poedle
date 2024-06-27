// Utils
import { AttrGuessResult, NameGuessResult, ResultType } from "../results-types.ts";
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
    results: AttrGuessResult[]
}

export function AttrResultsTableData({ results }: AttrResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: AttrGuessResult) =>
                <tr key={r.attributes.id}>
                    <ResultsTableRow guessResult={r.results.name} text={r.attributes.name} />
                    <ResultsTableRow guessResult={r.results.itemClass} text={r.attributes.itemClass} />
                    <ResultsTableRow guessResult={r.results.baseItem} text={r.attributes.baseItem} />
                    <ResultsTableRow guessResult={r.results.leaguesIntroduced} text={r.attributes.leaguesIntroduced} />
                    <ResultsTableRow guessResult={r.results.itemAspects} text={r.attributes.itemAspects} />
                    <ResultsTableRow guessResult={r.results.dropSources} text={r.attributes.dropSources} />
                    <ResultsTableRow guessResult={r.results.dropSourcesSpecific} text={r.attributes.dropSourcesSpecific} />
                    <ResultsTableRow guessResult={r.results.reqLvl} text={r.attributes.reqLvl} />
                    <ResultsTableRow guessResult={r.results.reqDex} text={r.attributes.reqDex} />
                    <ResultsTableRow guessResult={r.results.reqInt} text={r.attributes.reqInt} />
                    <ResultsTableRow guessResult={r.results.reqStr} text={r.attributes.reqStr} />
                </tr>)}
        </tbody>
    );
}

interface NameResultsTableDataProps {
    results: NameGuessResult[]
}

export function NameResultsTableData({ results }: NameResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: NameGuessResult) =>
                <tr key={r.attributes.id}>
                    <ResultsTableRow guessResult={r.results.name} text={r.attributes.name} />
                </tr>)}
        </tbody>
    );
}