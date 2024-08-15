import { AttrResult, ResultType } from "../types/results-type-def.ts";
import { getColorClassFromResult } from "../utils/results-table-coloring.ts";

import "../styles/results-table-row.css";

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
interface ResultsTableDataProps {
    results: AttrResult[]
}

export function ResultsTableData({ results }: ResultsTableDataProps) {
    return (
        <tbody>
            {results.map((r: AttrResult) =>
                <tr key={r.id}>
                    <ResultsTableRow guessResult={r.nameResult} text={r.name} />
                    <ResultsTableRow guessResult={r.itemClassResult} text={r.itemClass} />
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