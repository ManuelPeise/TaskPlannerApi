import React from "react";
import { AuthContext } from "../Context/AuthContextProvider";

export const useAuth = () => {
  const context = React.useContext(AuthContext);
    if (!context) {
        throw new Error("useAuth must be used within an AuthContextProvider");
    }
    return context;
};