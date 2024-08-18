import { MouseEventHandler } from "react";

import { DefaultLoadingText } from "./shared/comps/default-loading-text.tsx";

import { TableRowCol } from "./shared/comps/table-row-col.tsx";
import { TableHeader } from "./shared/comps/table-header.tsx";

export interface TableStats {
    readonly answer: string,
    readonly bestScore: number,
    readonly worstScore: number,
    readonly averageScore: number,
    readonly totalGames: number
}

export type NullableTableStats = TableStats | null

export interface Stats {
    readonly answerName: string,
    readonly score: number,
    readonly totalGames: number,
    readonly bestSingleScore: number,
    readonly worstSingleScore: number,
    readonly bestXAnswers: string,
    readonly worstXAnswers: string,
    readonly bestWorstXThreshold: number,
    readonly totalAverage: string,
    readonly nbrHintsRemaining: number
    readonly statsPerAnswer: TableStats[]
}

export type NullableStats = Stats | null

interface StatsTableDataProps {
    tableStats: TableStats[]
}

export function StatsTableData({ tableStats }: StatsTableDataProps) {
    let i: number = 0;
    return (
        <tbody>
            {tableStats.map((s: TableStats) =>
                <tr key={i++}>
                    <TableRowCol customClassName={"td-black"} text={s.answer} />
                    <TableRowCol customClassName={"td-black"} text={s.bestScore} />
                    <TableRowCol customClassName={"td-black"} text={s.worstScore} />
                    <TableRowCol customClassName={"td-black"} text={s.averageScore} />
                    <TableRowCol customClassName={"td-black"} text={s.totalGames} />
                </tr>)}
        </tbody>
    );
}

interface StatsTableProps {
    headers: string[],
    tableStats: TableStats[]
}

export function StatsTable({ headers, tableStats }: StatsTableProps) {
    const statsArea = (tableStats == null) ? (<DefaultLoadingText />) :
        (<div className="div-table">
            <table className="table">
                <TableHeader headers={headers} />
                <StatsTableData tableStats={tableStats} />
            </table>
        </div>);


    return (
        <>{statsArea}</>
    );
}

interface GameEndAreaProps {
    stats: NullableStats,
    onClickPlayAgain: MouseEventHandler,
    onClearStats: MouseEventHandler
}

export function GameEndArea({ stats, onClickPlayAgain, onClearStats }: GameEndAreaProps) {
    const headers = ["Answer", "Best", "Worst", "Average", "Total"];
    const gameEndArea = (stats == null) ? (<DefaultLoadingText />) :
        (<>
            <div className="stat-area">
                <p className="end-result-text">You got <span className="txt-unique-item txt-important">{stats?.answerName}</span> with <span className="txt-important">{stats?.score}</span> guesses and <span className="txt-important">{stats?.nbrHintsRemaining}</span> hints remaining!</p>
                <div className="fade-border" />
                <p className="end-stat-text"><span className="txt-important">Total Games</span>: {stats?.totalGames}</p>
                <p className="end-stat-text"><span className="txt-important">Average Score</span>: {stats?.totalAverage}</p>
                <p className="end-stat-text"><span className="txt-important">Best Score</span>: {stats?.bestSingleScore} / <span className="text-important">Worst Score</span>: {stats?.worstSingleScore} </p>
                <p className="end-stat-text"><span className="txt-important">Best {stats?.bestWorstXThreshold} Answers</span>: {stats?.bestXAnswers}</p>
                <p className="end-stat-text"><span className="txt-important">Worst {stats?.bestWorstXThreshold} Answers</span>: {stats?.worstXAnswers}</p>
                <div className="fade-border" />
                <StatsTable headers={headers} tableStats={stats.statsPerAnswer} />
            </div>
            <div className="stat-btn-area">
                <button className="btn btn-animated" onClick={onClickPlayAgain}><span className="btn-text">Play Again</span></button>
                <button className="btn btn-animated" onClick={onClearStats}><span className="btn-text">Clear Stats</span></button>
            </div>

        </>);

    return (
        <div className="game-end-area">{gameEndArea}</div>
    );
}
