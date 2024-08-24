import { MouseEventHandler } from "react"
import { ActionMeta, SingleValue } from "react-select"
import { DropDownItem, NullableDropDownItem, NullableDropDownItemList } from "../../common/drop-down-item/drop-down-item.ts"
import { GuessSelectOptions } from "../../guesses/guess-select/guess-select.ts"
import { NullableAllHints } from "../../hints/hint/hint.ts"
import { NullableUniqueItemResultList } from "../../results/result/result.ts"
import { NullableStats } from "../../stats/stats/stats.ts"

export interface GuessesAreaProps {
    readonly availGuesses: NullableDropDownItemList
    readonly selectedGuess: NullableDropDownItem
    readonly onChange: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void
    readonly onClick: MouseEventHandler
    readonly options: GuessSelectOptions
}

export interface HintsAreaProps {
    readonly hints: NullableAllHints
}

export interface ResultsAreaProps {
    readonly results: NullableUniqueItemResultList
}
export interface StatsAreaProps {
    readonly stats: NullableStats
    readonly onClickPlayAgain: MouseEventHandler
    readonly onClickClearStats: MouseEventHandler
    readonly answerClassName: string
}
