import React from "react";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import useStateFulApiService from "../Hooks/useStateFulApiService";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import { ITaskPageModel } from "./Interfaces/ITaskPageModel";
import { Grid } from "@mui/material";
import { IDropdownItem } from "../Lib/Interfaces/IDropdownItem";
import TaskToolbar from "./Components/TaskToolbar";
import TaskTable from "./Components/TaskTable";
import { ITaskFilterOptions } from "./Interfaces/ITaskFilterOptions";
import { TaskTypeEnum } from "../Lib/Enums/TaskTypeEnum";
import { ITaskItemBase } from "./Interfaces/ITaskItemBase";
import { useLocalization } from "../Hooks/useLocalization";
import { TaskStatusEnum } from "../Lib/Enums/TaskStatusEnum";
import useStatelessApi from "../Hooks/useStatelessApi";
import LoadingIndicator from "../Components/LoadingIndicator";

interface IProps {
  isLoading: boolean;
  filterOptions?: ITaskFilterOptions;
  availableTasks: ITaskItemBase[];
  userDropdownItems: IDropdownItem[];
  handleAddTask: (task: ITaskItemBase) => Promise<ITaskItemBase[]>;
  handleFilterChange: (options: Partial<ITaskFilterOptions>) => void;
  handleUpdateTaskBase: (
    task: ITaskItemBase,
    callBack: (task: ITaskItemBase) => void,
  ) => void;
  handleDeleteTask: (taskId: number) => Promise<ITaskItemBase[]>;
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

  const taskUpdateApi = useStatelessApi();

  const handleAddTask = React.useCallback(
    async (task: ITaskItemBase): Promise<ITaskItemBase[]> => {
      const resopnse = await taskUpdateApi.sendRequest<ITaskItemBase[]>({
        method: "POST",
        requestUrl:
          process.env.REACT_APP_API_URL + `taskadministration/addtask`,
        model: task,
        token: storage.getItem()?.jwt ?? null,
      });

      return resopnse ?? [];
    },
    [taskUpdateApi, storage],
  );

  const handleDeleteTask = React.useCallback(
    async (taskId: number): Promise<ITaskItemBase[]> => {
      const resopnse = await taskUpdateApi.sendRequest<ITaskItemBase[]>({
        method: "POST",
        requestUrl:
          process.env.REACT_APP_API_URL +
          `taskadministration/deletetask?taskid=${taskId}`,
        token: storage.getItem()?.jwt ?? null,
      });

      return resopnse ?? [];
    },
    [taskUpdateApi, storage],
  );

  const handleUpdateTaskBase = React.useCallback(
    async (task: ITaskItemBase, callBack: (task: ITaskItemBase) => void) => {
      await taskUpdateApi
        .sendRequest<ITaskItemBase>({
          method: "POST",
          requestUrl:
            process.env.REACT_APP_API_URL + `taskadministration/updatetaskbase`,
          model: task,
          token: storage.getItem()?.jwt ?? null,
        })
        .then((res) => {
          if (res) {
            callBack(res);
          }
        });
    },
    [taskUpdateApi, storage],
  );

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
      userDropdownItems={taskApi.response.userDropdownItems}
      filterOptions={taskFilterOptions}
      handleAddTask={handleAddTask}
      isLoading={taskApi.loading || taskUpdateApi.loading}
      handleFilterChange={handleFilterChange}
      handleUpdateTaskBase={handleUpdateTaskBase}
      handleDeleteTask={handleDeleteTask}
    />
  );
};

const TaskAdministrationPage: React.FC<IProps> = (props) => {
  const {
    isLoading,
    filterOptions,
    userDropdownItems,
    availableTasks,
    handleAddTask,
    handleFilterChange,
    handleUpdateTaskBase,
    handleDeleteTask,
  } = props;

  const { getResource } = useLocalization();

  const [tasks, setTasks] = React.useState<ITaskItemBase[]>(availableTasks);

  const handleUpdateTaskBaseWithStateUpdate = React.useCallback(
    async (updatedTask: ITaskItemBase) => {
      setTasks((prevTasks) =>
        prevTasks.map((t) => (t.id === updatedTask.id ? updatedTask : t)),
      );
    },
    [],
  );

  const handleSaveTask = React.useCallback(
    async (task: ITaskItemBase) => {
      const updatedTasks = await handleAddTask(task);
      setTasks(updatedTasks);
    },
    [handleAddTask],
  );

  const handleMoveTask = React.useCallback(
    async (updatedTask: ITaskItemBase, status: TaskStatusEnum) => {
      const task = tasks.find((t) => t.id === updatedTask.id);
      if (!task) {
        return;
      }

      task.status = status;

      await handleUpdateTaskBase?.(
        updatedTask,
        handleUpdateTaskBaseWithStateUpdate,
      );
    },
    [tasks, handleUpdateTaskBase, handleUpdateTaskBaseWithStateUpdate],
  );

  const handleAssignUser = React.useCallback(
    async (updatedTask: ITaskItemBase) => {
      const task = tasks.find((t) => t.id === updatedTask.id);

      if (!task) {
        return;
      }

      task.assignedUserId = updatedTask.assignedUserId;

      await handleUpdateTaskBase?.(
        updatedTask,
        handleUpdateTaskBaseWithStateUpdate,
      );
    },
    [tasks, handleUpdateTaskBase, handleUpdateTaskBaseWithStateUpdate],
  );

  const onDeleteTask = React.useCallback(
    async (taskId: number) => {
      const updatedTasks = await handleDeleteTask(taskId);
      setTasks(updatedTasks);
    },
    [handleDeleteTask],
  );

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
    return tasks.filter((task) => {
      const matchesUserFilter = filterOptions?.assignedUserId
        ? task.assignedUserId === filterOptions.assignedUserId
        : true;

      const matchesTypeFilter = filterOptions?.taskTypeId
        ? task.taskTypeId === filterOptions.taskTypeId
        : true;

      return matchesUserFilter && matchesTypeFilter;
    });
  }, [tasks, filterOptions]);

  return (
    <Grid container spacing={2}>
      <TaskToolbar
        isloading={isLoading}
        filterOptions={filterOptions}
        userDropdownItems={userFilterDropdownItems}
        handleSaveTask={handleSaveTask}
        handleFilterChange={handleFilterChange}
      />
      <TaskTable
        tasks={filteredTasks}
        userDropdownItems={assignedUserDropdownItems}
        handleMoveTask={handleMoveTask}
        handleAssignUser={handleAssignUser}
        handleDeleteTask={onDeleteTask}
      />
      <LoadingIndicator isLoading={isLoading} />
    </Grid>
  );
};

export default TaskAdministrationPageContainer;
