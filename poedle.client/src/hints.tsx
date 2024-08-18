import { DefaultLoadingText } from "./shared/comps/default-loading-text.tsx";

export interface AllHints {
    nbrGuessToReveal: number,
    nbrRevealsLeft: number,
    nextHintType: string,
    nameHint: NullableSingleHint,
    baseItemHint: NullableSingleHint,
    statModHint: NullableStatHint,
    flavourHint: NullableListHint,
    descriptionHint: NullableSingleHint
}

export interface SingleHint {
    readonly hint: string
}

export interface ListHint {
    readonly hint: string[]
}

export interface StatHint {
    readonly hint: string[],
    readonly nbrImplicits: number
}

export type NullableAllHints = AllHints | null;
export type NullableSingleHint = SingleHint | null;
export type NullableListHint = ListHint | null;
export type NullableStatHint = StatHint | null;

export function NbrGuessesToRevealText(nbrGuessesToReveal: number, isComplete: boolean) {
    return isComplete ? "No more hints left!" : nbrGuessesToReveal + " more guesses to reveal!"
}

interface NameHintAreaProps {
    readonly nameHint: SingleHint
    readonly secondaryNameHint: NullableSingleHint
}

export function NameHintArea({ nameHint, secondaryNameHint }: NameHintAreaProps) {
    return (
        <div className="div-txt-box">
            <div className="fade-border-name" />
            <p className="txt-name-hint txt-unique-item">{nameHint.hint}</p>
            {secondaryNameHint == null ? <></> :
                <p className="txt-name-hint txt-unique-item">{secondaryNameHint.hint}</p>
            }
            <div className="fade-border-name" />
        </div>
    );
}

interface FlavourHintAreaProps {
    readonly flavourHint: ListHint
}

export function FlavourHintArea({ flavourHint }: FlavourHintAreaProps) {
    let i = 0;
    return (
        <div className="div-txt-box">
            {flavourHint.hint.map((x: string) =>
                <p className="txt-flavour" key={i++}>{x}</p>
            )}
        </div>
    );
}

interface StatModTextProps {
    readonly statModText: string
}

function StatModText({ statModText }: StatModTextProps) {
    const statMod = statModText == "_" ?
        <img src="images/Veiled_Mod.gif" className="stat-hider" /> :
        <p className="stat-text">{statModText}</p>;

    return (
        <>
            {statMod}
        </>
    );
}

interface StatHintAreaProps {
    readonly statModHint: StatHint
}

export function StatHintArea({ statModHint }: StatHintAreaProps) {
    let i = 0, j = 0;
    return (
        <div>
            <div id="div-stat-implicit-mod-hint-area">
                {statModHint.hint.slice(0, statModHint.nbrImplicits).map((x: string) =>
                    <StatModText statModText={x} key={i++} />
                )}
            </div>
            <div className="fade-border" />
            <div id="div-stat-explicit-mod-hint-area">
                {statModHint.hint.slice(statModHint.nbrImplicits).map((x: string) =>
                    <StatModText statModText={x} key={j++} />
                )}
            </div>
            <div className="fade-border" />
        </div>
    );
}

interface DescriptionHintAreaProps {
    readonly description: SingleHint
}

function DescriptionHintArea({ description }: DescriptionHintAreaProps) {
    return (
        <div id="div-desc-area" className="div-txt-box">
            <p className="txt-description">{description.hint}</p>
        </div>
    );
}

interface HintsAreaProps {
    hints: NullableAllHints,
    isWin: boolean
}

export function HintsArea({ hints, isWin }: HintsAreaProps) {
    if (hints == null) {
        return <DefaultLoadingText />;
    }

    const guessWordText = hints.nbrGuessToReveal > 1 ? "guesses" : "guess";

    const nbrGuessText = hints.nbrGuessToReveal > 0 && hints.nbrRevealsLeft > 0 ?
        <div>
            <p className="txt-guess txt-outer">Make <span className="txt-important">{hints.nbrGuessToReveal}</span> more {guessWordText} to reveal the next <span className="txt-important">{hints.nextHintType}</span> hint!</p>
            <p className="txt-guess txt-outer"><span className="txt-important">{hints.nbrRevealsLeft}</span> hints left to go!</p>
        </div> :
        <div>
            <p className="txt-guess txt-outer">No more guesses left!</p>
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
        <div id="div-hint-area">
            <div id="div-hint-only-area" className="div-black-box">
                {nameHintsArea}
                {statModHintsArea}
                {flavourHintsArea}
            </div>
            {guessText}
        </div>
    );
}

export function HintsAreaSkillGems({ hints, isWin }: HintsAreaProps) {
    if (hints == null) {
        return <DefaultLoadingText />;
    }

    const guessWordText = hints.nbrGuessToReveal > 1 ? "guesses" : "guess";

    const nbrGuessText = hints.nbrGuessToReveal > 0 && hints.nbrRevealsLeft > 0 ?
        <div>
            <p className="txt-guess txt-outer">Make <span className="txt-important">{hints.nbrGuessToReveal}</span> more {guessWordText} to reveal the next <span className="txt-important">{hints.nextHintType}</span> hint!</p>
            <p className="txt-guess txt-outer"><span className="txt-important">{hints.nbrRevealsLeft}</span> hints left to go!</p>
        </div> :
        <div>
            <p className="txt-guess txt-outer">No more guesses left!</p>
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

    const descriptionHintArea = (hints.descriptionHint == null) ? <></> :
        <>
            <DescriptionHintArea description={hints.descriptionHint!} />
        </>;


    return (
        <div id="div-hint-area">
            <div id="div-hint-only-area" className="div-black-box">
                {nameHintsArea}
                {descriptionHintArea}
            </div>
            {guessText}
        </div>
    );
}