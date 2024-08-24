import { DataTable } from "../common/data-table/data-table.tsx";
import { NullableTableDataMatrix } from "../common/data-table/table-data.ts";
import { BuildSkillGemResultTableData, BuildUniqueItemResultTableData } from "./result/result-table-data-builder.ts";
import { NullableSkillGemResultList, NullableUniqueItemResultList } from "./result/result.ts";

import "./results-area.css";

interface SkillGemResultAreaProps {
    readonly results: NullableSkillGemResultList
}

export function SkillGemResultsArea({ results }: SkillGemResultAreaProps) {
    const headers: string[] = ["Name", "Primary Attr", "% Dex", "% Int", "% Str", "Tag #", "Tags"];
    const data: NullableTableDataMatrix = BuildSkillGemResultTableData(results);
    const extraStyleClasses: string = "txt-center txt-wrap";
    return (
        <ResultsArea headers={headers} data={data} extraDataRowStyleClasses={extraStyleClasses} />
    );
}

interface UniqueItemResultAreaProps {
    readonly results: NullableUniqueItemResultList
}

export function UniqueItemResultsArea({ results }: UniqueItemResultAreaProps) {
    const headers: string[] = ["Name", "Class", "Rq Lvl", "Rq Dex", "Rq Int", "Rq Str", "Special", "League", "Drop Source", "Drop Type"];
    const data: NullableTableDataMatrix = BuildUniqueItemResultTableData(results);
    const extraStyleClasses: string = "txt-center txt-wrap";
    return (
        <ResultsArea headers={headers} data={data} extraDataRowStyleClasses={extraStyleClasses} />
    );
}

interface ResultAreaProps {
    readonly headers: string[]
    readonly data: NullableTableDataMatrix
    readonly extraDataRowStyleClasses: string
}

function ResultsArea({ headers, data, extraDataRowStyleClasses  }: ResultAreaProps) {
    if (data == null) {
        return (
            <></>
        );
    }

    return (
        <>
            <DataTable headers={headers} tableData={data} extraDataRowStyleClasses={extraDataRowStyleClasses} />
        </>
    );
}
