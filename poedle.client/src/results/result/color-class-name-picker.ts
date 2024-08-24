import { ResultTypes } from "./result-types.ts";

const correctClassName: string = "td-green";
const partialClassName: string = "td-orange";
const wrongClassName: string = "td-red";
const highClassName: string = "td-high";
const lowClassName: string = "td-low";

export function getColorClassFromResult(resultType: ResultTypes): string {
    switch (resultType) {
        case ResultTypes.Correct:
            return correctClassName;
        case ResultTypes.Partial:
            return partialClassName;
        case ResultTypes.BitHigh:
            return partialClassName + " " + highClassName;
        case ResultTypes.BitLow:
            return partialClassName + " " + lowClassName;
        case ResultTypes.TooHigh:
            return wrongClassName + " " + highClassName;
        case ResultTypes.TooLow:
            return wrongClassName + " " + lowClassName;
        case ResultTypes.Wrong:
        default:
            return wrongClassName;
    }
}