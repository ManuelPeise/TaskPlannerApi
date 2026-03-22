import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Grid,
} from "@mui/material";
import React from "react";
import { useLocalization } from "../Hooks/useLocalization";
import FormButton from "../Components/FormButton";
import useForm from "../Hooks/useForm";
import { IUserData } from "../Lib/Interfaces/IUserData";
import { UserRoleEnum } from "../Lib/Enums/UserRoleEnum";
import FormTextInput from "../Components/FormTextInput";
import { ICheckboxOption } from "../Components/FormCheckboxGroup";
import UserRoles from "./Components/UserRoles";

const initialUserModel: IUserData = {
  id: 0,
  name: "",
  lastName: "",
  emailAddress: "",
  isActive: false,
  userRole: UserRoleEnum.User,
  credentialsId: 0,
  createdBy: "",
  createdAt: new Date(),
  updatedBy: "",
  updatedAt: new Date(),
  accessRights: [],
};

interface IProps {
  open: boolean;
  onClose: () => void;
  handleCreateUser: (data: IUserData) => Promise<void>;
}

const AddUserDialog: React.FC<IProps> = (props) => {
  const { open, onClose, handleCreateUser } = props;
  const { getResource } = useLocalization();

  const userForm = useForm<IUserData>(initialUserModel, handleCreateUser);

  const handleCancel = React.useCallback(() => {
    userForm.resetForm();
    onClose();
  }, [userForm, onClose]);

  const handleSubmit = React.useCallback(() => {
    userForm.handleSubmit();
    onClose();
  }, [userForm, onClose]);

  const userModel = React.useMemo((): IUserData => {
    return userForm.model;
  }, [userForm.model]);

  const userRoleCheckboxOptions = React.useMemo((): ICheckboxOption[] => {
    return [
      {
        label: getResource("labelUser"),
        value: UserRoleEnum.User,
        checked: userModel.userRole === UserRoleEnum.User,
      },
      {
        label: getResource("labelAdmin"),
        value: UserRoleEnum.Admin,
        checked: userModel.userRole === UserRoleEnum.Admin,
      },
    ];
  }, [getResource, userModel.userRole]);

  const handleUserRoleChange = React.useCallback(
    (option: ICheckboxOption) => {
      userForm.handleChange("userRole", option.value);
    },
    [userForm],
  );

  const saveDisabled = React.useMemo(() => {
    return (
      userModel.name.trim() === "" ||
      userModel.lastName.trim() === "" ||
      userModel.emailAddress.trim() === "" ||
      !userModel.emailAddress
        .trim()
        .match(/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/)
    );
  }, [userModel.name, userModel.lastName, userModel.emailAddress]);

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="md">
      <DialogTitle margin={2}>{getResource("labelAddNewUser")}</DialogTitle>
      <DialogContent sx={{ marginTop: 2, padding: 4 }}>
        <Grid container spacing={2} display="flex" flexDirection="row">
          <Grid size={6}>
            <FormTextInput
              type="text"
              value={userModel.name}
              label={getResource("labelFirstName")}
              onChange={(value) => userForm.handleChange("name", value)}
            />
          </Grid>
          <Grid size={6}>
            <FormTextInput
              type="text"
              value={userModel.lastName}
              label={getResource("labelLastName")}
              onChange={(value) => userForm.handleChange("lastName", value)}
            />
          </Grid>
          <Grid size={12}>
            <FormTextInput
              type="text"
              value={userModel.emailAddress}
              label={getResource("labelEmailAddress")}
              onChange={(value) => userForm.handleChange("emailAddress", value)}
            />
          </Grid>

          <UserRoles
            label={getResource("labelUserRole")}
            options={userRoleCheckboxOptions}
            handleOptionChange={handleUserRoleChange}
          />
        </Grid>
      </DialogContent>
      <DialogActions>
        <Grid container spacing={2} size={10} paddingRight={2}>
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
              action={handleSubmit}
              disabled={saveDisabled}
            />
          </Grid>
        </Grid>
      </DialogActions>
    </Dialog>
  );
};

export default AddUserDialog;
