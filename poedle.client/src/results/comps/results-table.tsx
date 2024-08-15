import { ResultsTableData } from "./results-table-data.tsx";
import { ResultsTableHeader } from "./results-table-header.tsx";

import { AttrResult } from "../types/results-type-def.ts";
// Table
interface ResultsTableProps {
    headers: string[],
    results: AttrResult[]
}

export function ResultsTable({ headers, results }: ResultsTableProps) {
    return (
        <table>
            <ResultsTableHeader headers={headers} />
            <ResultsTableData results={results!} />
        </table>
    );
}