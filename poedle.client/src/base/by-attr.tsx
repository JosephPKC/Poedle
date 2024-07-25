// Ext Modules
import { useEffect, useRef, useState } from "react";
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

// TODO:
// Use secondary UseEffects to ensure that logic waits until certain states are loaded (i.e. the hints). See example below
// Try to move things to the controllers so that it persists on page refresh
//  Hints should be calculated and stored on server side and sent to client for display
//      It calculates the hint on game start
//      API returns the hint as is and UI just displays it
//      On each process guess, also do a hint reveal every 3 guesses
//      Server side will reveal a new letter for the hint, update it in state. Then, UI can get the hint again
//  Results should be calculated and stored on server side and sent
//      On each process guess, calculate the result and store in state
//      API returns all results, in order from latest to oldest.
//      UI stores in state and will just display it as is via result table.
//      This should persist through refresh
//  Scores and stats should be stored server side
//      On each process guess, update score on server side
//      On game end, API returns the updated score and calculate stats.
//      UI will just display as is (no need to store it)
//  Available answers should be stored server side
//      API returns all available answers
//      On each process guess, server will update available answers by removing the guess
//      UI will request from API to store in state
//  Chosen answer should be calculated server side but it does not need to be on UI
//      On game start, server will calculate and store correct answer

//  Game start/Reset game - At the start of a new game, i.e. on game load or on play again click
//      Load via API: all available answers, hints.
//      Set states in UI to default.
//      Set state in server to default. Do post to API: start new game
//  Process Guess - When a guess is selected, process it.
//      Post to API: Post guess so that server can calculate stuff
//      Post to API: Update score
//      Get from API: results so far and store it in state. It should update table automatically
//      Post to API: Check if Win. If win, go to ON Win. If not, finish.
//      Get from API: available answers and store it in state. It should update ddl automatically
//      Get from API: Get hints. server will automatically reveal hints based on score. It should update hints automatically
//  On Win - On win, display the win page and end game
//      Set win state, so it should update page to a new set of components.
//      Get from API: score and stats. store them in state and page should update automatically
//  On Play Again - On play again, start a brand new game
//      Go back to Game Start

// Need a button to:
//      Reset stats
//          Completely reset score and stats in API
//      Force start a new game despite being in the middle of one.
//          Reset the game state (results, chosen answer, etc)
//          Redo the on start useEffect calls to populate the UI state.



// Primary Ops
//- GetAllAvailableAnswers() - Queries the state to get all available answers. State will remove from available options during ProcessGuess.
//- GetHints() - Queries the state to get the hint for the answer. State will calculate what the hint string should be during ProcessGuess.
//- GetResults() - Queries the state to get the results for the game, from latest to oldest. State will calculate the result and hold onto the result list for display during ProcessGuess.
//- ProcessGuess() - Processes a guess:
//  - Calculate the results. Add results to Results.
//  - Update Score
//  - Check if Win. If Win, set IsWin
//  - If Not Win:
//      - Remove guess from AvailableAnswers
//      - Check if to Reveal Hint: if so, update hint
//  - Returns isWin
//- IsWin() - Queries the state to see if the game has been won
//- GetStats() - Queries the state to get the stats total. State will calculate the stats based on the total score history when queried.
//- ResetGame() - Reset the state to default for a new game: AvailabeleAnswers = full list, ChosenAnswer = random, Hints = based on chosen, Results = blank, IsWin = false
//- ResetStats() - Reset the state to reset the stats. Scores = blank

// Flow for UI side effects
//- Page load - []
//  - IsWin() - Check if game has been won or not.
//- Page load New Game - [IsNewGame]
//  - GetAvailableAnswers(), GetHints(), GetResults()
//- Page Load On Win - [IsInitialLoad && IsWin]
//  - SetIsInitialLoad(false) - IsInitialLoad defaults to true. It is to hold whether this is the initial load or not.
//  - GetHints(), GetResults(), GetStats()
//- Page Load On Not Win - [IsInitialLoad && !IsWin]
//  - SetIsInitialLoad(false)
//  - GetAvailableAnswers(), GetHints(), GetResults()
//- OnGuess - Handler
//  - ProcessGuess()
//- OnGuess Finish - [IsProcessedGuess]
//  - IsWin()
//- OnGuess IsWin - [IsProcessedGuess && IsWin]
//  - SetIsProcessedGuess(false)
//  - GetHints(), GetResults(), GetStats()
//- OnGuess Not IsWin - [IsProcessedGuess && !IsWin]
//  - SetIsProcessedGuess(false)
//  - GetAvailableAnswers(), GetHints(), GetResults()
//- On Play Again - Handler
//  - SetIsNewGame(true)
//  - set all other states to their defaults
//  - ResetGame()
//- On New Game - Handler
//  - SetIsNewGame(true)
//  - set all other states to their defaults
//  - ResetGame()
//- On Clear Stats - Handler
//  - ClearStats()


