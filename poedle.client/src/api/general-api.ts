import { ApiReturnTypes } from "./api-return-type";
import { ApiTypes, GetApiBaseUrl } from "./api-type";

export class GeneralApi {
    protected baseUrl: string = "";
    protected apiType: ApiTypes = ApiTypes.None;
    protected fullBaseUrl: string = "";

    public constructor(baseUrl: string, apiType: ApiTypes) {
        this.baseUrl = baseUrl;
        this.apiType = apiType;
        this.fullBaseUrl = baseUrl + "/" + GetApiBaseUrl(apiType) + "/";
    }

    public GetOnRefresh(setIsWin: (data: any) => void, setAvailGuesses: (data: any) => void, setHints: (data: any) => void, setResults: (data: any) => void) {
        this.GetAndSetIsWin(setIsWin);
        this.GetAndSetAvailGuesses(setAvailGuesses);
        this.GetAndSetHints(setHints);
        this.GetAndSetResults(setResults);
    }

    /* GET queries */
    public async GetAndSetAvailGuesses(processData: (data: any) => void): Promise<void> {
        await this.GetJsonDataFromApi(this.fullBaseUrl + "Answers/AllAvailable", processData, ApiReturnTypes.Json);
    }

    public async GetAndSetHints(processData: (data: any) => void): Promise<void> {
        await this.GetJsonDataFromApi(this.fullBaseUrl + "Hints/All", processData, ApiReturnTypes.Json);
    }

    public async GetAndSetResults(processData: (data: any) => void): Promise<void> {
        await this.GetJsonDataFromApi(this.fullBaseUrl + "Results", processData, ApiReturnTypes.Json);
    }

    public async GetAndSetStats(processData: (data: any) => void): Promise<void> {
        await this.GetJsonDataFromApi(this.fullBaseUrl + "Stats", processData, ApiReturnTypes.Json);
    }

    public async GetAndSetIsWin(processData: (data: any) => void): Promise<void> {
        const processBooleanData = (data: any) => {
            processData(data == "true");
        }
        await this.GetJsonDataFromApi(this.fullBaseUrl + "Game/IsWin", processBooleanData, ApiReturnTypes.Text);
    }

    /* POST queries */
    public async PostAndSetProcessGuess(guessId: number, processData: (data: any) => void): Promise<void> {
        await this.PostDataToApi(this.fullBaseUrl + "Results/Process/" + guessId, processData, ApiReturnTypes.Json);
    }

    public async PostAndSetClearStats(processData: (data: any) => void): Promise<void> {
        const processBooleanData = (data: any) => {
            processData(data == "true");
        }
        await this.PostDataToApi(this.fullBaseUrl + "Stats/Clear", processBooleanData, ApiReturnTypes.Text);
    }

    public async PostAndSetResetGame(processData: (data: any) => void): Promise<void> {
        const processBooleanData = (data: any) => {
            processData(data == "true");
        }
        await this.PostDataToApi(this.fullBaseUrl + "Game/Reset", processBooleanData, ApiReturnTypes.Text);
    }

    /* Helpers */
    protected async GetJsonDataFromApi(apiUrl: string, processData: (data: any) => void, ApiReturnTypes: ApiReturnTypes): Promise<() => void> {
        console.log("BEGIN: " + apiUrl);
        const abortController = new AbortController();

        const response = await fetch(apiUrl);
        const data = await this.GetResponseData(response, ApiReturnTypes);
        processData(data);
        console.log("DATA: " + apiUrl);
        console.log(data);
        console.log("END: " + apiUrl);
        return () => abortController.abort();
    }

    protected async PostDataToApi(apiUrl: string, processData: (data: any) => void, ApiReturnTypes: ApiReturnTypes): Promise<() => void> {
        console.log("BEGIN: " + apiUrl);
        const abortController = new AbortController();

        const requestOptions = this.GetRequestOptions(ApiReturnTypes);
        const response = await fetch(apiUrl, requestOptions);
        const data = await this.GetResponseData(response, ApiReturnTypes);
        processData(data);

        console.log("END: " + apiUrl);
        return () => abortController.abort();
    }

    private GetRequestOptions(ApiReturnType: ApiReturnTypes) {
        switch (ApiReturnType) {
            case ApiReturnTypes.Json:
                return {
                    method: "POST",
                    headers: { "Content-Type": "application/json" }
                }
            case ApiReturnTypes.Text:
                return {
                    method: "POST",
                    headers: { "Content-Type": "application/text" }
                }
            case ApiReturnTypes.None:
            default:
                return {
                    method: "POST"
                };
        }
    }

    private async GetResponseData(response: Response, ApiReturnType: ApiReturnTypes) {
        switch (ApiReturnType) {
            case ApiReturnTypes.Json:
                return await response.json();
            case ApiReturnTypes.Text:
                return await response.text();
            case ApiReturnTypes.None:
            default:
                return null;
        }
    }
}