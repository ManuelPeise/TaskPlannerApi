import React from "react";
import { Grid } from "@mui/material";
import TaskTableColumnHeader from "./TaskTableColumnHeader";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import TaskDropColumn from "./TaskDropColumn";
import { ITaskItemProps } from "../Interfaces/ITaskItemProps";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";

interface IProps {
  tasks: ITaskItemProps[];
  userDropdownItems: IDropdownItem[];
  handleTaskChanged: (
    updatedTask: ITaskItemProps,
    status: TaskStatusEnum,
  ) => void;
  handleAssignUser: (task: ITaskItemProps) => void;
}

const TaskTable: React.FC<IProps> = (props) => {
  const { tasks, userDropdownItems, handleTaskChanged, handleAssignUser } =
    props;

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
        handleTaskChanged(task, newStatus);
      }
    },
    [tasks, handleTaskChanged],
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
      <Grid size={4} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.Created} />
        <TaskDropColumn
          columnId={TaskStatusEnum.Created}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={handleAssignUser}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
        />
      </Grid>
      <Grid size={4} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.InProgress} />
        <TaskDropColumn
          columnId={TaskStatusEnum.InProgress}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={() => {}}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
        />
      </Grid>
      <Grid size={4} spacing={1} bgcolor="#f2f2f2">
        <TaskTableColumnHeader status={TaskStatusEnum.Done} />
        <TaskDropColumn
          columnId={TaskStatusEnum.Done}
          tasks={tasks}
          userDropdownItems={userDropdownItems}
          onItemChanged={() => {}}
          onDragStart={handleDragStart}
          onDrop={handleDrop}
          onDragOver={handleDragOver}
        />
      </Grid>
    </Grid>
  );
};

export default TaskTable;
