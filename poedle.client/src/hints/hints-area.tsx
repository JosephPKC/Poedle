import { HintGuessText } from "./hint-guess-text/hint-guess-text.tsx";
import { GeneralTextHint } from "./hint/general-text-hint.tsx";
import { AllHints, NullableAllHints } from "./hint/hint.ts";
import { NameHint } from "./hint/name-hint.tsx";
import { StatModHint } from "./hint/stat-mod-hint.tsx";

import "./hints-area.css";

interface HintsAreaProps {
    readonly hints: NullableAllHints
    readonly isWin: boolean
}

export function SkillGemsHintsArea({ hints, isWin }: HintsAreaProps) {
    if (hints == null) {
        return (
            <></>
        );
    }

    const nameHint = (hints.nameHint == null) ? <></> :
        <>
            <NameHint hint={hints.nameHint!} secondaryHint={hints.baseItemHint} hintStyleId={"txt-name-hint"} secondaryHintStyleId={"txt-secondary-hint"} styleClassName={"txt-center txt-skill-gem"} fadeStyleClassName={"fade-border-skill-gem"} />
        </>;

    const descriptionHint = (hints.descriptionHint == null) ? <></> :
        <>
            <GeneralTextHint hint={{ hint: [hints.descriptionHint.hint] }} hintStyleId={"txt-desc"} styleClassName={"txt-center txt-skill-desc txt-wrap"} />
        </>;

    const divs =
        <div id="div-skill-gem-hints" className="div-hints">
            {nameHint}
            {descriptionHint}
        </div>

    return (
        <HintArea hints={hints} isWin={isWin} hintDivs={divs} />
    );
}

export function UniqueItemHintsArea({ hints, isWin }: HintsAreaProps) {
    if (hints == null) {
        return (
            <></>
        );
    }

    const nameHint = (hints.nameHint == null) ? <></> :
        <>
            <NameHint hint={hints.nameHint!} secondaryHint={hints.baseItemHint} hintStyleId={"txt-name-hint"} secondaryHintStyleId={"txt-secondary-hint"} styleClassName={"txt-center txt-unique-item"} fadeStyleClassName={"fade-border-unique-item"} />
        </>;

    const statModHint = (hints.statModHint == null) ? <></> :
        <>
            <StatModHint hint={hints.statModHint!} imgStyleClassName={"img-stat-hider"} hintStyleClassName={"txt-center txt-stat-mod txt-wrap"} divStyleClassName={"div-stat-hints"}  />
        </>;

    const flavourHint = (hints.flavourHint == null) ? <></> :
        <>
            <GeneralTextHint hint={hints.flavourHint} hintStyleId={"txt-flavour-hint"} styleClassName={"txt-center txt-flavour txt-wrap"} />
        </>;

    const divs = 
        <div id="div-unique-item-hints" className="div-hints">
            {nameHint}
            {statModHint}
            {flavourHint}
        </div>

    return (
        <HintArea hints={hints} isWin={isWin} hintDivs={divs} />
    );
}

interface HintAreaProps {
    readonly hints: AllHints
    readonly isWin: boolean
    readonly hintDivs: JSX.Element
}

function HintArea({ hints, isWin, hintDivs }: HintAreaProps) {
    return (
        <div id="div-hints-area">
            {hintDivs}
            <HintGuessText isWin={isWin} nbrGuessToReveal={hints.nbrGuessToReveal} nbrRevealsLeft={hints.nbrRevealsLeft} nextHintType={hints.nextHintType} />
        </div>
    );
}