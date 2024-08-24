import { NullableString } from "../type-defs.ts";
import { TableData, TableDataMatrix, TableDataRow } from "./table-data.ts";

import "./data-table.css";

interface DataTableProps {
    readonly headers: string[]
    readonly tableData: TableDataMatrix
    readonly extraDataRowStyleClasses: NullableString
}

export function DataTable({ headers, tableData, extraDataRowStyleClasses }: DataTableProps) {
    return (
        <div className="div-data-table">
            <table className="tbl-data-table">
                <DataTableHeaders headers={headers} />
                <DataTableRows tableData={tableData} extraDataRowStyleClasses={extraDataRowStyleClasses} />
            </table>
        </div>
    );
}

interface DataTableHeadersProps {
    headers: string[]
}

function DataTableHeaders({ headers }: DataTableHeadersProps) {
    return (
        <thead>
            <tr>
                {headers.map((h: string) => <th className="th-data-table-header" key={h}>{h}</th>)}
            </tr>
        </thead>
    );
}

interface DataTableRowsProps {
    readonly tableData: TableDataMatrix
    readonly extraDataRowStyleClasses: NullableString
}

function DataTableRows({ tableData, extraDataRowStyleClasses }: DataTableRowsProps) {
    let i = 0, j = 0;
    return (
        <tbody>
            {tableData.map((row: TableDataRow) =>
                <tr key={i++}>
                    {row.map((data: TableData) =>
                        <td key={j++} className={"td-data-table-row " + data.className + " " + extraDataRowStyleClasses ?? ""}>{data.text}</td>
                    )}
                </tr>
            )}
        </tbody>
    );
}
