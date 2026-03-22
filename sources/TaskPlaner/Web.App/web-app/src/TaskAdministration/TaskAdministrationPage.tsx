import React from "react";
import { useAuth } from "../Hooks/useAuth";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import useStateFulApiService from "../Hooks/useStateFulApiService";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import { ITaskPageModel } from "./Interfaces/ITaskPageModel";
import { Grid } from "@mui/material";
import { useLocalization } from "../Hooks/useLocalization";
import { ITaskModel } from "./Interfaces/ITaskModel";
import { IDropdownItem } from "../Lib/Interfaces/IDropdownItem";
import TaskToolbar from "./Components/TaskToolbar";
import TaskTable from "./Components/TaskTable";

interface IProps {
  availableTasks: ITaskModel[];
  gitRepositoryDropdownItems: IDropdownItem[];
  userDropdownItems: IDropdownItem[];
}

const TaskAdministrationPageContainer: React.FC = () => {
  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);

  const taskApi = useStateFulApiService<ITaskPageModel>({
    method: "GET",
    requestUrl:
      process.env.REACT_APP_API_URL + `taskadministration/gettaskpagemodel`,
    token: storage.getItem()?.jwt ?? null,
  });

  if (taskApi.loading || !taskApi.response) {
    return null;
  }

  return (
    <TaskAdministrationPage
      availableTasks={taskApi.response.taskModels}
      gitRepositoryDropdownItems={taskApi.response.gitDropdownItems}
      userDropdownItems={taskApi.response.userDropdownItems}
    />
  );
};

const TaskAdministrationPage: React.FC<IProps> = (props) => {
  const { availableTasks } = props;
  const {} = useLocalization();

  return (
    <Grid container spacing={2}>
      <TaskToolbar />
      <TaskTable tasks={availableTasks} />
    </Grid>
  );
};

export default TaskAdministrationPageContainer;
