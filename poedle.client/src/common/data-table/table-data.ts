export interface TableData {
    readonly text: string | number
    readonly className: string
}

export type TableDataRow = TableData[];
export type TableDataMatrix = TableDataRow[];
export type NullableTableDataMatrix = TableDataMatrix | null;