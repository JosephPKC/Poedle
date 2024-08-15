import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";
import { ResultsTable } from "./comps/results-table.tsx";

import { NullableAttrResultList } from "./types/results-type-def.ts";

interface ResultsAreaProps {
    headers: string[],
    results: NullableAttrResultList
}

export function ResultsArea({ headers, results }: ResultsAreaProps) {
    const resultArea = (results == null) ? (<DefaultLoadingText />) : (<ResultsTable headers={headers} results={results!} />);

    return (
        <div>{resultArea}</div>
    );
}