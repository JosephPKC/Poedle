export interface AllHints {
    nbrGuessToReveal: number,
    nbrRevealsLeft: number,
    nextHintType: string,
    nameHint: NullableSingleHint,
    baseItemHint: NullableSingleHint,
    statModHint: NullableStatHint,
    flavourHint: NullableListHint,
    descriptionHint: NullableSingleHint
}

export type NullableAllHints = AllHints | null;

export interface SingleHint {
    readonly hint: string
}

export type NullableSingleHint = SingleHint | null;

export interface ListHint {
    readonly hint: string[]
}

export type NullableListHint = ListHint | null;

export interface StatHint {
    readonly hint: string[],
    readonly nbrImplicits: number
}

export type NullableStatHint = StatHint | null;