import { ResultType } from "../results-types.ts";

const correctClassName: string = "td-green";
const partialClassName: string = "td-orange";
const wrongClassName: string = "td-red";

export function getColorClassFromResult(pResult: ResultType) {
    switch (pResult) {
        case ResultType.Correct:
            return correctClassName;
        case ResultType.Partial:
            return partialClassName;
        case ResultType.Wrong:
        default:
            return wrongClassName;
    }
}