// Components
import { DefaultLoadingText } from "../shared/components/default-loading-text.tsx";
// Utils
import { NullableStats } from "./stats.ts";

interface StatsAreaProps {
    stats: NullableStats
}

export function StatsArea({ stats }: StatsAreaProps) {
    const statsArea = (stats == null) ? (<DefaultLoadingText />) :
        (<>
            <p><span className="cl-span-heading">Total Games</span>: {stats?.totalGames}</p>
            <p><span className="cl-span-heading">Best Score</span>: {stats?.bestScore} - <span className="cl-span-stat-important">{stats?.bestAnswers}</span></p>
            <p><span className="cl-span-heading">Worst Score</span>: {stats?.worstScore} - <span className="cl-span-stat-important">{stats?.worstAnswers}</span></p>
            <p><span className="cl-span-heading">Top 5 Answers</span>: <span className="cl-span-stat-important">{stats?.topAnswers}</span></p>
            <p><span className="cl-span-heading">Bottom 5 Answers</span>: <span className="cl-span-stat-important">{stats?.bottomAnswers}</span></p>
            <p><span className="cl-span-heading">Total Average</span>: {stats?.totalAverage}</p>
            <div className="cl-div-avg-all">
                {stats?.averagesPerAnswer.map((x: string) => (<p key={x}>&emsp; * <span className="cl-span-stat-important">{x}</span></p>))}
            </div>
        </>);


    return (
        <div>{statsArea}</div>
    );
}