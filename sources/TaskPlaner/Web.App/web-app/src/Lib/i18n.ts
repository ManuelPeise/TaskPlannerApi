import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import common_en from "./Localization/common_en.json";
import common_de from "./Localization/common_de.json";

const resources = {
  en: { common: common_en },
  de: { common: common_de },
};

i18n.use(initReactI18next).init({
  resources,
  lng: "en",
  fallbackLng: "en",
  ns: ["common"],
  defaultNS: "common",
  interpolation: {
    escapeValue: false,
  },
});

export default i18n;
