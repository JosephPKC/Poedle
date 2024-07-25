export interface Stats {
    readonly score: number,
    readonly totalGames: number,
    readonly bestScore: number,
    readonly bestAnswers: string,
    readonly worstScore: number,
    readonly worstAnswers: string,
    readonly topAnswers: string,
    readonly bottomAnswers: string,
    readonly totalAverage: number,
    readonly averagesPerAnswer: string[]
}

export type NullableStats = Stats | null