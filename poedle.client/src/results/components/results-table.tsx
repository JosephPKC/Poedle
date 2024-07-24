// Components
import { ResultsTableHeader } from "./results-table-header.tsx";
import { AttrResultsTableData, NameResultsTableData } from "./results-table-data.tsx";
// Utils
import { AttrResult, NameResult } from "../results-types.ts";

interface AttrResultsTableProps {
    headers: string[],
    results: AttrResult[]
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
    results: NameResult[]
}

export function NameResultsTable({ headers, results }: NameResultsTableProps) {
    return (
        <table>
            <ResultsTableHeader headers={headers} />
            <NameResultsTableData results={results!} />
        </table>
    );
}