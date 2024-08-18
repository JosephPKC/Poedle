import { MouseEventHandler, useEffect, useState } from "react";
import { ActionMeta, SingleValue } from "react-select";

import { TitleHeader } from "./shared/comps/title-header.tsx"
import { GuessArea } from "./guess.tsx";
import { HintsAreaSkillGems, NullableAllHints } from "./hints.tsx";
import { ResultsAreaSkillGem, NullableSkillGemResult, NullableSkillGemResultList } from "./result.tsx";
import { GameEndArea, NullableStats } from "./stats.tsx";

import { DropDownItem, NullableDropDownItem, NullableDropDownItemList } from "./shared/types/drop-down-item.ts";

import "./unique-items.css";

interface HintsAreaProps {
    readonly hints: NullableAllHints
}

interface GameEndAreaProps {
    readonly stats: NullableStats,
    readonly onClickPlayAgain: MouseEventHandler,
    readonly onClearStats: MouseEventHandler
}

interface ResultsAreaProps {
    readonly headers: string[],
    readonly results: NullableSkillGemResultList
}

interface GuessAreaProps {
    readonly availGuesses: NullableDropDownItemList,
    readonly selectedGuess: NullableDropDownItem,
    readonly onSelectGuess: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void,
    readonly onClickGuess: MouseEventHandler,
    readonly placeHolder: string
}

interface GameAreaProps {
    readonly isWin: boolean,

    readonly hintsArea: HintsAreaProps,
    readonly gameEndArea: GameEndAreaProps,
    readonly resultsArea: ResultsAreaProps,
    readonly guessArea: GuessAreaProps
}

function GameArea({ isWin, hintsArea, gameEndArea, resultsArea, guessArea } : GameAreaProps) {
    const gameArea = isWin ?
        <>
            <HintsAreaSkillGems hints={hintsArea.hints} isWin={true} />
            <GameEndArea stats={gameEndArea.stats!} onClickPlayAgain={gameEndArea.onClickPlayAgain} onClearStats={gameEndArea.onClearStats} />
            <ResultsAreaSkillGem headers={resultsArea.headers} results={resultsArea.results} />
        </> :
        <>
            <HintsAreaSkillGems hints={hintsArea.hints} isWin={false} />
            <GuessArea availGuesses={guessArea.availGuesses} selectedGuess={guessArea.selectedGuess} onSelectGuess={guessArea.onSelectGuess} onClickGuess={guessArea.onClickGuess} placeHolder={guessArea.placeHolder} />
            <ResultsAreaSkillGem headers={resultsArea.headers} results={resultsArea.results} />
        </>;

    return (
        <div id="div-game-area">
            {gameArea}
        </div>
    );
}

