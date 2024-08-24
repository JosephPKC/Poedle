interface HintGuessTextProps {
    readonly isWin: boolean
    readonly nbrGuessToReveal: number
    readonly nbrRevealsLeft: number
    readonly nextHintType: string
}

export function HintGuessText({ isWin, nbrGuessToReveal, nbrRevealsLeft, nextHintType }: HintGuessTextProps) {
    const guessWordText = nbrGuessToReveal > 1 ? "guesses" : "guess";

    const nbrGuessText = nbrGuessToReveal > 0 && nbrRevealsLeft > 0 ?
        <div id="div-hint-guess-text">
            <p className="txt-guess txt-outer txt-wrap">Make <span className="txt-strong">{nbrGuessToReveal}</span> more {guessWordText} to reveal the next <span className="txt-strong">{nextHintType}</span> hint!</p>
            <p className="txt-guess txt-outer txt-wrap"><span className="txt-strong">{nbrRevealsLeft}</span> hints left to go!</p>
        </div> :
        <div>
            <p className="txt-guess txt-outer txt-wrap">No more guesses left!</p>
        </div>

    const guessText = isWin ? <></> : <>{nbrGuessText}</>;

    return (
        <>{guessText}</>
    );
}