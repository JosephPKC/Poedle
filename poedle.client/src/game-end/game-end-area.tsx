// Ext Modules
import { MouseEventHandler } from "react";
// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";
// Utils
import { Guess } from "../shared/types/type-defs.ts";

interface GameEndAreaProps {
    correctAnswer: Guess,
    score: number,
    onClickPlayAgain: MouseEventHandler
}

export function GameEndArea({ correctAnswer, score, onClickPlayAgain }: GameEndAreaProps) {
    const gameEndArea = (correctAnswer == null) ? (<DefaultLoadingText />) :
        (<>
            <p>You win!</p>
            <p>
                The correct answer was <b>{correctAnswer.label}</b>
            </p>
            <p>
                It took you <b>{score}</b> tries!
            </p>
            <button onClick={onClickPlayAgain}>Play Again?</button>
        </>);

    return (
        <div>{gameEndArea}</div>
    );
}
