import React from "react";
import { useTranslation } from "react-i18next";

export const useLocalization = () => {
  const { t } = useTranslation();

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

  return {
    getResource,
  };
};
