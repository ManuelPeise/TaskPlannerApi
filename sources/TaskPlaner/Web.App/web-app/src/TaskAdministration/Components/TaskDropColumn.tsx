import { Grid } from "@mui/material";
import React from "react";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { ITaskItemProps } from "../Interfaces/ITaskItemProps";
import TaskItem from "./TaskItem";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";

interface IProps {
  columnId: TaskStatusEnum;
  tasks: ITaskItemProps[];
  userDropdownItems: IDropdownItem[];
  onItemChanged: (task: ITaskItemProps) => void;
  onDragStart: (event: React.DragEvent<HTMLDivElement>, taskId: number) => void;
  onDrop: (
    event: React.DragEvent<HTMLDivElement>,
    newStatus: TaskStatusEnum,
  ) => void;
  onDragOver: (event: React.DragEvent<HTMLDivElement>) => void;
}

const TaskDropColumn: React.FC<IProps> = (props) => {
  const {
    columnId,
    tasks,
    userDropdownItems,
    onItemChanged,
    onDrop,
    onDragOver,
    onDragStart,
  } = props;

  return (
    <Grid
      key={columnId}
      display="flex"
      flexDirection="column"
      gap={1}
      spacing={1}
      padding={1}
      minHeight={700}
      maxHeight={700}
      onDrop={(event) => onDrop(event, columnId)}
      onDragOver={onDragOver}
      sx={{
        overflowY: "scroll",
        mozScollbarWidth: "none",
        msOverflowStyle: "none",
        "&::-webkit-scrollbar": { display: "none" },
      }}
    >
      {tasks
        .filter((task) => task.status === columnId)
        .map((task) => (
          <TaskItem
            key={task.id}
            task={task}
            userDropdownItems={userDropdownItems}
            onDragStart={onDragStart}
            onChange={onItemChanged}
          />
        ))}
    </Grid>
  );
};

export default React.memo(TaskDropColumn);
