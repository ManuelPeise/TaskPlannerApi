import React from "react";
import { ITaskModel } from "../Interfaces/ITaskModel";
import { Grid } from "@mui/material";

interface IProps {
  tasks: ITaskModel[];
}

const TaskTable: React.FC<IProps> = (props) => {
  return (
    <Grid size={12} spacing={2} padding={2}>
      Task Table
    </Grid>
  );
};

export default TaskTable;
