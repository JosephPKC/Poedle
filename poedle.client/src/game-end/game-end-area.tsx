// Ext Modules
import { MouseEventHandler } from "react";
// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";
import { StatsArea } from "./stats-area.tsx";
// Utils
import { NullableStats } from "./stats.ts";
import { Guess } from "../shared/types/type-defs.ts";

interface GameEndAreaProps {
    correctAnswer: Guess,
    score: number,
    onClickPlayAgain: MouseEventHandler,
    stats: NullableStats
}

export function GameEndArea({ correctAnswer, score, onClickPlayAgain, stats }: GameEndAreaProps) {
    const gameEndArea = (correctAnswer == null) ? (<DefaultLoadingText />) :
        (<>
            <p>You win! The correct answer was <b>{correctAnswer.label}</b>. It took you <b>{score}</b> tries!</p>
            <StatsArea stats={stats} />
            <button onClick={onClickPlayAgain}>Play Again?</button>
        </>);

    return (
        <div>{gameEndArea}</div>
    );
}
