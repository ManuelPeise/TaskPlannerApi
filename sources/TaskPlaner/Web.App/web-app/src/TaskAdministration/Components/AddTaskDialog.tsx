import React from "react";
import { ITaskItemBase } from "../Interfaces/ITaskItemBase";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
} from "@mui/material";
import useForm from "../../Hooks/useForm";
import { useLocalization } from "../../Hooks/useLocalization";
import FormButton from "../../Components/FormButton";
import FormTextInput from "../../Components/FormTextInput";
import FormDropdown from "../../Components/FormDropdown";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";

interface IProps {
  open: boolean;
  isLoading: boolean;
  onClose: () => void;
  handleSaveTask: (task: ITaskItemBase) => Promise<void>;
}

export const AddTaskDialog: React.FC<IProps> = (props) => {
  const { open, isLoading, onClose, handleSaveTask } = props;
  const { getResource } = useLocalization();

  const { model, resetForm, handleChange } = useForm<ITaskItemBase>({
    id: 0,
    title: "",
    shortDescription: "",
    status: 0,
    assignedUserId: 0,
    taskTypeId: 0,
  });

  const handleCloseDialog = React.useCallback(
    (event: React.SyntheticEvent, reason?: string) => {
      if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
        resetForm();
        onClose();
      }
    },
    [onClose, resetForm],
  );

  const handleCancel = React.useCallback(() => {
    resetForm();
    onClose();
  }, [onClose, resetForm]);

  const onSaveTask = React.useCallback(async () => {
    await handleSaveTask(model);
    resetForm();
    onClose();
  }, [handleSaveTask, model, onClose, resetForm]);

  const taskTypeDropdownItems = React.useMemo(() => {
    return [
      { id: TaskTypeEnum.All, label: getResource("labelSelectTaskType") },
      {
        id: TaskTypeEnum.BacklogItem,
        label: getResource("labelTaskTypeBacklogItem"),
      },
      { id: TaskTypeEnum.Health, label: getResource("labelTaskTypeHealth") },
      { id: TaskTypeEnum.Hobby, label: getResource("labelTaskTypeHobby") },
      { id: TaskTypeEnum.Work, label: getResource("labelTaskTypeWork") },
    ];
  }, [getResource]);

  const saveDisabled = React.useMemo(() => {
    return (
      !model.title ||
      !model.shortDescription ||
      model.taskTypeId === TaskTypeEnum.All ||
      isLoading
    );
  }, [model.shortDescription, model.title, model.taskTypeId, isLoading]);

  return (
    <Dialog open={open} onClose={handleCloseDialog} maxWidth="sm" fullWidth>
      <DialogTitle>{getResource("titleAddTask")}</DialogTitle>
      <DialogContent>
        <Grid size={12} container spacing={2} paddingTop={3}>
          <Grid size={12}>
            <FormDropdown
              value={
                taskTypeDropdownItems.find(
                  (item) => item.id === model.taskTypeId,
                ) || null
              }
              dropdownItems={taskTypeDropdownItems}
              onChange={(value) => handleChange("taskTypeId", value.id)}
              disabled={isLoading}
            />
          </Grid>
          <Grid size={12}>
            <FormTextInput
              type="text"
              value={model.title}
              label={getResource("labelTitle")}
              onChange={(value) => handleChange("title", value)}
              disabled={isLoading}
            />
          </Grid>
          <Grid size={12}>
            <FormTextInput
              type="text"
              value={model.shortDescription}
              label={getResource("labelShortDescription")}
              onChange={(value) => handleChange("shortDescription", value)}
              disabled={isLoading}
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Grid container spacing={2} size={12} padding={2}>
          <Grid size={6}>
            <FormButton
              label={getResource("labelCancel")}
              action={handleCancel}
            />
          </Grid>
          <Grid size={6}>
            <FormButton
              label={getResource("labelSave")}
              fullWidth={false}
              disabled={saveDisabled}
              action={onSaveTask}
            />
          </Grid>
        </Grid>
      </DialogActions>
    </Dialog>
  );
};

export default AddTaskDialog;
