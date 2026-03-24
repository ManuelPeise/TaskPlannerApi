import React from "react";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { Grid, Typography } from "@mui/material";
import { useLocalization } from "../../Hooks/useLocalization";

interface IProps {
  status: TaskStatusEnum;
}

const TaskTableColumnHeader: React.FC<IProps> = (props) => {
  const { status } = props;
  const { getResource } = useLocalization();

  const label = React.useMemo((): string => {
    switch (status) {
      case TaskStatusEnum.Created:
        return getResource("labelCreated");
      case TaskStatusEnum.ReadyToStart:
        return getResource("labelReadyToStart");
      case TaskStatusEnum.InProgress:
        return getResource("labelInProgress");
      case TaskStatusEnum.Completed:
        return getResource("labelCompleted");
    }
  }, [status, getResource]);

  return (
    <Grid size={12} bgcolor="#f2f2f2">
      <Grid
        size={12}
        display="flex"
        justifyContent="center"
        alignItems="baseline"
      >
        <Typography paddingTop={1} variant="body1">
          {label}
        </Typography>
      </Grid>
    </Grid>
  );
};

export default TaskTableColumnHeader;
