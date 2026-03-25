import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
} from "@mui/material";
import React from "react";
import { useLocalization } from "../../Hooks/useLocalization";
import { IAuthenticationRequestModel } from "../../Context/AuthContextProvider";
import FormTextInput from "../../Components/FormTextInput";
import useForm from "../../Hooks/useForm";
import FormButton from "../../Components/FormButton";
import { useAuth } from "../../Hooks/useAuth";
import LoadingIndicator from "../../Components/LoadingIndicator";

interface IProps {
  open: boolean;
  onClose: () => void;
}

export const LoginDialog: React.FC<IProps> = (props) => {
  const { open, onClose } = props;
  const { isLoading, onLogin } = useAuth();
  const { getResource } = useLocalization();

  const submitCallback = React.useCallback(
    async (model: IAuthenticationRequestModel) => {
      await onLogin(model.emailAddress, model.password);
      onClose();
    },
    [onLogin, onClose],
  );

  const { model, handleChange, resetForm, handleSubmit } =
    useForm<IAuthenticationRequestModel>(
      { emailAddress: "", password: "" },
      submitCallback,
    );

  const handleCancel = React.useCallback(() => {
    resetForm();
    onClose();
  }, [onClose, resetForm]);

  const handleCloseDialog = React.useCallback(
    (event: React.SyntheticEvent, reason?: string) => {
      if (reason !== "backdropClick" && reason !== "escapeKeyDown") {
        resetForm();
        onClose();
      }
    },
    [onClose, resetForm],
  );

  return (
    <Dialog
      keepMounted={open}
      open={open}
      onClose={handleCloseDialog}
      maxWidth="sm"
      fullWidth
      sx={{ padding: 4 }}
    >
      <DialogTitle>{getResource("titleLogin")}</DialogTitle>
      <DialogContent sx={{ padding: 4 }}>
        <Grid container spacing={2} direction="column">
          {isLoading && <LoadingIndicator isLoading={isLoading} />}
          <Grid size={12} alignItems="center" justifyContent="center">
            {isLoading && <LoadingIndicator isLoading={isLoading} />}
          </Grid>
          <Grid size={12}>
            <FormTextInput
              type="text"
              label={getResource("labelEmail")}
              value={model.emailAddress}
              onChange={(value) => handleChange("emailAddress", value)}
            />
          </Grid>
          <Grid size={12}>
            <FormTextInput
              type="password"
              label={getResource("labelPassword")}
              value={model.password}
              onChange={(value) => handleChange("password", value)}
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions sx={{ padding: 4 }}>
        <Grid container spacing={2} size={12}>
          <Grid size={6}>
            <FormButton
              label={getResource("labelCancel")}
              action={handleCancel}
            />
          </Grid>
          <Grid size={6}>
            <FormButton
              label={getResource("labelLogin")}
              fullWidth={false}
              disabled={!model.emailAddress || !model.password || isLoading}
              action={handleSubmit}
            />
          </Grid>
        </Grid>
      </DialogActions>
    </Dialog>
  );
};

export default LoginDialog;
