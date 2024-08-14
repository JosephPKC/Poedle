import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";

interface HintsAreaProps {
    hints: string,
    nbrGuessForHint: number | null
}

export function HintsArea({ hints, nbrGuessForHint }: HintsAreaProps) {
    const hintsArea = (hints == null && nbrGuessForHint == null) ? (<DefaultLoadingText />) : (
        <>
            <div className="cl-div-hint">
                <p><span className="cl-span-heading">Hint</span>: {hints}</p>
                <p>You need {nbrGuessForHint} more guesses to reveal the next hint!</p>
            </div>
        </>
    );

    return (
        <div>{hintsArea}</div>
    );
}