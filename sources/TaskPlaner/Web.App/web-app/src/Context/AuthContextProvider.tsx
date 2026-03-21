import React, { PropsWithChildren } from "react";
import { IAuthenticationContext } from "../Lib/Interfaces/IAuthenticationContext";
import useApi from "../Hooks/useApi";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import { ITokenData } from "../Lib/Interfaces/ITokenData";

export const AuthContext = React.createContext<IAuthenticationContext>(
  {} as IAuthenticationContext,
);

export interface IAuthenticationRequestModel {
  emailAddress: string;
  password: string;
}

export const AuthContextProvider: React.FC<PropsWithChildren> = (props) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);

  const requestUrl = process.env.REACT_APP_API_URL
    ? `${process.env.REACT_APP_API_URL}authentication/authenticate`
    : "";

  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);
  const api = useApi<ITokenData>({ requestUrl, method: "POST" });

  const onLogin = async (email: string, password: string) => {
    const requestData: IAuthenticationRequestModel = {
      emailAddress: email,
      password,
    };
    await api.sendRequest({ requestUrl, method: "POST", model: requestData });

    if (api.data?.jwt && api.data?.refreshToken) {
      const storageData: ITokenData = {
        jwt: api.data.jwt,
        refreshToken: api.data.refreshToken,
        emailAddress: email,
      };
      storage.setItem(storageData);
      setIsAuthenticated(true);
    }
  };

  const onLogout = () => {
    storage.removeItem();
    setIsAuthenticated(false);
  };

  const contextValue: IAuthenticationContext = {
    isAuthenticated,
    emailAddress: storage.data?.emailAddress,
    onLogin,
    onLogout,
  };

  return (
    <AuthContext.Provider value={contextValue}>
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthContextProvider;
