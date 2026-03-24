import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import useStateFulApiService from "../../Hooks/useStateFulApiService";
import useLocalStorage, { LocalStorageKeys } from "../../Hooks/useLocalStorage";
import { ITokenData } from "../../Lib/Interfaces/ITokenData";
import { Grid, Paper, Typography } from "@mui/material";
import { useLocalization } from "../../Hooks/useLocalization";

import useForm from "../../Hooks/useForm";
import FormTextInput from "../../Components/FormTextInput";
import FormTextArea from "../../Components/FormTextArea";
import { ITaskModel } from "../Interfaces/ITaskModel";
import { ITaskDetailsPageModel } from "../Interfaces/ITaskDetailsPageModel";
import FormDropdown from "../../Components/FormDropdown";
import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { useAuth } from "../../Hooks/useAuth";
import FormLabel from "../../Components/FormLabel";
import FormDatePicker from "../../Components/FormDatePicker";
import FormButton from "../../Components/FormButton";
import useStatelessApi from "../../Hooks/useStatelessApi";
import LoadingIndicator from "../../Components/LoadingIndicator";

interface IProps {
  pageModel: ITaskDetailsPageModel;
  isLoading: boolean;
  handleUpdateTask: (updatedTask: ITaskModel) => Promise<void>;
}

const TaskDetailsPageContainer: React.FC = () => {
  const { taskId } = useParams<{ taskId: string }>();
  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);

  const taskApi = useStateFulApiService<ITaskDetailsPageModel>({
    requestUrl:
      process.env.REACT_APP_API_URL +
      `taskadministration/gettaskdetailspagemodel?taskid=${taskId}`,
    method: "GET",
    token: storage.getItem()?.jwt ?? null,
  });

  const taskUpdateApi = useStatelessApi();

  const handleUpdateTask = async (updatedTask: ITaskModel) => {
    await taskUpdateApi
      .sendRequest({
        requestUrl:
          process.env.REACT_APP_API_URL + "taskadministration/updatetask",
        method: "POST",
        model: updatedTask,
        token: storage.getItem()?.jwt ?? null,
      })
      .then(
        async () =>
          await taskApi.sendRequest({
            requestUrl:
              process.env.REACT_APP_API_URL +
              `taskadministration/gettaskdetailspagemodel?taskid=${taskId}`,
            method: "GET",
            token: storage.getItem()?.jwt ?? null,
          }),
      );
  };

  if (!taskApi.response) {
    return null;
  }

  return (
    <TaskDetailsPage
      isLoading={taskApi.loading}
      pageModel={taskApi.response}
      handleUpdateTask={handleUpdateTask}
    />
  );
};

