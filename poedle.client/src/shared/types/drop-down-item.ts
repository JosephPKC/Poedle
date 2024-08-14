export interface DropDownItem {
    label: string,
    value: number
}

export type NullableDropDownItem = DropDownItem | null;
export type NullableDropDownItemList = DropDownItem[] | null;