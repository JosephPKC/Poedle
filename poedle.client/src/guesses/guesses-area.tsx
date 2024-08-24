import { MouseEventHandler } from "react";
import { ActionMeta, SingleValue } from "react-select";
import { DropDownItem, NullableDropDownItem, NullableDropDownItemList } from "../common/drop-down-item/drop-down-item.ts";
import { GuessSelect, GuessSelectOptions } from "./guess-select/guess-select.tsx";

import "./guesses-area.css";

interface GuessesAreaProps {
    readonly availGuesses: NullableDropDownItemList
    readonly selectedGuess: NullableDropDownItem
    readonly onChange: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void
    readonly onClick: MouseEventHandler
    readonly options: GuessSelectOptions
}

export function GuessesArea({ availGuesses, selectedGuess, onChange, onClick, options }: GuessesAreaProps) {
    if (availGuesses == null) {
        return (
            <></>
        );
    } 

    return (
        <GuessSelect availGuesses={availGuesses} selectedGuess={selectedGuess} onChange={onChange} onClick={onClick} options={options} />
    );
}