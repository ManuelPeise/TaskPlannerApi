import React from "react";

export interface IUseApiOptions {
  requestUrl: string;
  method: "GET" | "POST" | "PUT" | "DELETE";
  model?: any;
  token?: string | null;
}

const useStatelessApi = () => {
  const [error, setError] = React.useState<string | null>(null);
  const [loading, setLoading] = React.useState<boolean>(false);

  const sendRequest = React.useCallback(
    async <TResponse>(
      apiOptions: IUseApiOptions,
    ): Promise<TResponse | null> => {
      let responseJson: TResponse | null = null;
      try {
        setLoading(true);

        const headers = {
          "Content-Type": "application/json",
          Authentication: `Bearer ${apiOptions.token}`,
        };

        const response = await fetch(apiOptions.requestUrl, {
          method: apiOptions.method,
          mode: "cors",
          headers: headers,
          body: apiOptions.model ? JSON.stringify(apiOptions.model) : null,
        });

        if (response.status === 200) {
          responseJson = await response.json();
        }
      } catch (err) {
        setError(
          err instanceof Error ? err.message : "An unknown error occurred",
        );
      } finally {
        setLoading(false);
      }

      return responseJson;
    },
    [],
  );

  return { error, loading, sendRequest };
};

export default useStatelessApi;
