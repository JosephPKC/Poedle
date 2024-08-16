import { MouseEventHandler } from "react";
import Select, { ActionMeta, createFilter, SingleValue } from "react-select";

import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";

import { DropDownItem } from "../shared/types/drop-down-item.ts";
import { NullableDropDownItem, NullableDropDownItemList } from "../shared/types/drop-down-item.ts";

interface GuessAreaProps {
    availGuesses: NullableDropDownItemList,
    selectedGuess: NullableDropDownItem,
    onSelectGuess: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void,
    onClickGuess: MouseEventHandler
}

export function GuessArea({ availGuesses, selectedGuess, onSelectGuess, onClickGuess }: GuessAreaProps) {
    const ddlName = "ddl-avail-guesses";

    const filterConfig = {
        ignoreCase: true,
        ignoreAccents: true,
        trim: true,
        matchFrom: "any" as const
    };

    const guessArea = (availGuesses == null) ? (<DefaultLoadingText />) : (
        <>
            <label htmlFor={ddlName}>Guess: </label>
            <Select name={ddlName} onChange={onSelectGuess} value={selectedGuess} options={availGuesses} isSearchable={true} filterOption={createFilter(filterConfig)} />
            <button type="button" onClick={onClickGuess}>Guess</button>
        </>
    );

    return (
        <div>{guessArea}</div>
    );
}