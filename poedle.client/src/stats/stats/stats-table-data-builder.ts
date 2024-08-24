import { TableData, TableDataMatrix, TableDataRow } from "../../common/data-table/table-data";
import { TableStats } from "./stats";

export function BuildStatsTableData(stats: TableStats[]): TableDataMatrix {
    let tableDataMatrix: TableDataRow[] = [];
    for (let i: number = 0; i < stats.length; i++) {
        const tableStats: TableStats = stats[i];
        const statData: string[] = [tableStats.answer, tableStats.bestScore, tableStats.worstScore, tableStats.averageScore, tableStats.totalGames];
        const tableDataRow: TableDataRow = GetTableDataRow(statData);
        tableDataMatrix.push(tableDataRow);
    }

    return tableDataMatrix;
}

function GetTableDataRow(stats: string[]): TableDataRow {
    const row: TableDataRow = stats.map((s: string) => GetTableData(s));
    return row;
}

function GetTableData(stat: string): TableData {
    const cell: TableData = {
        className: "td-black",
        text: stat
    };
    return cell;
}