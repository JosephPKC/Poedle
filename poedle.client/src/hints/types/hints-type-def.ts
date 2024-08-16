export interface AllHints {
    nbrGuessToReveal: number,
    nbrRevealsLeft: number,
    nextHintType: string,
    nameHint: NullableSingleHint,
    baseItemHint: NullableSingleHint,
    statModHint: NullableStatHint,
    flavourHint: NullableListHint
}

export interface SingleHint {
    readonly hint: string
}

export interface ListHint {
    readonly hint: string[]
}

export interface StatHint {
    readonly hint: string[],
    readonly nbrImplicits: number
}

export type NullableAllHints = AllHints | null;
export type NullableSingleHint = SingleHint | null;
export type NullableListHint = ListHint | null;
export type NullableStatHint = StatHint | null;