import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";
import { FlavourHintArea } from "./comps/flavour-hint-area.tsx";
import { NameHintArea } from "./comps/name-hint-area.tsx";
import { StatHintArea } from "./comps/stat-hint-area.tsx";

import { NullableAllHints } from "./types/hints-type-def.ts";

import "./styles/hints-area.css";

interface HintsAreaProps {
    hints: NullableAllHints,
    isWin: boolean
}

export function HintsArea({ hints, isWin }: HintsAreaProps) {
    if (hints == null) {
        return <DefaultLoadingText />;
    }

    const nbrGuessText = hints.nbrGuessToReveal > 0 && hints.nbrRevealsLeft > 0 ? 
        <div>
            <p>Make {hints.nbrGuessToReveal} more guess(es) to reveal the next {hints.nextHintType} hint!</p> 
            <p>{hints.nbrRevealsLeft} hints left to go!</p>
        </div> :
        <div>
            <p>No more guesses left!</p>
        </div>

    const guessText = isWin ?
        <></> :
        <>
            {nbrGuessText}
        </>;

    const nameHintsArea = (hints.nameHint == null) ? <></> :
        <>
            <NameHintArea nameHint={hints.nameHint!} secondaryNameHint={hints.baseItemHint} />
        </>;

    const statModHintsArea = (hints.statModHint == null) ? <></> :
        <>
            <StatHintArea statModHint={hints.statModHint!} />
        </>;   

    const flavourHintsArea = (hints.flavourHint == null) ? <></> :
        <>
            <FlavourHintArea flavourHint={hints.flavourHint!} />
        </>;   

    return (
        <div id="id-div-hints-area">
            <h3>Hints:</h3>
            {nameHintsArea}
            {statModHintsArea}
            {flavourHintsArea}
            {guessText}
        </div>
    );
}