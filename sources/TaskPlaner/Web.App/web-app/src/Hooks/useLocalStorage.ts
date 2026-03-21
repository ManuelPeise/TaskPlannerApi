import React from "react";

export enum LocalStorageKeys{
    Token = "token",
}

const useLocalStorage = <TModel>(key: LocalStorageKeys) => {
    const [storedValue, setStoredValue] = React.useState<TModel | null>(null);

    const getItem = React.useCallback(() => {
        const item = window.localStorage.getItem(key);
        return item ? JSON.parse(item) as TModel : null;
    }, [key]);

    const setItem = React.useCallback((value: TModel) => {
        setStoredValue(value);
        window.localStorage.setItem(key, JSON.stringify(value));
    }, [key]);

    const removeItem = React.useCallback(() => {
        setStoredValue(null);
        window.localStorage.removeItem(key);
    }, [key]);


    React.useEffect(() => {
        const item = getItem();
        
        if (item) {
            setStoredValue(item);
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [getItem]);

    return { data: storedValue, setItem, removeItem };
}

export default useLocalStorage;