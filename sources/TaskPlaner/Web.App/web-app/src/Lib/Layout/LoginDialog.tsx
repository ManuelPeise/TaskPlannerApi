import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
} from "@mui/material";
import React, { use } from "react";
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
  const { onLogin } = useAuth();
  const { getResource } = useLocalization();

  const [isLoading, setIsLoading] = React.useState(false);

  const submitCallback = React.useCallback(
    async (model: IAuthenticationRequestModel) => {
      setIsLoading(true);
      await onLogin(model.emailAddress, model.password);
      onClose();
      setIsLoading(false);
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

  return (
    <Dialog
      keepMounted
      open={open}
      onClose={onClose}
      maxWidth="sm"
      fullWidth
      sx={{ padding: 4 }}
    >
      <DialogTitle>{getResource("titleLogin")}</DialogTitle>
      <DialogContent sx={{ padding: 4 }}>
        <Grid container spacing={2} direction="column">
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
