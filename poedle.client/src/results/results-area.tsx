import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";
import { AttrResultsTable, NameResultsTable } from "./comps/results-table.tsx";

import { NullableAttrResultList, NullableNameResultList } from "./types/results-type-def.ts";

interface AttrResultsTableProps {
    headers: string[],
    results: NullableAttrResultList
}

export function AttrResultsArea({ headers, results }: AttrResultsTableProps) {
    const resultArea = (results == null) ? (<DefaultLoadingText />) : (<AttrResultsTable headers={headers} results={results!} />);

    return (
        <div>{resultArea}</div>
    );
}

interface NameResultsTableProps {
    headers: string[],
    results: NullableNameResultList
}

export function NameResultsArea({ headers, results }: NameResultsTableProps) {
    const resultArea = (results == null) ? (<DefaultLoadingText />) : (<NameResultsTable headers={headers} results={results!} />);

    return (
        <div>{resultArea}</div>
    );
}