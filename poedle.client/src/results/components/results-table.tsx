// Components
import { ResultsTableHeader } from "./results-table-header.tsx";
import { AttrResultsTableData, NameResultsTableData } from "./results-table-data.tsx";
// Utils
import { AttrGuessResult, NameGuessResult } from "../results-types.ts";

interface AttrResultsTableProps {
    headers: string[],
    results: AttrGuessResult[]
}
export function AttrResultsTable({ headers, results }: AttrResultsTableProps) {
    return (
        <table>
            <ResultsTableHeader headers={headers} />
            <AttrResultsTableData results={results!} />
        </table>
    );
}

interface NameResultsTableProps {
    headers: string[],
    results: NameGuessResult[]
}

export function NameResultsTable({ headers, results }: NameResultsTableProps) {
    return (
        <table>
            <ResultsTableHeader headers={headers} />
            <NameResultsTableData results={results!} />
        </table>
    );
}