// Clean up UI and backend server
// Add new games
// Add styling
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
    const [processedGuess, setProcessedGuess] = useState<AttrResult | null>(null);
    const [processedWin, setProcessedWin] = useState<string | null>(null);
    // For the Hint
    const [hints, setHints] = useState<string>("");
    //const [hintReveal, setHintReveal] = useState<number[]>([] as number[]);

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

    }
    // Handle when the play again button is clicked
    const onClickPlayAgain = () => {
        // Reset the state
        //setScore(0);
        setStats(null);
        setHints("");
        //setHintReveal([] as number[]);
        setIsWin(false);
        setIsReset(true);
    }

    // Do side effects on initial start and when game is reset
    useEffect(() => {
        if (isReset) {
            setGameViaAPI();
            setIsReset(false);
            setProcessedGuess(null);
            setResults([] as AttrResult[]);
        }

        checkIfWinViaAPI();
        console.log("In primary useEFfect IsWin: " + isWin);
        getResultsFromAPI();
        getAvailGuessesFromAPI();
        getCorrectAnswerFromAPI();
        

    }, [isReset]);

    // This useEffect waits until correct answer is loaded before doing the hints
    useEffect(() => {
        if (correctAnswer) {
            console.log("Correct Answer in separate use effect: " + correctAnswer?.value + " / " + correctAnswer?.label + " / " + correctAnswer?.hintName);
            //createInitialHint(correctAnswer.hintName);
            //setIndicesToReveal(correctAnswer.hintName);
            getHintsFromAPI();
        }
    }, [correctAnswer]);

    // This useEffect waits until the correct answer is loaded, a guess is selected, and the guess is processed before doing more processing (getting results to display, updating score, processing win, and getting available answers)
    useEffect(() => {
        console.log("Results in separate use effect?: " + results?.map((x: AttrResult) => x.name));
        if (processedGuess && selectedGuess && correctAnswer) {
            console.log("Results in separate use effect: " + results?.map((x: AttrResult) => x.name));
            getResultsFromAPI();
            updateScoreToAPI();
            getHintsFromAPI();
            setProcessedGuess(null);
            setIsWinViaAPI(selectedGuess.value);
        }
    }, [processedGuess && selectedGuess && correctAnswer]);

    useEffect(() => {
        if (isWin) {
            //updateScoreToAPI(score);
            getScoreFromAPI();
            getStatsFromAPI();
        }
    }, [isWin]);

    useEffect(() => {
        if (processedWin) {
            checkIfWinViaAPI();
        }
    }, [processedWin]);

    useEffect(() => {
        if (processedWin && !isWin) {
            //removeSelectedGuess(availGuesses, selectedGuess);
            setSelectedGuess(null);
            getAvailGuessesFromAPI();
            setProcessedWin(null);
        }
    }, [processedWin && !isWin]);

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

        fetch("/Poedle/UniqueByAttr/Game/Set", requestOptions);

  
        return () => abortController.abort();
    }

    async function setIsWinViaAPI(guessId: number) {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/text" }
        }
        const response = await fetch("/Poedle/UniqueByAttr/Game/SetIsWin/" + guessId, requestOptions);
        const data = await response.text();
        setProcessedWin(data);

        return () => abortController.abort();
    }

    async function checkIfWinViaAPI() {
        const abortController = new AbortController();

        const response = await fetch("/Poedle/UniqueByAttr/Game/IsWin");
        const data = await response.text();
        await setIsWin(data == "true");

        return () => abortController.abort();
    }

    async function getAvailGuessesFromAPI() {
        console.log("GETAVAILGUESSFROMAPI");
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
        console.log("State Correct Answer: " + correctAnswer?.value + " / " + correctAnswer?.label + " / " + correctAnswer?.hintName);
        console.log("Correct Answer: " + data.value + " / " + data.label + " / " + data.hintName);

        //await createInitialHint(data.hintName);
        //await setIndicesToReveal(data.hintName);
    }

    async function getHintsFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/Poedle/UniqueByAttr/Hints");
        const data = await response.text();
        await setHints(data);

        return () => abortController.abort();
    }

    async function processResultViaAPI(guessId: number) {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        }
        // Just handle adding to the results here for now.
        const response = await fetch("/Poedle/UniqueByAttr/Guess/" + guessId, requestOptions);
        const data = await response.json();
        await setProcessedGuess(data);
        console.log("State Guess: " + processedGuess);
        console.log("Guess: " + data);

        return () => abortController.abort();
    }

    async function getResultsFromAPI() {
        const abortController = new AbortController();

        console.log("GetResultsFromAPI");
        const response = await fetch("/Poedle/UniqueByAttr/Guess/AllResults");
        const data = await response.json();
        await setResults(data);
        console.log("Get Results: " + data.map((x: AttrResult) => x.name));
        return () => abortController.abort();
    }

    async function updateScoreToAPI() {
        const abortController = new AbortController();

        const requestOptions = {
            method: "POST"
        }

        fetch("/Poedle/UniqueByAttr/Score/Update/", requestOptions);

        return () => abortController.abort();
    }

    async function getScoreFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/Poedle/UniqueByAttr/Score/");
        const data = await response.text;
        await setScore(+data);

        return () => abortController.abort();
    }

    async function getStatsFromAPI() {
        const abortController = new AbortController();

        const response = await fetch("/Poedle/UniqueByAttr/Score/Stats");
        const data = await response.json();
        await setStats(data);
        await setScore(data.score);

        return () => abortController.abort();
    }

    //function createInitialHint(hintName: string) {
    //    console.log("Create initial hint")
    //    if (hintName == null) return;

    //    let hint: string = hintName.replace(" ", " /");
    //    hint = hint.toUpperCase();
    //    hint = hint.replace(/\w/g, " _");
    //    setHints(hint);
    //    console.log("initial hint: " + hint);

    //    //console.log("set to reveal")
    //    //let revealIndices = new Array(hintName.length).fill("").map((_, i) => i);

    //    //for (let i: number = revealIndices.length - 1; i > 0; i--) {
    //    //    let j: number = Math.floor(Math.random() * (i + 1));
    //    //    let temp: number = revealIndices[i];
    //    //    revealIndices[i] = revealIndices[j];
    //    //    revealIndices[j] = temp;
    //    //}

    //    //setHintReveal(revealIndices);
    //    //console.log("reveals: " + revealIndices);
    //}

    //function setIndicesToReveal(hintName: string) {
    //    console.log("set to reveal")
    //    if (hintName == null) return;

    //    let revealIndices = new Array(hintName.length).fill("").map((_, i) => i);

    //    for (let i: number = revealIndices.length - 1; i > 0; i--) {
    //        let j: number = Math.floor(Math.random() * (i + 1));
    //        let temp: number = revealIndices[i];
    //        revealIndices[i] = revealIndices[j];
    //        revealIndices[j] = temp;
    //    }

    //    setHintReveal(revealIndices);
    //    console.log("reveals: " + hintReveal);
    //}

    //function revealHint(hintName: string, toReveal: number[], currentHint: string) {
    //    console.log("reveal hint")
    //    if (toReveal.length == 0) return;

    //    let index: number = toReveal[0];
    //    console.log("Index: " + index);
    //    console.log("Reveals: " + toReveal);
    //    toReveal.splice(0, 1);
    //    console.log("After: " + toReveal);
    //    setHintReveal(toReveal);

    //    let revealHint: string = hintName[index];
    //    console.log("Revealed Hint: " + revealHint);
    //    let newHint: string = currentHint.substring(0, index) + revealHint.toUpperCase() + currentHint.substring(index + 1);
    //    console.log("Parts: " + currentHint.substring(0, index) + " + " + revealHint.toUpperCase() + " + " + currentHint.substring(index + 1));
    //    setHints(newHint);
    //}

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
            //updateScoreToAPI(score);
            getScoreFromAPI();
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
