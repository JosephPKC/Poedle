export interface TableStats {
    readonly answer: string
    readonly bestScore: string
    readonly worstScore: string
    readonly averageScore: string
    readonly totalGames: string
}

export type NullableTableStats = TableStats | null
export type NullableTableStatsList = TableStats[] | null

export interface Stats {
    readonly answerName: string
    readonly score: string
    readonly totalGames: string
    readonly bestSingleScore: string
    readonly worstSingleScore: string
    readonly bestXAnswers: string
    readonly worstXAnswers: string
    readonly bestWorstXThreshold: string
    readonly totalAverage: string
    readonly nbrHintsRemaining: string
    readonly statsPerAnswer: TableStats[]
}

export type NullableStats = Stats | null