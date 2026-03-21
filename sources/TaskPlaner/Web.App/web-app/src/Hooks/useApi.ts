import React from "react";
import useLocalStorage, { LocalStorageKeys } from "./useLocalStorage";

interface IUseApiOptions {
    requestUrl: string;
    method: "GET" | "POST" | "PUT" | "DELETE";
    model?: any;
}

const useApi = <TResponse>(options: IUseApiOptions) => {
    const [data, setData] = React.useState<TResponse | null>(null);
    const [error, setError] = React.useState<string | null>(null);
    const [loading, setLoading] = React.useState<boolean>(false);

    const storage = useLocalStorage<string>(LocalStorageKeys.Token);

    const sendRequest = React.useCallback(async (apiOptions?: IUseApiOptions) =>{
        try{

            let requestOptions = options;
            
            if(apiOptions){
                requestOptions = {...options, ...apiOptions};
            }
           
            setLoading(true);

            await fetch(requestOptions.requestUrl, {
                method: requestOptions.method,
                headers: {
                    "Content-Type": "application/json",
                    "Authentication": storage.data ? `Bearer ${storage.data}` : ""
                },
                body: requestOptions.model ? JSON.stringify(requestOptions.model) : null
            }).then(async res => {
                if (!res.ok) {
                    throw new Error(`HTTP error! status: ${res.status}`);
                }

                const responseData = await res.json();

                if(responseData){
                    const model: TResponse = JSON.parse(JSON.stringify(responseData)) as TResponse;
                    setData(model);
                }
               
            })
        }catch(err){
            setError(err instanceof Error ? err.message : "An unknown error occurred");
        }finally{
            setLoading(false);
        }
    }, [options]);

    React.useEffect(() => {
        if(options.method === "GET"){
            const sendRequestAsync = async () => {
                await sendRequest();
            }

            sendRequestAsync();
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    },[]);

    return { data, error, loading, sendRequest };
}

export default useApi;