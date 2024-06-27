import { DropDownItem } from "./drop-down-item.ts";
import { AttrGuessResult, NameGuessResult } from "../../results/results-types.ts";

export type Guess = DropDownItem | null;
export type GuessList = DropDownItem[] | null;
export type HintList = string[] | null;

export type AttrResultList = AttrGuessResult[] | null;
export type NameResultList = NameGuessResult[] | null;