const TaskDetailsPage: React.FC<IProps> = (props) => {
  const { isLoading, pageModel, handleUpdateTask } = props;
  const { currentUser } = useAuth();
  const { getResource } = useLocalization();
  const navigate = useNavigate();

  const { model, isModified, handleChange, resetForm } = useForm<ITaskModel>(
    pageModel.task,
  );

  const canEdit = React.useMemo(() => {
    if (!currentUser) return false;
    return (
      currentUser.accessRights.find((x) => x.name === "TasksAdministration")
        ?.canEdit ?? false
    );
  }, [currentUser]);

  const userDropdownItems = React.useMemo(() => {
    return pageModel.userDropdownItems.map((item) => {
      return item.id === 0 ? { ...item, label: getResource(item.label) } : item;
    });
  }, [pageModel.userDropdownItems, getResource]);

  const priorityDropdownItems = React.useMemo((): IDropdownItem[] => {
    return pageModel.priorityDropdownItems.map((item) => {
      return { ...item, id: item.id, label: getResource(item.label) };
    });
  }, [pageModel.priorityDropdownItems, getResource]);

  const statusDropdownItems = React.useMemo((): IDropdownItem[] => {
    return pageModel.statusDropdownItems.map((item) => {
      return { ...item, id: item.id, label: getResource(`${item.label}`) };
    });
  }, [pageModel.statusDropdownItems, getResource]);

  const handleSave = React.useCallback(
    async (model: ITaskModel, close: boolean) => {
      if (model) {
        handleUpdateTask(model).then(() => {
          if (close) {
            navigate(-1);
          }
        });
      }
    },
    [handleUpdateTask, navigate],
  );

  return (
    <Grid container direction="column" spacing={2} height="100%">
      <Grid
        size={12}
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        height="7%"
      >
        <Grid size={6} padding={2}>
          <Typography variant="h4" sx={{ fontWeight: "bold" }}>
            {`#${model.taskId}`}
          </Typography>
        </Grid>
        <Grid
          size={6}
          display="flex"
          justifyContent="flex-end"
          gap={2}
          padding={2}
        >
          <FormButton
            size="small"
            variant="outlined"
            label={getResource("labelCancel")}
            disabled={!isModified}
            action={resetForm}
          />
          <FormButton
            size="small"
            label={getResource("labelSave")}
            variant="outlined"
            disabled={!isModified}
            action={handleSave.bind(null, model, false)}
          />
          <FormButton
            size="small"
            label={getResource("labelSaveAndClose")}
            variant="outlined"
            disabled={!isModified}
            action={handleSave.bind(null, model, true)}
          />
        </Grid>
      </Grid>
      <Grid
        size={12}
        display="flex"
        justifyContent="space-between"
        alignItems="start"
        spacing={2}
      >
        <Grid
          size={8}
          display="flex"
          justifyContent="space-between"
          alignItems="start"
          height="100%"
        >
          <Paper elevation={4} sx={{ height: "100%", width: "100%" }}>
            <Grid
              size={12}
              padding={2}
              display="flex"
              flexDirection="column"
              gap={2}
            >
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelTitle")}
                  variant="body1"
                  bold
                />
                <FormTextInput
                  label=""
                  type="text"
                  fullWidth
                  disabled={!canEdit}
                  value={model.title}
                  onChange={(value) => handleChange("title", value)}
                />
              </Grid>
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelShortDescription")}
                  variant="body1"
                  bold
                />
                <FormTextInput
                  label=""
                  type="text"
                  fullWidth
                  disabled={!canEdit}
                  value={model.shortDescription}
                  onChange={(value) => handleChange("shortDescription", value)}
                />
              </Grid>
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelDescription")}
                  variant="body1"
                  bold
                />
                <FormTextArea
                  label=""
                  height={100}
                  disabled={!canEdit}
                  value={model.description ?? ""}
                  onChange={(value) => handleChange("description", value)}
                />
              </Grid>
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelAcceptanceCriteria")}
                  variant="body1"
                  bold
                />
                <FormTextArea
                  label=""
                  height={200}
                  disabled={!canEdit}
                  value={model.acceptanceCriteria ?? ""}
                  onChange={(value) =>
                    handleChange("acceptanceCriteria", value)
                  }
                />
              </Grid>
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelLastUpdateByAt")
                    .replace("{User}", model.updatedBy ?? "")
                    .replace(
                      "{Date}",
                      new Date(model.updatedAt)?.toLocaleString(),
                    )}
                  variant="body1"
                />
              </Grid>
            </Grid>
          </Paper>
        </Grid>
        <Grid
          size={4}
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          height="100%"
        >
          <Paper elevation={4} sx={{ height: "100%", width: "100%" }}>
            <Grid
              size={12}
              padding={2}
              display="flex"
              flexDirection="column"
              gap={2}
            >
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelDeadlineDate")}
                  variant="body1"
                  bold
                />
                <FormDatePicker
                  label=""
                  fullWidth
                  minDate={new Date()}
                  dateValue={model.deadLineDate ?? null}
                  onChange={(date) => handleChange("deadLineDate", date)}
                  disabled={!canEdit}
                />
              </Grid>
              <Grid size={12} padding={2} display="flex" flexDirection="column">
                <FormLabel
                  text={getResource("labelAssignedTo")}
                  variant="body1"
                  bold
                />
                <FormDropdown
                  dropdownItems={userDropdownItems}
                  value={
                    userDropdownItems.find(
                      (pm) => pm.id === (model.assignedUserId ?? 0),
                    ) || null
                  }
                  onChange={(value) => handleChange("assignedUserId", value.id)}
                  disabled={!canEdit}
                />
              </Grid>
              <Grid
                size={12}
                padding={2}
                display="flex"
                flexDirection="column"
                gap={2}
              >
                <FormLabel
                  text={getResource("labelStatus")}
                  variant="body1"
                  bold
                />
                <FormDropdown
                  dropdownItems={statusDropdownItems}
                  value={
                    statusDropdownItems.find(
                      (pm) => pm.id === (model.status ?? 0),
                    ) || null
                  }
                  onChange={(value) => handleChange("status", value.id)}
                  disabled={!canEdit}
                />
              </Grid>
              <Grid
                size={12}
                padding={2}
                display="flex"
                flexDirection="column"
                gap={2}
              >
                <FormLabel
                  text={getResource("labelPriority")}
                  variant="body1"
                  bold
                />
                <FormDropdown
                  dropdownItems={priorityDropdownItems}
                  value={
                    priorityDropdownItems.find(
                      (pm) => pm.id === (model.priority ?? 0),
                    ) || null
                  }
                  onChange={(value) => handleChange("priority", value.id)}
                  disabled={!canEdit}
                />
              </Grid>
            </Grid>
          </Paper>
        </Grid>
      </Grid>
      <LoadingIndicator isLoading={isLoading} />
    </Grid>
  );
};

export default TaskDetailsPageContainer;
