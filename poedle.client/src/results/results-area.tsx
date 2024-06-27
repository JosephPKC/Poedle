// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";
import { AttrResultsTable, NameResultsTable } from "./components/results-table.tsx";
// Utils
import { AttrResultList, NameResultList } from "../shared/types/type-defs.ts";

interface AttrResultsTableProps {
    headers: string[],
    results: AttrResultList
}

export function AttrResultsArea({ headers, results }: AttrResultsTableProps) {
    const resultArea = (results == null) ? (<DefaultLoadingText />) : (<AttrResultsTable headers={headers} results={results!} />);

    return (
        <div>{resultArea}</div>
    );
}

interface NameResultsTableProps {
    headers: string[],
    results: NameResultList
}

export function NameResultsArea({ headers, results }: NameResultsTableProps) {
    const resultArea = (results == null) ? (<DefaultLoadingText />) : (<NameResultsTable headers={headers} results={results!} />);

    return (
        <div>{resultArea}</div>
    );
}