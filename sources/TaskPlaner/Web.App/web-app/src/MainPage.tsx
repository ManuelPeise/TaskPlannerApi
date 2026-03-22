import React from "react";
import PageLayout from "./Lib/Layout/PageLayout";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import UserAdministrationPageContainer from "./UserAdministration/UserAdministrationPage";
import UserDetailsPageContainer from "./UserAdministration/UserDetails/UserDetailsPage";
interface IProps {}

const MainPage: React.FC<IProps> = () => {
  return (
    <BrowserRouter>
      <PageLayout>
        <Routes>
          <Route path="/" element={<div>MainPage</div>} />
          <Route
            path="/user-administration"
            element={<UserAdministrationPageContainer />}
          />
          <Route
            path="/user-administration/details/:userId"
            element={<UserDetailsPageContainer />}
          />
        </Routes>
      </PageLayout>
    </BrowserRouter>
  );
};

export default MainPage;
