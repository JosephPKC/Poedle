// Ext Modules
import { useEffect, useState } from "react";
import { ActionMeta, SingleValue } from "react-select";
// Components
import { AttrResultsArea } from "../results/results-area.tsx";
import { GameEndArea } from "../game-end/game-end-area.tsx";
import { GuessArea } from "../guesses/guess-area.tsx";
import { HintsArea } from "../hints/hints-area.tsx";
import { TitleHeader } from "../shared/components/title-header.tsx";
// Utils
import { AttrResult } from "../results/results-types.ts";
import { DropDownItem } from "../shared/types/drop-down-item.ts";
import { NullableStats } from "../game-end/stats.ts";
import { Guess, GuessList, AttrResultList, ChosenAnswer } from "../shared/types/type-defs.ts";

// Primary:
//   Get all answers
//  Get Chosen answer
//  Get hint format + reveals
//  Send reset (new game)
//  Send reset with score reset (change game)

function GameArea() {
    const headers = [
        "Name", "Item Class", "Base Item", "Leagues Introduced", "Item Aspects", "Drop Sources", "Drop Types", "Req Lvl", "Req Dex", "Req Int", "Req Str"
    ];

    // States
    // Store values from API
    const [availGuesses, setAvailGuesses] = useState<GuessList>(null);
    const [correctAnswer, setCorrectAnswer] = useState<ChosenAnswer>(null);
    const [results, setResults] = useState<AttrResultList>([] as AttrResult[]);
    const [stats, setStats] = useState<NullableStats>(null);
    // For the Hint
    const [hints, setHints] = useState<string>("");
    const [hintReveal, setHintReveal] = useState<number[]>([] as number[]);

    // Store for processing
    const [score, setScore] = useState<number>(0);
    const [selectedGuess, setSelectedGuess] = useState<Guess>(null);
    const [isReset, setIsReset] = useState<boolean>(false);
    const [isWin, setIsWin] = useState<boolean>(false);

    // Handlers
    // Handle when a guess is selected in the dropdown list
    const onSelectGuess = (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => {
        console.log(actionMeta);
        setSelectedGuess(newValue);
    }
    // Handle when the guess button is clicked
    const onClickGuess = () => {
        if (correctAnswer == null) {
            console.log("Correct Answer is null. Wait to load from API.");
            return;
        }

        if (selectedGuess == null) {
            alert("Please select an item in the dropdown first.");
            return;
        }

        console.log("Correct: " + correctAnswer.value + " Guess: " + selectedGuess.value);

        processResultViaAPI(selectedGuess.value);
        removeSelectedGuess(availGuesses, selectedGuess);
        setSelectedGuess(null);
        setScore(score + 1);
        processIfWin(selectedGuess.value, correctAnswer.value);

        if (score % 3 == 0) {
            revealHint(correctAnswer.hintName, hintReveal, hints);
        }
    }
    // Handle when the play again button is clicked
    const onClickPlayAgain = () => {
        // Reset the state
        setResults([] as AttrResult[]);
        setSelectedGuess(null);
        setScore(0);
        setStats(null);
        setHints("");
        setHintReveal([] as number[]);
        setIsWin(false);
        setIsReset(true);
    }

    // Do side effects on initial start and when game is reset
    useEffect(() => {
        if (isReset) {
            setGameViaAPI();
            setIsReset(false);
        }

        //getAllResultsFromAPI();
        getAvailGuessesFromAPI();
        getCorrectAnswerFromAPI();
        //getHintsFromAPI();

    }, [isReset]);

    const gameArea = isWin ?
        <>
            <GameEndArea correctAnswer={correctAnswer} score={score} onClickPlayAgain={onClickPlayAgain} stats={stats} />
            <HintsArea hints={hints} />
            <AttrResultsArea headers={headers} results={results} />
        </> :
        <>
            <GuessArea availGuesses={availGuesses} selectedGuess={selectedGuess} onSelectGuess={onSelectGuess} onClickGuess={onClickGuess} />
            <HintsArea hints={hints} />
            <AttrResultsArea headers={headers} results={results} />
        </>

    return (
        <div>
            {gameArea}
        </div>
    );

    // Retrieve data from API
    async function setGameViaAPI() {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST"
        }

        fetch("/Poedle/UniqueByAttr/SetGame", requestOptions);

  
        return () => abortController.abort();
    }

    async function getAvailGuessesFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/Poedle/UniqueByAttr/AllAvailableAnswers");
        const data = await response.json();
        setAvailGuesses(data);

        return () => abortController.abort();
    }

    async function getCorrectAnswerFromAPI() {
        const response = await fetch("/Poedle/UniqueByAttr/CorrectAnswer");
        const data = await response.json();
        await setCorrectAnswer(data);
        console.log("Correct Answer: " + correctAnswer?.value + " / " + correctAnswer?.label);

        if (correctAnswer) {
            createInitialHint(correctAnswer.hintName);
            setIndicesToReveal(correctAnswer.hintName);
        }
    }

    //async function getHintsFromAPI() {
    //    const abortController = new AbortController();

    //    const response = await fetch("/Poedle/UniqueByAttr/Hints");
    //    const data = await response.text();
    //    setHints(data);

    //    return () => abortController.abort();
    //}

    async function processResultViaAPI(guessId: number) {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        }
        // Just handle adding to the results here for now.
        fetch("/Poedle/UniqueByAttr/Guess/" + guessId, requestOptions)
            .then(response => response.json())
            .then(data => setResults([data, ...results!]));

        return () => abortController.abort();
    }

    async function updateScoreToAPI(score: number) {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST"
        }

        fetch("/Poedle/UniqueByAttr/Score/Update/" + score, requestOptions);

        return () => abortController.abort();
    }

    async function getStatsFromAPI() {
        const abortController = new AbortController();

        fetch("/Poedle/UniqueByAttr/Score/Stats")
            .then(response => response.json())
            .then(data => setStats(data));

        return () => abortController.abort();
    }

    function createInitialHint(hintName: string) {
        console.log("Create initial hint")
        if (hintName == null) return;

        let hint: string = hintName.replace("\w", "_ ");
        hint = hint.replace(" ", "/");
        hint = hint.toUpperCase();
        setHints(hint);
        console.log("initial hint: " + hints);
    }

    function setIndicesToReveal(hintName: string) {
        console.log("set to reveal")
        if (hintName == null) return;

        let revealIndices = new Array(hintName.length).fill("").map((_, i) => i);

        for (let i: number = revealIndices.length - 1; i > 0; i--) {
            let j: number = Math.floor(Math.random() * (i + 1));
            let temp: number = revealIndices[i];
            revealIndices[i] = revealIndices[j];
            revealIndices[j] = temp;
        }

        setHintReveal(revealIndices);
        console.log("reveals: " + hintReveal);
    }

    function revealHint(hintName: string, toReveal: number[], currentHint: string) {
        console.log("reveal hint")
        if (toReveal.length == 0) return;

        let index: number = toReveal[0];
        setHintReveal(toReveal.splice(0, 1));

        let revealHint: string = hintName[index];
        let newHint: string = currentHint.substring(0, index) + revealHint + currentHint.substring(index + 1);
        setHints(newHint);
    }

    // Helpers
    function removeSelectedGuess(availGuesses: GuessList, selectedGuess: Guess) {
        const newAvailGuesses = availGuesses!.filter(
            (x: Guess) => x!.value != selectedGuess!.value
        );
        setAvailGuesses(newAvailGuesses);
    }

    function processIfWin(guessId: number, correctAnswerId: number) {
        if (guessId == correctAnswerId) {
            setIsWin(true);
            updateScoreToAPI(score);
            getStatsFromAPI();
        }
    }
}

export function ByAttr() {
    return (
        <div>
            <TitleHeader gameGuessType="Attribute" />
            <GameArea />
        </div>
    );
}
