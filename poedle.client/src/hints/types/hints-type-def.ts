export interface AllHints {
    nbrGuessToReveal: number,
    nbrRevealsLeft: number,
    nextHintType: string,
    nameHint: NullableHint,
    baseItemHint: NullableHint,
    statModHint: NullableHintList,
    flavourHint: NullableHint
}

export interface Hint {
    readonly hint: string
}

export interface HintList {
    readonly hint: string[]
}

export type NullableAllHints = AllHints | null;
export type NullableHint = Hint | null;
export type NullableHintList = HintList | null;