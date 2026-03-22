import React from "react";
import { IUserActivationDialogProps } from "./UserActivationPage";
import { useLocalization } from "../../Hooks/useLocalization";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";
import FormButton from "../../Components/FormButton";

interface IProps extends IUserActivationDialogProps {
  onClose: () => void;
}

const UserActivationDialog: React.FC<IProps> = (props) => {
  const { open, text, onClose } = props;
  const { getResource } = useLocalization();

  return (
    <Dialog open={open} onClose={onClose} sx={{ padding: 4 }} maxWidth="md">
      <DialogTitle>{getResource("labelAccountActivationResult")}</DialogTitle>
      <DialogContent>
        <DialogContentText>{text}</DialogContentText>
      </DialogContent>
      <DialogActions>
        <FormButton label={getResource("labelOK")} action={onClose} />
      </DialogActions>
    </Dialog>
  );
};

export default UserActivationDialog;
