import React from "react";
import { useTranslation } from "react-i18next";
import i18n from "../Lib/i18n";

export const useLocalization = () => {
  const { t } = useTranslation();

  const [currentLanguage, setCurrentLanguage] = React.useState(i18n.language);
  const getResource = React.useCallback(
    (key: string): string => {
      if (key.includes(".")) {
        const [namespace, ...resourceParts] = key.split(".");
        const resource = resourceParts.join(".");
        return t(resource, { ns: namespace });
      }
      return t(key);
    },
    [t],
  );

  const toggleLanguage = React.useCallback(() => {
    const currentLanguage = i18n.language;
    const newLanguage = currentLanguage === "en" ? "de" : "en";
    i18n.changeLanguage(newLanguage);
    setCurrentLanguage(newLanguage);
  }, []);

  return {
    currentLanguage,
    getResource,
    toggleLanguage,
  };
};
