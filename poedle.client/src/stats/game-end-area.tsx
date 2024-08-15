import { MouseEventHandler } from "react";

import { StatsArea } from "./comps/stats-area.tsx";
import { DefaultLoadingText } from "../shared/comps/default-loading-text.tsx";

import { NullableStats } from "./types/stats-type-def.ts";

interface GameEndAreaProps {
    stats: NullableStats,
    onClickPlayAgain: MouseEventHandler,
    onClearStats: MouseEventHandler
}

export function GameEndArea({ stats, onClickPlayAgain, onClearStats }: GameEndAreaProps) {
    const gameEndArea = (stats == null) ? (<DefaultLoadingText />) :
        (<>
            <p>You got <b>{stats?.answerName}</b> with <b>{stats?.score}</b> guesses and <b>{stats?.nbrHintsRemaining}</b> hints remaining!</p>
            <StatsArea stats={stats} />
            <button onClick={onClickPlayAgain}>Play Again?</button>
            <button onClick={onClearStats}>Clear Stats</button>
        </>);

    return (
        <div>{gameEndArea}</div>
    );
}
