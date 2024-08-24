import { NullableTableDataMatrix, TableData, TableDataMatrix, TableDataRow } from "../../common/data-table/table-data.ts";
import { getColorClassFromResult } from "./color-class-name-picker.ts";
import { NullableSkillGemResultList, NullableUniqueItemResultList, Result, SkillGemResult, UniqueItemResult } from "./result.ts";

export function BuildSkillGemResultTableData(results: NullableSkillGemResultList): NullableTableDataMatrix {
    if (results == null) {
        return null;
    }

    let tableDataMatrix: TableDataRow[] = [];
    for (let i: number = 0; i < results.length; i++) {
        const result: SkillGemResult = results[i];
        const resultData: Result[] = [result.name, result.primaryAttribute, result.dexterityPercent, result.intelligencePercent, result.strengthPercent, result.tagAmount, result.gemTags];
        const tableDataRow: TableDataRow = GetTableDataRow(resultData);
        tableDataMatrix.push(tableDataRow);
    }

    return tableDataMatrix;
}

export function BuildUniqueItemResultTableData(results: NullableUniqueItemResultList): NullableTableDataMatrix {
    if (results == null) {
        return null;
    }

    let tableDataMatrix: TableDataMatrix = [];
    for (let i: number = 0; i < results.length; i++) {  
        const result: UniqueItemResult = results[i];
        const resultData: Result[] = [result.name, result.itemClass, result.reqLvl, result.reqDex, result.reqInt, result.reqStr, result.itemAspects, result.leaguesIntroduced, result.dropSources, result.dropTypes];
        const tableDataRow: TableDataRow = GetTableDataRow(resultData);
        tableDataMatrix.push(tableDataRow);
    }

    return tableDataMatrix;
}

function GetTableDataRow(results: Result[]): TableDataRow {
    const row: TableDataRow = results.map((r: Result) => GetTableData(r));
    return row;
}

function GetTableData(result: Result): TableData {
    const cell: TableData = {
        className: getColorClassFromResult(result.result),
        text: result.value
    };
    console.log(result);
    console.log(cell);
    return cell;
}