import React from "react";

export enum LocalStorageKeys {
  Token = "token",
}

const useLocalStorage = <TModel>(key: LocalStorageKeys) => {
  const getItem = React.useCallback((): TModel | null => {
    const item = window.localStorage.getItem(key);
    return item ? (JSON.parse(item) as TModel) : null;
  }, [key]);

  const setItem = React.useCallback(
    (value: TModel) => {
      window.localStorage.setItem(key, JSON.stringify(value));
    },
    [key],
  );

  const removeItem = React.useCallback(() => {
    window.localStorage.removeItem(key);
  }, [key]);

  return { setItem, getItem, removeItem };
};

export default useLocalStorage;
