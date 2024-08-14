import { DefaultLoadingText } from "../../shared/comps/default-loading-text.tsx";

import { NullableStats } from "../types/stats-type-def.ts";

interface StatsAreaProps {
    stats: NullableStats
}

export function StatsArea({ stats }: StatsAreaProps) {
    const statsArea = (stats == null) ? (<DefaultLoadingText />) :
        (<>
            <p><span className="cl-span-heading">Total Games</span>: {stats?.totalGames}</p>
            <p><span className="cl-span-heading">Best Answer(s)</span>: {stats?.bestSingleScore} - <span className="cl-span-stat-important">{stats?.bestSingleAnswer}</span></p>
            <p><span className="cl-span-heading">Worst Answer(s)</span>: {stats?.worstSingleScore} - <span className="cl-span-stat-important">{stats?.worstSingleAnswer}</span></p>
            <p><span className="cl-span-heading">Best {stats?.bestWorstXThreshold} Answers</span>: <span className="cl-span-stat-important">{stats?.bestXAnswers}</span></p>
            <p><span className="cl-span-heading">Worst {stats?.bestWorstXThreshold} Answers</span>: <span className="cl-span-stat-important">{stats?.worstXAnswers}</span></p>
            <p><span className="cl-span-heading">Total Average</span>: {stats?.totalAverage}</p>
            <div className="cl-div-avg-all">
                {stats?.averagesPerAnswer.map((x: string) => (<p key={x}>&emsp; * <span className="cl-span-stat-important">{x}</span></p>))}
            </div>
        </>);


    return (
        <div>{statsArea}</div>
    );
}