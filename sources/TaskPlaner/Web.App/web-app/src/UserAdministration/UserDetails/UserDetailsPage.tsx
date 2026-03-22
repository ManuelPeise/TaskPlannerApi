import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import useStateFulApiService from "../../Hooks/useStateFulApiService";
import { IUserData } from "../../Lib/Interfaces/IUserData";
import useLocalStorage, { LocalStorageKeys } from "../../Hooks/useLocalStorage";
import { ITokenData } from "../../Lib/Interfaces/ITokenData";
import { Grid, List, ListItem, Typography } from "@mui/material";
import { useLocalization } from "../../Hooks/useLocalization";
import FormLabel from "../../Components/FormLabel";
import FormCheckbox from "../../Components/FormCheckbox";
import useForm from "../../Hooks/useForm";
import AccessRightForm from "../Components/AccessRightForm";
import { IAccessRight } from "../../Lib/Interfaces/IAccessRightModel";
import UserRoles from "../Components/UserRoles";
import { UserRoleEnum } from "../../Lib/Enums/UserRoleEnum";
import { ICheckboxOption } from "../../Components/FormCheckboxGroup";
import FormButton from "../../Components/FormButton";
import LoadingIndicator from "../../Components/LoadingIndicator";

interface IProps {
  user: IUserData;
  handleUpdateUser: (data: IUserData) => Promise<void>;
}

const UserDetailsPageContainer: React.FC = () => {
  const { userId } = useParams<{ userId: string }>();
  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);
  const userIdNumber = userId ? parseInt(userId, 10) : undefined;

  const userApi = useStateFulApiService<IUserData>({
    method: "GET",
    requestUrl:
      process.env.REACT_APP_API_URL +
      `useradministration/getuserbyid?userId=${userIdNumber}&includeCredentials=false&includeUserRights=true`,
    token: storage.getItem()?.jwt ?? null,
  });

  const handleUpdateUser = React.useCallback(
    async (data: IUserData) => {
      await userApi
        .sendRequest({
          method: "POST",
          requestUrl:
            process.env.REACT_APP_API_URL +
            `useradministration/updateuser?updateCredentials=true`,
          token: storage.getItem()?.jwt ?? null,
          model: data,
        })
        .then(async () => {
          await userApi.sendRequest({
            method: "GET",
            requestUrl:
              process.env.REACT_APP_API_URL +
              `useradministration/getuserbyid?userId=${userIdNumber}&includeCredentials=false&includeUserRights=true`,
            token: storage.getItem()?.jwt ?? null,
          });
        });
    },
    [userApi, userIdNumber, storage],
  );

  if (userApi.loading || !userApi.response) {
    return <LoadingIndicator isLoading={true} />;
  }

  return (
    <UserAdministrationDetailsPage
      user={userApi.response}
      handleUpdateUser={handleUpdateUser}
    />
  );
};

const UserAdministrationDetailsPage: React.FC<IProps> = (props) => {
  const { user, handleUpdateUser } = props;
  const { getResource } = useLocalization();
  const navigate = useNavigate();

  const { model, isModified, handleChange, resetForm } = useForm<IUserData>(
    user,
    async (data) => {},
  );

  const userRoleCheckboxOptions = React.useMemo((): ICheckboxOption[] => {
    return [
      {
        label: getResource("labelUser"),
        value: UserRoleEnum.User,
        checked: model.userRole === UserRoleEnum.User,
      },
      {
        label: getResource("labelAdmin"),
        value: UserRoleEnum.Admin,
        checked: model.userRole === UserRoleEnum.Admin,
      },
    ];
  }, [getResource, model.userRole]);

  const handleAccessRightChange = React.useCallback(
    (accessRight: IAccessRight) => {
      const updatedAccessRights = model.accessRights.map((ar) => {
        if (ar.id === accessRight.id) {
          ar = { ...ar, ...accessRight };
        }

        return ar;
      });
      handleChange("accessRights", updatedAccessRights);
    },
    [model.accessRights, handleChange],
  );

  return (
    <Grid direction="column" spacing={2} sx={{ padding: 2 }}>
      <Grid size={12}>
        <Typography variant="h4">{getResource("labelUserDetails")}</Typography>
      </Grid>
      <Grid size={12} marginTop={2}>
        <List>
          <ListItem
            key="firstName"
            divider
            sx={{ padding: 2 }}
            secondaryAction={<FormLabel text={model.name} variant="body2" />}
          >
            <FormLabel text={getResource("labelFirstName")} variant="body1" />
          </ListItem>
          <ListItem
            key="lastName"
            divider
            sx={{ padding: 2 }}
            secondaryAction={
              <FormLabel text={model.lastName} variant="body2" />
            }
          >
            <FormLabel text={getResource("labelLastName")} variant="body1" />
          </ListItem>
          <ListItem
            key="emailAddress"
            divider
            sx={{ padding: 2 }}
            secondaryAction={
              <FormLabel text={model.emailAddress} variant="body2" />
            }
          >
            <FormLabel
              text={getResource("labelEmailAddress")}
              variant="body1"
            />
          </ListItem>
          <ListItem
            key="userRole"
            divider
            sx={{ padding: 2 }}
            secondaryAction={
              <UserRoles
                options={userRoleCheckboxOptions}
                handleOptionChange={(e) => handleChange("userRole", e.value)}
              />
            }
          >
            <FormLabel text={getResource("labelUserRole")} variant="body1" />
          </ListItem>
          <ListItem
            key="isActive"
            divider
            sx={{ padding: 2 }}
            secondaryAction={
              <FormCheckbox
                checked={model.isActive}
                onChange={(e) => handleChange("isActive", e)}
              />
            }
          >
            <FormLabel text={getResource("labelIsActive")} variant="body1" />
          </ListItem>
        </List>
      </Grid>
      <Grid size={12}>
        <Grid size={12} marginBottom={2} marginTop={3}>
          <Typography variant="h5">
            {getResource("labelAccessRights")}
          </Typography>
        </Grid>
        <AccessRightForm
          maxHeight={300}
          accessRights={model.accessRights}
          handleAccessRightChange={handleAccessRightChange}
        />
      </Grid>
      <Grid
        container
        spacing={2}
        size={12}
        paddingRight={2}
        flexDirection="row"
        display="flex"
        justifyContent="space-between"
        marginTop={4}
      >
        <Grid size={6} display="flex" justifyContent="flex-start">
          <Grid size={3}>
            <FormButton
              label={getResource("labelBackToList")}
              action={() => navigate("/user-administration")}
              disabled={isModified}
            />
          </Grid>
        </Grid>
        <Grid container size={6} display="flex" justifyContent="flex-end">
          <Grid size={2}>
            <FormButton
              disabled={!isModified}
              label={getResource("labelCancel")}
              action={resetForm}
            />
          </Grid>
          <Grid size={2}>
            <FormButton
              label={getResource("labelSave")}
              fullWidth={false}
              action={handleUpdateUser.bind(null, model)}
              disabled={!isModified}
            />
          </Grid>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default UserDetailsPageContainer;
