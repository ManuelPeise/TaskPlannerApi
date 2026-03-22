import React from "react";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import useStateFulApiService from "../Hooks/useStateFulApiService";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import { ITaskPageModel } from "./Interfaces/ITaskPageModel";
import { Grid } from "@mui/material";
import { ITaskModel } from "./Interfaces/ITaskModel";
import { IDropdownItem } from "../Lib/Interfaces/IDropdownItem";
import TaskToolbar from "./Components/TaskToolbar";
import TaskTable from "./Components/TaskTable";
import { ITaskFilterOptions } from "./Interfaces/ITaskFilterOptions";
import { TaskTypeEnum } from "../Lib/Enums/TaskTypeEnum";
import { ITaskItemProps } from "./Interfaces/ITaskItemProps";
import { dummyTaskDataCollection } from "./dummyData";
import { useLocalization } from "../Hooks/useLocalization";
import { TaskStatusEnum } from "../Lib/Enums/TaskStatusEnum";

interface IProps {
  filterOptions?: ITaskFilterOptions;
  availableTasks: ITaskModel[];
  gitRepositoryDropdownItems: IDropdownItem[];
  userDropdownItems: IDropdownItem[];
  handleFilterChange?: (options: Partial<ITaskFilterOptions>) => void;
}

const TaskAdministrationPageContainer: React.FC = () => {
  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);

  const [taskFilterOptions, setTaskFilterOptions] =
    React.useState<ITaskFilterOptions>({
      assignedUserId: 0,
      taskTypeId: TaskTypeEnum.All,
    });

  const taskApi = useStateFulApiService<ITaskPageModel>({
    method: "GET",
    requestUrl:
      process.env.REACT_APP_API_URL + `taskadministration/gettaskpagemodel`,
    token: storage.getItem()?.jwt ?? null,
  });

  const handleFilterChange = React.useCallback(
    (options: Partial<ITaskFilterOptions>) => {
      setTaskFilterOptions({ ...taskFilterOptions, ...options });
    },
    [taskFilterOptions],
  );

  if (taskApi.loading || !taskApi.response) {
    return null;
  }

  return (
    <TaskAdministrationPage
      availableTasks={taskApi.response.taskModels}
      gitRepositoryDropdownItems={taskApi.response.gitDropdownItems}
      userDropdownItems={taskApi.response.userDropdownItems}
      filterOptions={taskFilterOptions}
      handleFilterChange={handleFilterChange}
    />
  );
};

const TaskAdministrationPage: React.FC<IProps> = (props) => {
  const {
    filterOptions,
    gitRepositoryDropdownItems,
    userDropdownItems,
    handleFilterChange,
  } = props;

  const { getResource } = useLocalization();

  const [dummyTasks, setDummyTasks] = React.useState<ITaskItemProps[]>(
    dummyTaskDataCollection,
  );

  const handleTaskChanged = React.useCallback(
    (updatedTask: ITaskItemProps, status: TaskStatusEnum) => {
      setDummyTasks((prevTasks) =>
        prevTasks.map((task) =>
          task.id === updatedTask.id ? { ...updatedTask, status } : task,
        ),
      );
    },
    [],
  );

  const handleAssignUser = React.useCallback((task: ITaskItemProps) => {
    setDummyTasks((prevTasks) =>
      prevTasks.map((t) =>
        t.id === task.id ? { ...t, assignedUserId: task.assignedUserId } : t,
      ),
    );
  }, []);

  const assignedUserDropdownItems = React.useMemo((): IDropdownItem[] => {
    return userDropdownItems.map((item) => {
      return item.id === 0
        ? { id: item.id, label: getResource("labelUnassigned") }
        : item;
    });
  }, [userDropdownItems, getResource]);

  const userFilterDropdownItems = React.useMemo((): IDropdownItem[] => {
    return userDropdownItems.map((item) => {
      return item.id === 0
        ? { id: item.id, label: getResource("labelAll") }
        : item;
    });
  }, [userDropdownItems, getResource]);

  const filteredTasks = React.useMemo(() => {
    return dummyTasks.filter((task) => {
      const matchesUserFilter = filterOptions?.assignedUserId
        ? task.assignedUserId === filterOptions.assignedUserId
        : true;

      const matchesTypeFilter = filterOptions?.taskTypeId
        ? task.taskTypeId === filterOptions.taskTypeId
        : true;

      return matchesUserFilter && matchesTypeFilter;
    });
  }, [dummyTasks, filterOptions]);

  return (
    <Grid container spacing={2}>
      <TaskToolbar
        filterOptions={filterOptions}
        gitRepositoryDropdownItems={gitRepositoryDropdownItems}
        userDropdownItems={userFilterDropdownItems}
        handleFilterChange={handleFilterChange}
      />
      <TaskTable
        tasks={filteredTasks}
        userDropdownItems={assignedUserDropdownItems}
        handleTaskChanged={handleTaskChanged}
        handleAssignUser={handleAssignUser}
      />
    </Grid>
  );
};

export default TaskAdministrationPageContainer;
