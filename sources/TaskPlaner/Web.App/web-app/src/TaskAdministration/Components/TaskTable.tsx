import React from "react";
import { Grid } from "@mui/material";
import TaskTableColumnHeader from "./TaskTableColumnHeader";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import TaskDropColumn from "./TaskDropColumn";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { ITaskItemBase } from "../Interfaces/ITaskItemBase";

interface IProps {
  tasks: ITaskItemBase[];
  userDropdownItems: IDropdownItem[];
  handleMoveTask: (updatedTask: ITaskItemBase, status: TaskStatusEnum) => void;
  handleAssignUser: (task: ITaskItemBase) => void;
  handleDeleteTask: (taskId: number) => Promise<void>;
}

const TaskTable: React.FC<IProps> = (props) => {
  const {
    tasks,
    userDropdownItems,
    handleMoveTask,
    handleAssignUser,
    handleDeleteTask,
  } = props;

  const handleDragStart = React.useCallback(
    (event: React.DragEvent<HTMLDivElement>, taskId: number) => {
      event.dataTransfer.setData("text/plain", taskId.toString());
    },
    [],
  );

  const handleDragOver = React.useCallback(
    (event: React.DragEvent<HTMLDivElement>) => {
      event.preventDefault();
    },
    [],
  );

  const handleDrop = React.useCallback(
    async (
      event: React.DragEvent<HTMLDivElement>,
      newStatus: TaskStatusEnum,
    ) => {
      event.preventDefault();
      const taskId = parseInt(event.dataTransfer.getData("text/plain"), 10);

      const task = tasks.find((t) => t.id === taskId) || null;

      if (task && task.status !== newStatus) {
        handleMoveTask(task, newStatus);
      }
    },
    [tasks, handleMoveTask],
  );

  return (
    <Grid
      size={12}
      spacing={1}
      padding={2}
      display="flex"
      flexDirection="row"
      justifyContent="space-between"
      alignItems="center"
      marginTop={4}
    >
      <Grid size={3} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.Created} />
        <TaskDropColumn
          columnId={TaskStatusEnum.Created}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={handleAssignUser}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
          handleDeleteTask={handleDeleteTask}
        />
      </Grid>
      <Grid size={3} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.ReadyToStart} />
        <TaskDropColumn
          columnId={TaskStatusEnum.ReadyToStart}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={handleAssignUser}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
          handleDeleteTask={handleDeleteTask}
        />
      </Grid>
      <Grid size={3} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.InProgress} />
        <TaskDropColumn
          columnId={TaskStatusEnum.InProgress}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={handleAssignUser}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
          handleDeleteTask={handleDeleteTask}
        />
      </Grid>
      <Grid size={3} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.Completed} />
        <TaskDropColumn
          columnId={TaskStatusEnum.Completed}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={() => {}}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
          handleDeleteTask={handleDeleteTask}
        />
      </Grid>
    </Grid>
  );
};

export default TaskTable;
