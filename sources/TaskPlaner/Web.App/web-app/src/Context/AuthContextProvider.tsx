import React, { PropsWithChildren } from "react";
import { IAuthenticationContext } from "../Lib/Interfaces/IAuthenticationContext";
import useApi from "../Hooks/useApi";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import { IUserData } from "../Lib/Interfaces/IUserData";

export const AuthContext = React.createContext<IAuthenticationContext>(
  {} as IAuthenticationContext,
);

export interface IAuthenticationRequestModel {
  emailAddress: string;
  password: string;
}

export const AuthContextProvider: React.FC<PropsWithChildren> = (props) => {
  const [isAuthenticated, setIsAuthenticated] = React.useState(false);
  const [currentUser, setCurrentUser] = React.useState<IUserData | null>(null);
  const [isLoading, setIsLoading] = React.useState(false);

  const authenticateRequestUrl = process.env.REACT_APP_API_URL
    ? `${process.env.REACT_APP_API_URL}Authentication/Authenticate`
    : "";

  const currentUserRequestUrl = process.env.REACT_APP_API_URL
    ? `${process.env.REACT_APP_API_URL}UserAdministration/LoadCurrentUser`
    : "";

  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);

  const authApi = useApi<ITokenData>();

  const currentUserApi = useApi<IUserData>();

  const onLogin = async (email: string, password: string) => {
    setIsLoading(true);
    const requestData: IAuthenticationRequestModel = {
      emailAddress: email,
      password,
    };
    const authResponse = await authApi.sendRequest({
      requestUrl: authenticateRequestUrl,
      method: "POST",
      model: requestData,
    });

    const storageData: ITokenData = {
      jwt: authResponse?.jwt || "",
      refreshToken: authResponse?.refreshToken || "",
    };

    storage.setItem(storageData);

    const currentUserResponse = await currentUserApi.sendRequest({
      requestUrl: currentUserRequestUrl,
      method: "POST",
      token: authResponse?.jwt || "",
    });

    setCurrentUser(currentUserResponse);

    setIsAuthenticated(true);
    setIsLoading(false);
  };

  const onLogout = () => {
    storage.removeItem();
    setIsAuthenticated(false);
    setCurrentUser(null);
  };

  const contextValue: IAuthenticationContext = {
    isAuthenticated,
    isLoading,
    currentUser,
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
