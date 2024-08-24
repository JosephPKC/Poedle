import { MouseEventHandler } from "react";
import { NullableStats } from "./stats/stats.ts";
import { DataTable } from "../common/data-table/data-table.tsx";
import { BuildStatsTableData } from "./stats/stats-table-data-builder.ts";

import "./stats-area.css";

interface StatsAreaProps {
    readonly stats: NullableStats
    readonly onClickPlayAgain: MouseEventHandler
    readonly onClickClearStats: MouseEventHandler
    readonly answerClassName: string
}

export function StatsArea({ stats, onClickPlayAgain, onClickClearStats, answerClassName }: StatsAreaProps) {
    if (stats == null) {
        return (
            <></>
        );
    }

    const headers = ["Answer", "Best", "Worst", "Average", "Total"];
    const data = BuildStatsTableData(stats.statsPerAnswer);
    const styleClasses = "";

    return (
        <div id="div-stats-area">
            <div id="div-stats">
                <p className="txt-center txt-outer txt-stat txt-wrap">You got <span id="txt-stats-answer" className={answerClassName}>{stats?.answerName}</span> with <span className="txt-strong">{stats?.score}</span> guesses and <span className="txt-strong">{stats?.nbrHintsRemaining}</span> hints remaining!</p>
                <div className="fade-border" />
                <StatLine header={"Total Games"} text={stats.totalGames} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <StatLine header={"Average Score"} text={stats.totalAverage} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <StatLine header={"Best Score"} text={stats.bestSingleScore} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <StatLine header={"Worst Score"} text={stats.worstSingleScore} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <StatLine header={"Best " + stats.bestWorstXThreshold + " Answers"} text={stats.bestXAnswers} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <StatLine header={"Worst " + stats.bestWorstXThreshold + " Answers"} text={stats.worstXAnswers} className={"txt-center txt-outer txt-stat txt-wrap"} />
                <div className="fade-border" />
                <DataTable headers={headers} tableData={data} extraDataRowStyleClasses={styleClasses} />
            </div>
            <div id="div-stats-buttons">
                <button className="button" onClick={onClickPlayAgain}><span className="">Play Again</span></button>
                <button className="button" onClick={onClickClearStats}><span className="">Clear Stats</span></button>
            </div>
        </div>
    );
}

interface StatLineProps {
    readonly header: string
    readonly text: string
    readonly className: string
}

function StatLine({ header, text, className}: StatLineProps) {
    return (
        <p className={className}><span className="txt-strong">{header}</span>: {text}</p>
    );
}