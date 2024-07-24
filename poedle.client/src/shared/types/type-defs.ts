import { ChosenDropDownitem, DropDownItem } from "./drop-down-item.ts";
import { AttrResult, NameResult } from "../../results/results-types.ts";

export type ChosenAnswer = ChosenDropDownitem | null;
export type Guess = DropDownItem | null;
export type GuessList = DropDownItem[] | null;

export type AttrResultList = AttrResult[] | null;
export type NameResultList = NameResult[] | null;