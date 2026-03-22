import { Grid, IconButton, Tooltip } from "@mui/material";
import React from "react";
import { ITaskFilterOptions } from "../Interfaces/ITaskFilterOptions";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { useLocalization } from "../../Hooks/useLocalization";
import FormDropdown from "../../Components/FormDropdown";
import { AddRounded } from "@mui/icons-material";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";

interface IProps {
  filterOptions?: ITaskFilterOptions;
  gitRepositoryDropdownItems: IDropdownItem[];
  userDropdownItems: IDropdownItem[];
  handleFilterChange?: (options: Partial<ITaskFilterOptions>) => void;
}

const TaskToolbar: React.FC<IProps> = (props) => {
  const { filterOptions, userDropdownItems, handleFilterChange } = props;
  const { getResource } = useLocalization();

  const selectedUserItem = React.useMemo((): IDropdownItem | null => {
    return (
      userDropdownItems.find(
        (item) => item.id === filterOptions?.assignedUserId,
      ) || null
    );
  }, [filterOptions?.assignedUserId, userDropdownItems]);

  const taskTypeDropdownItems = React.useMemo((): IDropdownItem[] => {
    return [
      { id: TaskTypeEnum.All, label: getResource("labelTaskTypeAll") },
      {
        id: TaskTypeEnum.BacklogItem,
        label: getResource("labelTaskTypeBacklogItem"),
      },
      { id: TaskTypeEnum.Health, label: getResource("labelTaskTypeHealth") },
      { id: TaskTypeEnum.Hobby, label: getResource("labelTaskTypeHobby") },
      { id: TaskTypeEnum.Work, label: getResource("labelTaskTypeWork") },
    ];
  }, [getResource]);

  const selectedTaskTypeItem = React.useMemo((): IDropdownItem | null => {
    return (
      taskTypeDropdownItems.find(
        (item) => item.id === filterOptions?.taskTypeId,
      ) || null
    );
  }, [filterOptions?.taskTypeId, taskTypeDropdownItems]);

  return (
    <Grid
      size={12}
      container
      spacing={2}
      padding={2}
      display="flex"
      flexDirection="row"
      alignItems="flex-end"
      justifyContent="space-between"
      paddingLeft={6}
      sx={{ borderBottom: "1px solid", borderColor: "divider" }}
    >
      <Grid size={4} display="flex" justifyContent="center">
        <FormDropdown
          value={selectedUserItem}
          dropdownItems={userDropdownItems}
          onChange={(value) =>
            handleFilterChange?.({ assignedUserId: value.id })
          }
        />
      </Grid>
      <Grid size={4} display="flex" justifyContent="center">
        <FormDropdown
          value={selectedTaskTypeItem}
          dropdownItems={taskTypeDropdownItems}
          onChange={(value) => handleFilterChange?.({ taskTypeId: value.id })}
        />
      </Grid>
      <Grid size={2} display="flex" justifyContent="flex-end">
        <Tooltip title={getResource("labelAddTask")}>
          <IconButton>
            <AddRounded />
          </IconButton>
        </Tooltip>
      </Grid>
    </Grid>
  );
};

export default TaskToolbar;
