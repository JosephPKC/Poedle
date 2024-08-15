
export function NbrGuessesToRevealText(nbrGuessesToReveal: number, isComplete: boolean) {
    return isComplete ? "No more hints left!" : nbrGuessesToReveal + " more guesses to reveal!"
}