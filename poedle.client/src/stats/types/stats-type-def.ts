export interface Stats {
    readonly answerName: string,
    readonly score: number,
    readonly totalGames: number,
    readonly bestSingleScore: number,
    readonly worstSingleScore: number,
    readonly bestSingleAnswer: string,
    readonly worstSingleAnswer: string,
    readonly bestXAnswers: string,
    readonly worstXAnswers: string,
    readonly bestWorstXThreshold: number,
    readonly totalAverage: string,
    readonly averagesPerAnswer: string[]
}

export type NullableStats = Stats | null