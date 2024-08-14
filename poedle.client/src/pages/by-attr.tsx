import { MouseEventHandler, useEffect, useState } from "react";
import { ActionMeta, SingleValue } from "react-select";

import { TitleHeader } from "../shared/comps/title-header.tsx"
import { GuessArea } from "../guess/guess-area.tsx";
import { HintsArea } from "../hints/hints-area.tsx";
import { AttrResultsArea } from "../results/results-area.tsx";
import { GameEndArea } from "../stats/game-end-area.tsx";

import { DropDownItem, NullableDropDownItem, NullableDropDownItemList } from "../shared/types/drop-down-item.ts";
import { NullableAttrResult, NullableAttrResultList } from "../results/types/results-type-def.ts";
import { NullableStats, Stats } from "../stats/types/stats-type-def.ts";

function GameArea() {
    const headers = [
        "Name", "Item Class", "Base Item", "Leagues Introduced", "Item Aspects", "Drop Sources", "Drop Types", "Req Lvl", "Req Dex", "Req Int", "Req Str"
    ];

    const [availGuesses, setAvailGuesses] = useState<NullableDropDownItemList>(null);
    const [nameHint, setNameHint] = useState<string>("");
    const [nbrGuessForHint, setNbrGuessForHint] = useState<number | null>(null);
    const [results, setResults] = useState<NullableAttrResultList>(null);
    const [stats, setStats] = useState<NullableStats>(null);
    const [isWin, setIsWin] = useState<boolean>(false);

    const [selectedGuess, setSelectedGuess] = useState<NullableDropDownItem>(null);
    const [processedGuess, setProcessedGuess] = useState<NullableAttrResult>(null);
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
        setNameHint("");
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

    const gameArea = isWin ?
        <>
            <WinGameArea stats={stats!} onClickPlayAgain={onClickPlayAgain} onClearStats={onClearStats} />
            <CommonGameArea hint={nameHint} nbrGuessForHint={nbrGuessForHint} headers={headers} results={results} />
        </> :
        <>
            <ContGameArea availGuesses={availGuesses} selectedGuess={selectedGuess} onSelectGuess={onSelectGuess} onClickGuess={onClickGuess} />
            <CommonGameArea hint={nameHint} nbrGuessForHint={nbrGuessForHint} headers={headers} results={results} />
        </>;

    return (
        <div>
            {gameArea}
        </div>
    );

    // Retrieve data from API
    async function GetAvailGuesses() {
        const apiName: string = "GetAvailGuesses";
        const apiUrl: string = "/Poedle/UniqueByAttr/Answers/AllAvailable";
        const processData = (data: any) => {
            setAvailGuesses(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function GetNameHint() {
        const apiName: string = "GetNameHint";
        const apiUrl: string = "/Poedle/UniqueByAttr/Hints/Name";
        const processData = (data: any) => {
            setNameHint(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function GetNbrGuessesForHint() {
        const apiName: string = "GetNbrGuessesForHint";
        const apiUrl: string = "/Poedle/UniqueByAttr/Hints/NbrGuessRemaining";
        const processData = (data: any) => {
            setNbrGuessForHint(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function GetResults() {
        const apiName: string = "GetResults";
        const apiUrl: string = "/Poedle/UniqueByAttr/Results";
        const processData = (data: any) => {
            setResults(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function GetIsWin() {
        const apiName: string = "GetIsWin";
        const apiUrl: string = "/Poedle/UniqueByAttr/Game/IsWin";
        const processData = (data: any) => {
            setIsWin(data == "true");
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function GetStats() {
        const apiName: string = "GetStats";
        const apiUrl: string = "/Poedle/UniqueByAttr/Stats";
        const processData = (data: any) => {
            setStats(data);
        };

        await GetJsonDataFromApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    // Post data to API for processing
    async function ProcessGuess(guessId: number) {
        const apiName: string = "ProcessGuess";
        const apiUrl: string = "/Poedle/UniqueByAttr/Results/Process/" + guessId;
        const processData = (data: any) => {
            setProcessedGuess(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Json);
    }

    async function ClearStats() {
        const apiName: string = "ClearStats";
        const apiUrl: string = "/Poedle/UniqueByAttr/Stats/Clear";
        const processData = (data: any) => {
            setIsClearStats(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    async function ResetGame() {
        const apiName: string = "ResetGame";
        const apiUrl: string = "/Poedle/UniqueByAttr/Game/Reset";
        const processData = (data: any) => {
            setIsResetGame(data);
        };

        await PostDataToApi(apiName, apiUrl, processData, ApiReturnType.Text);
    }

    // Helpers
    function FetchApi() {
        GetIsWin();
        GetAvailGuesses();
        GetNameHint();
        GetNbrGuessesForHint();
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


        const data = await response.json();
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
}

interface CommonGameAreaProps {
    hint: string,
    nbrGuessForHint: number | null,
    headers: string[],
    results: NullableAttrResultList
}

function CommonGameArea({ hint, nbrGuessForHint, headers, results }: CommonGameAreaProps) {
    return (
        <>
            <HintsArea hints={hint} nbrGuessForHint={nbrGuessForHint} />
            <AttrResultsArea headers={headers} results={results} />
        </>
    );
}

interface WinGameAreaProps {
    stats: Stats,
    onClickPlayAgain: MouseEventHandler,
    onClearStats: MouseEventHandler
}

function WinGameArea({ stats, onClickPlayAgain, onClearStats }: WinGameAreaProps) {
    return (
        <>
            <GameEndArea stats={stats} onClickPlayAgain={onClickPlayAgain} onClearStats={onClearStats} />
        </>
    );
}

interface ContGameAreaProps {
    availGuesses: NullableDropDownItemList,
    selectedGuess: NullableDropDownItem,
    onSelectGuess: (newValue: SingleValue<DropDownItem>, actionMeta: ActionMeta<DropDownItem>) => void,
    onClickGuess: MouseEventHandler
}

function ContGameArea({ availGuesses, selectedGuess, onSelectGuess, onClickGuess }: ContGameAreaProps) {
    return (
        <>
            <GuessArea availGuesses={availGuesses} selectedGuess={selectedGuess} onSelectGuess={onSelectGuess} onClickGuess={onClickGuess} />
        </>
    );
}

export function ByAttr() {
    return (
        <div>
            <TitleHeader gameGuessType="Attribute" />
            <GameArea />
        </div>
    );
}