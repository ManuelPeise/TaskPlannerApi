import React from "react";
import PageLayout from "./Lib/Layout/PageLayout";
import {
  BrowserRouter,
  Routes,
  Route,
  Outlet,
  Navigate,
} from "react-router-dom";
import UserAdministrationPageContainer from "./UserAdministration/UserAdministrationPage";
import UserDetailsPageContainer from "./UserAdministration/UserDetails/UserDetailsPage";
import UserActivationPageContainer from "./UserAdministration/UserActivation/UserActivationPage";
import { useAuth } from "./Hooks/useAuth";
import TaskAdministrationPageContainer from "./TaskAdministration/TaskAdministrationPage";
import TaskDetailsPageContainer from "./TaskAdministration/TaskDetails/TaskDetailsPage";
interface IProps {}

const PrivateRoute: React.FC = () => {
  const { isAuthenticated } = useAuth();
  if (!isAuthenticated) {
    return <Navigate to="/" />;
  }
  return <Outlet />;
};

const PublicRoute: React.FC = () => <Outlet />;

const MainPage: React.FC<IProps> = () => {
  return (
    <BrowserRouter>
      <PageLayout>
        <Routes>
          <Route path="/" element={<PrivateRoute />}>
            <Route path="/" element={<div>Welcome to the Task Planner!</div>} />
            <Route
              path="/user-administration"
              element={<UserAdministrationPageContainer />}
            />
            <Route
              path="/user-administration/details/:userId"
              element={<UserDetailsPageContainer />}
            />
            <Route
              path="/task-administration"
              element={<TaskAdministrationPageContainer />}
            />
            <Route
              path="/task-administration/details/:taskId"
              element={<TaskDetailsPageContainer />}
            />
            <Route path="/dashboard" element={<div>Dashboard</div>} />
          </Route>
          <Route path="/account" element={<PublicRoute />}>
            <Route
              path="/account/activate/:userId"
              element={<UserActivationPageContainer />}
            />
          </Route>
        </Routes>
      </PageLayout>
    </BrowserRouter>
  );
};

export default MainPage;
