import { MouseEventHandler } from "react";
import Select, { ActionMeta, createFilter, SingleValue } from "react-select";

import { DefaultLoadingText } from "./shared/comps/default-loading-text.tsx";

import { DropDownItem } from "./shared/types/drop-down-item.ts";
import { NullableDropDownItem, NullableDropDownItemList } from "./shared/types/drop-down-item.ts";

interface GuessAreaProps {
    readonly availGuesses: NullableDropDownItemList,
    readonly selectedGuess: NullableDropDownItem,
    readonly onSelectGuess: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void,
    readonly onClickGuess: MouseEventHandler,
    readonly placeHolder: string
}

export function GuessArea({ availGuesses, selectedGuess, onSelectGuess, onClickGuess, placeHolder }: GuessAreaProps) {
    const filterConfig = {
        ignoreCase: true,
        ignoreAccents: true,
        trim: true,
        matchFrom: "any" as const
    };

    const guessArea = (availGuesses == null) ? (<DefaultLoadingText />) : (
        <div className="div-guess-area">
            <Select className="select" onChange={onSelectGuess} value={selectedGuess} options={availGuesses} isSearchable={true} filterOption={createFilter(filterConfig)} placeholder={placeHolder} maxMenuHeight={250} />
            <button className="btn" type="button" onClick={onClickGuess}>Guess</button>
        </div>
    );

    return (
        <div>{guessArea}</div>
    );
}