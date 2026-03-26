import React from "react";
import { Card, CardContent, Grid, IconButton, Tooltip } from "@mui/material";
import { DeleteOutline, MoreVertRounded } from "@mui/icons-material";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import FormDropdown from "../../Components/FormDropdown";
import { useAuth } from "../../Hooks/useAuth";

import FormLabel from "../../Components/FormLabel";
import { useLocalization } from "../../Hooks/useLocalization";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { useNavigate } from "react-router-dom";
import { ITaskItemBase } from "../Interfaces/ITaskItemBase";

interface IProps {
  task: ITaskItemBase;
  userDropdownItems: IDropdownItem[];
  onChange: (updatedTask: ITaskItemBase) => void;
  onDragStart: (event: React.DragEvent<HTMLDivElement>, taskId: number) => void;
  handleDeleteTask: (taskId: number) => Promise<void>;
}

const TaskItem: React.FC<IProps> = (props) => {
  const { task, userDropdownItems, onDragStart, onChange, handleDeleteTask } =
    props;
  const { currentUser } = useAuth();
  const { getResource } = useLocalization();
  const navigate = useNavigate();

  const { canView, canEdit, canDelete } = React.useMemo(() => {
    const userRight = currentUser?.accessRights.find(
      (right) => right.name === "TasksAdministration",
    );

    return {
      canView: userRight?.canView ?? false,
      canEdit: userRight?.canEdit ?? false,
      canDelete: userRight?.canDelete ?? false,
    };
  }, [currentUser]);

  const navigateToDetails = React.useCallback(() => {
    if (canView) {
      navigate(`/task-administration/details/${task.id}`);
    }
  }, [canView, navigate, task.id]);

  return (
    <Card
      draggable
      onDragStart={(event) => onDragStart(event, task.id)}
      sx={{
        width: "auto",
        padding: 2,
        minHeight: 150,

        boxShadow: 4,
        "&:hover": { transform: "scale(1.01)", boxShadow: 6, cursor: "grab" },
      }}
    >
      <CardContent>
        <Grid container spacing={1} direction="column">
          <Grid size={12} display="flex" justifyContent="space-between">
            <Grid size={10}>
              <FormLabel
                bold
                text={`#${task.id} - ${task.title}`}
                variant="subtitle2"
              />
            </Grid>
            <Grid size={1} paddingRight={5}>
              <Tooltip
                title={
                  canView
                    ? getResource("labelViewTask")
                    : getResource("labelMissingPermission")
                }
              >
                <IconButton
                  size="medium"
                  color="primary"
                  disabled={!canView}
                  onClick={navigateToDetails}
                >
                  <MoreVertRounded fontSize="small" />
                </IconButton>
              </Tooltip>
            </Grid>
            <Grid size={1}>
              <Tooltip
                title={
                  canDelete
                    ? getResource("labelDeleteTask")
                    : getResource("labelMissingPermission")
                }
              >
                <IconButton
                  size="medium"
                  color="primary"
                  disabled={!canDelete}
                  onClick={() => handleDeleteTask(task.id)}
                >
                  <DeleteOutline fontSize="small" />
                </IconButton>
              </Tooltip>
            </Grid>
          </Grid>
          <Grid size={12}>
            <FormLabel
              text={task.shortDescription}
              variant="body2"
              color="#cccccc"
            />
          </Grid>
          <Grid size={12}>
            <FormDropdown
              value={
                userDropdownItems.find(
                  (item) => item.id === task.assignedUserId,
                ) || userDropdownItems[0]
              }
              dropdownItems={userDropdownItems}
              disabled={!canEdit || task.status === TaskStatusEnum.Completed}
              onChange={
                canEdit
                  ? (selectedItem) =>
                      onChange({ ...task, assignedUserId: selectedItem.id })
                  : null
              }
            />
          </Grid>
        </Grid>
      </CardContent>
    </Card>
  );
};

export default TaskItem;
