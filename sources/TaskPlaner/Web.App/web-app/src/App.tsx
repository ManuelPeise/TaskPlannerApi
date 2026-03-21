import React from "react";
import "./App.css";
import AuthContextProvider from "./Context/AuthContextProvider";
import MainPage from "./MainPage";

const App: React.FC = () => {
  return (
    <AuthContextProvider>
      <MainPage />
    </AuthContextProvider>
  );
};

export default App;
