import { useEffect, useState } from "react";
import { ActionMeta, SingleValue } from "react-select";
import { ApiTypes } from "../../api/api-type.ts";
import { GeneralApi } from "../../api/general-api.ts";
import { GuessesAreaProps, HintsAreaProps, StatsAreaProps, UniqueItemResultsAreaProps } from "../base-game/base-area-props.ts";
import { DropDownItem, NullableDropDownItem, NullableDropDownItemList } from "../../common/drop-down-item/drop-down-item.ts";
import { LoadingText } from "../../common/loading/loading-text.tsx";
import { PageTitle } from "../../common/page-title/page-title.tsx";
import { NullableAny } from "../../common/type-defs.ts";
import { GuessesArea } from "../../guesses/guesses-area.tsx";
import { GuessSelectOptions } from "../../guesses/guess-select/guess-select.tsx";
import { UniqueItemHintsArea } from "../../hints/hints-area.tsx";
import { NullableAllHints } from "../../hints/hint/hint.ts";
import { UniqueItemResultsArea } from "../../results/results-area.tsx";
import { NullableUniqueItemResult, NullableUniqueItemResultList } from "../../results/result/result.ts";
import { StatsArea } from "../../stats/stats-area.tsx";
import { NullableStats } from "../../stats/stats/stats.ts";

import "./unique-item-game.css";
export function UniqueItemsGame() {
    const api: GeneralApi = new GeneralApi("Poedle", ApiTypes.UniqueItems);

    /* States */
    /* States for storing from API */
    const [availGuesses, setAvailGuesses] = useState<NullableDropDownItemList>(null);
    const [hints, setHints] = useState<NullableAllHints>(null);
    const [results, setResults] = useState<NullableUniqueItemResultList>(null);
    const [stats, setStats] = useState<NullableStats>(null);
    const [isWin, setIsWin] = useState<boolean>(false);
    /* States for handling flags, processing, and handlers */
    const [selectedGuess, setSelectedGuess] = useState<NullableDropDownItem>(null);
    const [processedGuess, setProcessedGuess] = useState<NullableUniqueItemResult>(null);
    const [isClearStats, setIsClearStats] = useState<boolean>(false);
    const [isResetGame, setIsResetGame] = useState<boolean>(false);

    /* Handlers */
    // Handles when user selects a guess from the guess-select dropdown.
    const onChangeSelectGuess = async (newValue: SingleValue<DropDownItem>, _actionMeta: ActionMeta<DropDownItem>) => {
        setSelectedGuess(newValue);
    };
    // Handles when user clicks the Guess button in the guesses area.
    const onClickGuess = async () => {
        if (selectedGuess == null) {
            alert("Please make a guess first.");
            return;
        }

        api.PostAndSetProcessGuess(selectedGuess.value, setProcessedGuess);
    };
    // Handles when user clicks on the Play Again? button after the game ends.
    const onClickPlayAgain = async () => {
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

        api.PostAndSetResetGame(setIsResetGame);
    };
    // Handles when user clicks on the 
    const onClickClearStats = () => {
        api.PostAndSetClearStats(setIsClearStats);
    }

    /* Side Effect Hooks */
    /* On Initial Load Only */
    useEffect(() => {
        api.GetOnRefresh(setIsWin, setAvailGuesses, setHints, setResults);
    }, []);
    /* On Reset Game */
    useEffect(() => {
        if (isResetGame) {
            setIsResetGame(false);
            api.GetOnRefresh(setIsWin, setAvailGuesses, setHints, setResults);
        }
    }, [isResetGame]);
    /* On Guess Is Selected and Processed */
    useEffect(() => {
        if (processedGuess && selectedGuess) {
            setSelectedGuess(null);
            setProcessedGuess(null);
            api.GetOnRefresh(setIsWin, setAvailGuesses, setHints, setResults);
        }
    }, [processedGuess && selectedGuess]);
    /* On Win */
    useEffect(() => {
        if (isWin) {
            api.GetAndSetStats(setStats);
        }
    }, [isWin]);
    /* On Clear Stats */
    useEffect(() => {
        if (isClearStats) {
            setIsClearStats(false);
            api.GetAndSetStats(setStats);
        }
    }, [isClearStats]);

    /* Props for the other Components */
    const selectOptions: GuessSelectOptions = {
        ignoreAccents: true,
        ignoreCase: true,
        isSearchable: true,
        trim: true,
        maxMenuHeight: 250,
        matchFrom: "any",
        placeHolder: "Select a Unique Item"
    }

    const guessesArea: GuessesAreaProps = {
        availGuesses: availGuesses,
        selectedGuess: selectedGuess,
        onChange: onChangeSelectGuess,
        onClick: onClickGuess,
        options: selectOptions
    }

    const hintsArea: HintsAreaProps = {
        hints: hints
    };

    const resultsArea: UniqueItemResultsAreaProps = {
        results: results
    };

    const statsArea: StatsAreaProps = {
        stats: stats,
        onClickPlayAgain: onClickPlayAgain,
        onClickClearStats: onClickClearStats,
        answerClassName: "txt-unique-item"
    };

    return (
        <div className="div-game">
            <PageTitle gameType="Unique Item" />
            <GameArea isWin={isWin} guessesArea={guessesArea} hintsArea={hintsArea} resultsArea={resultsArea} statsArea={statsArea} />
        </div>
    );
}

interface GameAreaProps {
    readonly isWin: boolean
    readonly guessesArea: GuessesAreaProps
    readonly hintsArea: HintsAreaProps
    readonly resultsArea: UniqueItemResultsAreaProps
    readonly statsArea: StatsAreaProps
}

function GameArea({ isWin, guessesArea, hintsArea, resultsArea, statsArea }: GameAreaProps) {
    const apiData: NullableAny[] = isWin ? [hintsArea.hints, resultsArea.results, statsArea.stats] : [guessesArea.availGuesses, hintsArea.hints, resultsArea.results];
    if (apiData.some((x) => x == null)) {
        return (
            <LoadingText />
        );
    }

    const gameArea = isWin ?
        <>
            <UniqueItemHintsArea hints={hintsArea.hints} isWin={true} />
            <StatsArea stats={statsArea.stats} onClickPlayAgain={statsArea.onClickPlayAgain} onClickClearStats={statsArea.onClickClearStats} answerClassName={statsArea.answerClassName} />
            <UniqueItemResultsArea results={resultsArea.results} />
        </> :
        <>
            <UniqueItemHintsArea hints={hintsArea.hints} isWin={false} />
            <GuessesArea availGuesses={guessesArea.availGuesses} selectedGuess={guessesArea.selectedGuess} onChange={guessesArea.onChange} onClick={guessesArea.onClick} options={guessesArea.options} />
            <UniqueItemResultsArea results={resultsArea.results} />
        </>;

    return (
        <div id="div-game-area">
            {gameArea}
        </div>
    );
}