export function SkillGemsGame() {
    // Retrieve data from API
    async function GetAvailGuesses() {
        const apiName: string = "GetAvailGuesses";
        const apiUrl: string = "/Poedle/SkillGems/Answers/AllAvailable";
        const processData = (data: any) => {
            setAvailGuesses(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function GetHints() {
        const apiName: string = "GetHints";
        const apiUrl: string = "/Poedle/SkillGems/Hints/All";
        const processData = (data: any) => {
            setHints(data);
            console.log(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function GetResults() {
        const apiName: string = "GetResults";
        const apiUrl: string = "/Poedle/SkillGems/Results";
        const processData = (data: any) => {
            setResults(data);
            console.log(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function GetIsWin() {
        const apiName: string = "GetIsWin";
        const apiUrl: string = "/Poedle/SkillGems/Game/IsWin";
        const processData = (data: any) => {
            setIsWin(data == "true");
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function GetStats() {
        const apiName: string = "GetStats";
        const apiUrl: string = "/Poedle/SkillGems/Stats";
        const processData = (data: any) => {
            setStats(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    // Post data to API for processing
    async function ProcessGuess(guessId: number) {
        const apiName: string = "ProcessGuess";
        const apiUrl: string = "/Poedle/SkillGems/Results/Process/" + guessId;
        const processData = (data: any) => {
            setProcessedGuess(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function ClearStats() {
        const apiName: string = "ClearStats";
        const apiUrl: string = "/Poedle/SkillGems/Stats/Clear";
        const processData = (data: any) => {
            setIsClearStats(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function ResetGame() {
        const apiName: string = "ResetGame";
        const apiUrl: string = "/Poedle/SkillGems/Game/Reset";
        const processData = (data: any) => {
            setIsResetGame(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    // Helpers
    function FetchApi() {
        GetIsWin();
        GetAvailGuesses();
        GetHints();
        GetResults();
    }
    enum ApiReturnType {
        None = 0,
        Json = 1,
        Text = 2
    }

    async function GetJsonDataFromApi(apiName: string, apiUrl: string, processData: (data: any) => void, apiReturnType: ApiReturnType) {
        console.log("BEGIN: " + apiName);
        const abortController = new AbortController();

        const response = await fetch(apiUrl);
        const data = await GetResponseData(response, apiReturnType);
        processData(data);

        console.log("END: " + apiName);
        return () => abortController.abort();
    }

    async function PostDataToApi(apiName: string, apiUrl: string, processData: ((data: any) => void) | null, apiReturnType: ApiReturnType) {
        console.log("BEGIN: " + apiName);
        const abortController = new AbortController();

        const requestOptions = GetRequestOptions(apiReturnType);
        const response = await fetch(apiUrl, requestOptions);
        const data = await GetResponseData(response, apiReturnType);
        if (processData) {
            processData(data);
        }

        console.log("END: " + apiName);
        return () => abortController.abort();
    }

    function GetRequestOptions(apiReturnType: ApiReturnType) {
        switch (apiReturnType) {
            case ApiReturnType.Json:
                return {
                    method: "POST",
                    headers: { "Content-Type": "application/json" }
                }
            case ApiReturnType.Text:
                return {
                    method: "POST",
                    headers: { "Content-Type": "application/text" }
                }
            case ApiReturnType.None:
            default:
                return {
                    method: "POST"
                };
        }
    }

    async function GetResponseData(response: Response, apiReturnType: ApiReturnType) {
        switch (apiReturnType) {
            case ApiReturnType.Json:
                return await response.json();
            case ApiReturnType.Text:
                return await response.text();
            case ApiReturnType.None:
            default:
                return null;
        }
    }



    const headers = [
        "Name", "Primary Attr", "% Dex", "% Int", "% Str", "Tag #", "Tags"
    ];

    const [availGuesses, setAvailGuesses] = useState<NullableDropDownItemList>(null);
    const [hints, setHints] = useState<NullableAllHints>(null);
    const [results, setResults] = useState<NullableSkillGemResultList>(null);
    const [stats, setStats] = useState<NullableStats>(null);
    const [isWin, setIsWin] = useState<boolean>(false);

    const [selectedGuess, setSelectedGuess] = useState<NullableDropDownItem>(null);
    const [processedGuess, setProcessedGuess] = useState<NullableSkillGemResult>(null);
    const [isClearStats, setIsClearStats] = useState<boolean>(false);
    const [isResetGame, setIsResetGame] = useState<boolean>(false);

    const onSelectGuess = (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => {
        console.log("OnSelect");
        actionMeta;
        setSelectedGuess(newValue);
    };

    const onClickGuess = () => {
        console.log("OnClickGuess");
        if (selectedGuess == null) {
            console.log("Selected Guess is null.");
            alert("Please make a guess first.");
            return;
        }

        ProcessGuess(selectedGuess.value);
    };

    const onClickPlayAgain = () => {
        console.log("OnClickPlayAgain");
        // Reset state
        setAvailGuesses(null);
        setHints(null);
        setResults(null);
        setStats(null);
        setIsWin(false);

        setSelectedGuess(null);
        setProcessedGuess(null);
        setIsClearStats(false);
        setIsResetGame(false);

        ResetGame();
    };

    const onClearStats = () => {
        console.log("OnClickClearStats");
        ClearStats();
    }

    // On Page Load
    useEffect(() => {
        console.log("Initial Load UseEffect");
        FetchApi();
    }, []);

    // On Reset Game
    useEffect(() => {
        console.log("Reset Game UseEffect");
        if (isResetGame) {
            setIsResetGame(false);
            FetchApi();
        }
    }, [isResetGame]);

    // Re-fetch data from API after guess processing
    useEffect(() => {
        console.log("ProcessedGuess/SelectedGuess UseEffect");
        if (processedGuess && selectedGuess) {
            setSelectedGuess(null);
            setProcessedGuess(null);
            FetchApi();
            GetIsWin();
        }
    }, [processedGuess && selectedGuess]);

    // On Win
    useEffect(() => {
        console.log("OnWin UseEffect");
        if (isWin) {
            GetStats();
        }
    }, [isWin]);

    // On Clear Stats
    useEffect(() => {
        console.log("OnClearStats UseEffect");
        if (isClearStats) {
            setIsClearStats(false);
            GetStats();
        }
    }, [isClearStats]);

    const hintsArea: HintsAreaProps = {
        hints: hints
    };

    const gameEndArea: GameEndAreaProps = {
        stats: stats,
        onClickPlayAgain: onClickPlayAgain,
        onClearStats: onClearStats
    };

    const resultsArea: ResultsAreaProps = {
        headers: headers,
        results: results
    };

    const guessArea: GuessAreaProps = {
        availGuesses: availGuesses,
        selectedGuess: selectedGuess,
        onSelectGuess: onSelectGuess,
        onClickGuess: onClickGuess,
        placeHolder: "Choose a Skill Gem"
    }

    return (
        <div className="div-game-page">
            <TitleHeader gameGuessType="Skill Gem" />
            <GameArea isWin={isWin} hintsArea={hintsArea} gameEndArea={gameEndArea} resultsArea={resultsArea} guessArea={guessArea} />
        </div>
    );
}

