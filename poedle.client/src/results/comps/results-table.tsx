import { AttrResultsTableData, NameResultsTableData } from "./results-table-data.tsx";
import { ResultsTableHeader } from "./results-table-header.tsx";

import { AttrResult, NameResult } from "../types/results-type-def.ts";
// Table
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