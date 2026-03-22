import React from "react";
import useStatelessApi, { IUseApiOptions } from "./useStatelessApi";

const useStateFulApiService = <TResponse>(apiOptions: IUseApiOptions) => {
  const [error, setError] = React.useState<string | null>(null);
  const [loading, setLoading] = React.useState<boolean>(false);
  const [response, setResponse] = React.useState<TResponse | null>(null);

  const api = useStatelessApi();

  const sendRequest = React.useCallback(
    async (options: IUseApiOptions) => {
      setError(null);
      setLoading(true);
      try {
        const apiResponse = await api.sendRequest<TResponse>(options);
        setResponse(apiResponse);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "An unknown error occurred",
        );
      } finally {
        setLoading(false);
      }
    },
    [api],
  );

  React.useEffect(() => {
    const sendRequest = async () => {
      try {
        setLoading(true);
        const apiResponse = await api.sendRequest<TResponse>(apiOptions);
        setResponse(apiResponse);
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "An unknown error occurred",
        );
      } finally {
        setLoading(false);
      }
    };

    if (apiOptions.method === "GET") {
      sendRequest();
    }
    //eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return { error, loading, response, sendRequest };
};

export default useStateFulApiService;
