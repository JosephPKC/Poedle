export interface DropDownItem {
    readonly label: string
    readonly value: number
}

export type DropDownItemList = DropDownItem[];
export type NullableDropDownItem = DropDownItem | null;
export type NullableDropDownItemList = DropDownItem[] | null;
