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
import { AttrGuessResult } from "../results/results-types.ts";
import { DropDownItem } from "../shared/types/drop-down-item.ts";
import { Guess, GuessList, HintList, AttrResultList } from "../shared/types/type-defs.ts";

function GameArea() {
    const headers = [
        "Name", "Item Class", "Base Item", "Leagues Introduced", "Item Aspects", "Drop Sources", "Specific Drop Sources", "Req Lvl", "Req Dex", "Req Int", "Req Str"
    ];

    // States
    // Store values from API
    const [availGuesses, setAvailGuesses] = useState<GuessList>(null);
    const [correctAnswer, setCorrectAnswer] = useState<Guess>(null);
    const [hints, setHints] = useState<HintList>(null);
    const [results, setResults] = useState<AttrResultList>([] as AttrGuessResult[]);
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

        getGuessResultFromAPI(selectedGuess.value, correctAnswer.value);
        removeSelectedGuess(availGuesses, selectedGuess);
        setSelectedGuess(null);
    }
    // Handle when the play again button is clicked
    const onClickPlayAgain = () => {
        // Reset the state
        setResults([] as AttrGuessResult[]);
        setSelectedGuess(null);
        setScore(0);
        setIsWin(false);
        setIsReset(true);
    }

    // Do side effects on initial start and when game is reset
    useEffect(() => {
        // Retrieve from API
        getAvailGuessesFromAPI();
        getCorrectAnswerFromAPI();
        getHintsFromAPI();

        setIsReset(false);
    }, [isReset]);

    const gameArea = isWin ?
        <>
            <GameEndArea correctAnswer={correctAnswer} score={score} onClickPlayAgain={onClickPlayAgain} />
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
    async function getAvailGuessesFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/poedle/byattr/AllItemsCondensed");
        const data = await response.json();
        setAvailGuesses(data);

        return () => abortController.abort();
    }

    async function getCorrectAnswerFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/poedle/byattr/CorrectAnswer");
        const data = await response.json();
        console.log(data)
        setCorrectAnswer(data);

        return () => abortController.abort();
    }

    async function getHintsFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/poedle/byattr/Hints");
        const data = await response.json();
        setHints(data);

        return () => abortController.abort();
    }

    async function getGuessResultFromAPI(guessId: number, correctAnswerValue: number) {
        const abortController = new AbortController();

        const response = await fetch("/poedle/byattr/Guess/" + guessId + "/" + correctAnswerValue);
        const data = await response.json();
        console.log(data);
        processResults(data, correctAnswerValue, guessId);

        return () => abortController.abort();
    }

    // Helpers
    function removeSelectedGuess(availGuesses: GuessList, selectedGuess: Guess) {
        const newAvailGuesses = availGuesses!.filter(
            (x: Guess) => x!.value != selectedGuess!.value
        );
        setAvailGuesses(newAvailGuesses);
    }

    function processResults(result: AttrGuessResult, correctAnswerValue: number, selectedGuessValue: number) {
        console.log("Correct: " + correctAnswerValue + " Guess: " + selectedGuessValue);
        setScore(score + 1);
        setResults([result, ...results!]);
        if (correctAnswerValue == selectedGuessValue) {
            setIsWin(true);
            // Maybe post to score here
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
