import { Grid, Typography } from "@mui/material";
import React from "react";
import { NavigateFunction, useNavigate, useParams } from "react-router-dom";
import { useLocalization } from "../../Hooks/useLocalization";
import useForm from "../../Hooks/useForm";
import FormTextInput from "../../Components/FormTextInput";
import FormButton from "../../Components/FormButton";
import FormLabel from "../../Components/FormLabel";
import useStatelessApi from "../../Hooks/useStatelessApi";
import LoadingIndicator from "../../Components/LoadingIndicator";
import UserActivationDialog from "./UserActivationDialog";

export interface IUserActivationDialogProps {
  open: boolean;
  text: string;
}

interface IUserActivationModel {
  userId: number;
  emailAddress: string;
  password: string;
  confirmPassword: string;
}

interface IProps {
  userId: number;
  isLoading: boolean;
  dialogProps: IUserActivationDialogProps;
  handleCloseDialog: () => void;
  navigate: NavigateFunction;
  getResource: (key: string) => string;
  handleActivateAccount: (data: IUserActivationModel) => Promise<void>;
}

const UserActivationPageContainer: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const userIdNumber = userId ? parseInt(userId, 10) : undefined;

  const [dialogProps, setDialogProps] =
    React.useState<IUserActivationDialogProps>({
      open: false,
      text: "",
    });
  const { getResource } = useLocalization();
  const navigate = useNavigate();
  const activationApi = useStatelessApi();

  const handleCloseDialog = React.useCallback(() => {
    setDialogProps((prev) => ({ ...prev, open: false }));
    navigate("/");
  }, [navigate]);

  const handleActivateAccount = React.useCallback(
    async (data: IUserActivationModel) => {
      await activationApi
        .sendRequest<Boolean>({
          method: "POST",
          requestUrl:
            process.env.REACT_APP_API_URL + "authentication/activateaccount",
          model: data,
        })
        .then((res) => {
          setDialogProps({
            open: true,
            text: res
              ? getResource("labelAccountActivated")
              : getResource("labelAccountActivationFailed").replace(
                  "{Email}",
                  process.env.REACT_APP_CONTACT_MAIL_ADDRESS || "",
                ),
          });
        });
    },
    [activationApi, getResource],
  );

  if (!userIdNumber) {
    return <div>Invalid user ID</div>;
  }

  return (
    <UserActivationPage
      userId={userIdNumber}
      isLoading={activationApi.loading}
      dialogProps={dialogProps}
      handleCloseDialog={handleCloseDialog}
      navigate={navigate}
      getResource={getResource}
      handleActivateAccount={handleActivateAccount}
    />
  );
};

const UserActivationPage: React.FC<IProps> = (props) => {
  const {
    userId,
    isLoading,
    dialogProps,
    handleCloseDialog,
    navigate,
    handleActivateAccount,
    getResource,
  } = props;

  const { model, isModified, resetForm, handleChange } =
    useForm<IUserActivationModel>({
      userId,
      emailAddress: "",
      password: "",
      confirmPassword: "",
    });

  const handleCancel = React.useCallback(() => {
    navigate("/");
    resetForm();
  }, [navigate, resetForm]);

  const saveDisabled = React.useMemo(() => {
    return (
      !isModified ||
      model.password === "" ||
      model.password.length < 8 ||
      model.password !== model.confirmPassword ||
      model.emailAddress === "" ||
      !model.emailAddress
        .trim()
        .match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)
    );
  }, [isModified, model]);

  return (
    <Grid container spacing={2} padding={2} height="90vh">
      <Grid size={12}>
        <Typography variant="h4">
          {getResource("labelAccountActivation")}
        </Typography>
        <FormLabel
          text={getResource("descriptionActivateAccount")}
          variant="subtitle1"
          color="gray"
        />
      </Grid>
      <Grid container size={12} display="flex" justifyContent="center">
        <Grid
          size={4}
          display="flex"
          flexDirection="column"
          alignItems="center"
          sx={{
            padding: 4,
            height: "auto",
            border: "1px solid #cccccc",
            borderRadius: 2,
            maxHeight: "35vh",
          }}
        >
          <Grid size={12} p={2}>
            <FormTextInput
              type="text"
              label={getResource("labelEmailAddress")}
              value={model.emailAddress}
              onChange={(value) => handleChange("emailAddress", value)}
            />
          </Grid>
          <Grid size={12} p={2}>
            <FormTextInput
              type="password"
              label={getResource("labelPassword")}
              value={model.password}
              onChange={(value) => handleChange("password", value)}
            />
          </Grid>
          <Grid size={12} p={2}>
            <FormTextInput
              type="password"
              label={getResource("labelConfirmPassword")}
              value={model.confirmPassword}
              onChange={(value) => handleChange("confirmPassword", value)}
            />
          </Grid>
          <Grid
            size={12}
            display="flex"
            justifyContent="flex-end"
            alignItems="center"
            gap={2}
            marginTop={2}
            paddingRight={2}
          >
            <FormButton
              label={getResource("labelCancel")}
              fullWidth={false}
              action={handleCancel}
            />
            <FormButton
              label={getResource("labelActivateAccount")}
              fullWidth={false}
              action={() => handleActivateAccount(model)}
              disabled={saveDisabled}
            />
          </Grid>
        </Grid>
      </Grid>
      <LoadingIndicator isLoading={isLoading} />
      <UserActivationDialog
        open={dialogProps.open}
        text={dialogProps.text}
        onClose={handleCloseDialog}
      />
    </Grid>
  );
};

export default UserActivationPageContainer;
