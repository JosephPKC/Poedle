import { MouseEventHandler } from "react";
import Select, { ActionMeta, createFilter, SingleValue } from "react-select";
import { DropDownItem, DropDownItemList, NullableDropDownItem } from "../../common/drop-down-item/drop-down-item.ts";

interface GuessSelectProps {
    readonly availGuesses: DropDownItemList
    readonly selectedGuess: NullableDropDownItem
    readonly onChange: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void
    readonly onClick: MouseEventHandler
    readonly options: GuessSelectOptions
}

export interface GuessSelectOptions {
    readonly isSearchable: boolean
    readonly ignoreAccents: boolean
    readonly ignoreCase: boolean
    readonly trim: boolean
    readonly matchFrom: "any" | "start"
    readonly placeHolder: string
    readonly maxMenuHeight: number
}

export function GuessSelect({ availGuesses, selectedGuess, onChange, onClick, options }: GuessSelectProps) {
    const filterConfig = {
        ignoreCase: options.ignoreCase,
        ignoreAccents: options.ignoreAccents,
        trim: options.trim,
        matchFrom: options.matchFrom
    };

    return (
        <div id="div-guess-select">
            <Select className="select" onChange={onChange} value={selectedGuess} options={availGuesses} isSearchable={options.isSearchable} filterOption={createFilter(filterConfig)} placeholder={options.placeHolder} maxMenuHeight={options.maxMenuHeight} />
            <button className="button" type="button" onClick={onClick}>Guess</button>
        </div>
    );
}
