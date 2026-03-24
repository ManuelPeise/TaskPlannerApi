import React from "react";
import { IUserData } from "../Lib/Interfaces/IUserData";
import useStateFulApiService from "../Hooks/useStateFulApiService";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import {
  Grid,
  IconButton,
  List,
  ListItem,
  ListItemIcon,
  Tooltip,
  Typography,
} from "@mui/material";
import {
  AddRounded,
  CancelRounded,
  CheckRounded,
  MoreVertRounded,
} from "@mui/icons-material";
import { UserRoleEnum } from "../Lib/Enums/UserRoleEnum";
import { useLocalization } from "../Hooks/useLocalization";
import AddUserDialog from "./AddUserDialog";
import { IAccessRight } from "../Lib/Interfaces/IAccessRightModel";
import useStatelessApi from "../Hooks/useStatelessApi";
import { useNavigate } from "react-router-dom";

const userDetailsPageUrl = "/user-administration/details/{Id}";

interface IUserAdministrationPageDataModel {
  users: IUserData[];
  accessRights: IAccessRight[];
}

interface IProps {
  users: IUserData[];
  accessRights: IAccessRight[];
  handleCreateUser: (data: IUserData) => Promise<void>;
}

const UserAdministrationPageContainer: React.FC = () => {
  const storage = useLocalStorage<ITokenData>(LocalStorageKeys.Token);
  const userApi = useStateFulApiService<IUserAdministrationPageDataModel>({
    method: "GET",
    requestUrl:
      process.env.REACT_APP_API_URL +
      "useradministration/getuseradministrationpagemodel",
    token: storage.getItem()?.jwt ?? null,
  });

  const statelessUserApi = useStatelessApi();

  const handleCreateUser = React.useCallback(
    async (data: IUserData) => {
      await statelessUserApi
        .sendRequest({
          method: "POST",
          requestUrl:
            process.env.REACT_APP_API_URL + "useradministration/createuser",
          token: storage.getItem()?.jwt ?? null,
          model: data,
        })
        .then(async (response) => {
          userApi.sendRequest({
            method: "GET",
            requestUrl:
              process.env.REACT_APP_API_URL +
              "useradministration/getuseradministrationpagemodel",
            token: storage.getItem()?.jwt ?? null,
          });
        });
    },
    [statelessUserApi, storage, userApi],
  );

  if (userApi.loading || !userApi.response) {
    return null;
  }

  return (
    <UserAdministrationPage
      users={userApi.response.users}
      accessRights={userApi.response.accessRights}
      handleCreateUser={handleCreateUser}
    />
  );
};

const UserAdministrationPage: React.FC<IProps> = (props) => {
  const { users, handleCreateUser } = props;
  const { getResource } = useLocalization();
  const navigate = useNavigate();
  const [addUserDialogOpen, setAddUserDialogOpen] = React.useState(false);

  const getUserRoleLabel = React.useCallback(
    (role: UserRoleEnum): string => {
      switch (role) {
        case UserRoleEnum.Admin:
          return getResource("labelAdmin");
        case UserRoleEnum.User:
          return getResource("labelUser");
        default:
          return "";
      }
    },
    [getResource],
  );

  const handleUserDetailsClick = React.useCallback(
    (user: IUserData) => {
      const url = userDetailsPageUrl.replace("{Id}", user.id.toString());
      navigate(url);
    },
    [navigate],
  );

  return (
    <Grid container direction="column" spacing={2} sx={{ padding: 2 }}>
      <Grid
        size={12}
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        sx={{ marginBottom: 2 }}
      >
        <Grid size={6}>
          <Typography variant="h4" sx={{ fontWeight: "bold" }}>
            {getResource("labelUserAdministration")}
          </Typography>
        </Grid>
        <Grid size={6} display="flex" justifyContent="flex-end">
          <IconButton size="small" onClick={() => setAddUserDialogOpen(true)}>
            <Tooltip title={getResource("labelAddNewUser")}>
              <AddRounded sx={{ fontSize: 30, color: "#555" }} />
            </Tooltip>
          </IconButton>
        </Grid>
      </Grid>
      <Grid size={12}>
        <List disablePadding sx={{ border: "1px solid #ccc", borderRadius: 1 }}>
          {users.map((user) => (
            <ListItem
              key={user.id}
              sx={{
                width: "100%",
                fontWeight: "bold",
                "&:hover": { backgroundColor: "#f5f5f5", cursor: "pointer" },
              }}
            >
              <Grid
                size={12}
                display="flex"
                flexDirection="row"
                alignItems="center"
              >
                <ListItemIcon sx={{ minWidth: "auto", marginRight: 2 }}>
                  {user.isActive ? <CheckRounded /> : <CancelRounded />}
                </ListItemIcon>
                <Grid size={3}>
                  <Typography variant="body1">{user.emailAddress}</Typography>
                </Grid>
                <Grid size={2}>
                  <Typography variant="body1">
                    {user.name} {user.lastName}
                  </Typography>
                </Grid>
                <Grid size={2}>
                  <Typography variant="body1">
                    {getUserRoleLabel(user.userRole)}
                  </Typography>
                </Grid>
                <Grid size={4}>
                  <Typography variant="body1">
                    {getResource("labelLastUpdateByAt")
                      .replace("{User}", user.updatedBy)
                      .replace(
                        "{Date}",
                        new Date(user.updatedAt).toLocaleString(),
                      )}
                  </Typography>
                </Grid>
                <Grid size={2} display="flex" justifyContent="flex-end">
                  <IconButton
                    size="small"
                    onClick={handleUserDetailsClick.bind(null, user)}
                  >
                    <Tooltip title={getResource("labelDetails")}>
                      <MoreVertRounded />
                    </Tooltip>
                  </IconButton>
                </Grid>
              </Grid>
            </ListItem>
          ))}
        </List>
      </Grid>
      <AddUserDialog
        open={addUserDialogOpen}
        onClose={() => setAddUserDialogOpen(false)}
        handleCreateUser={handleCreateUser}
      />
    </Grid>
  );
};

export default UserAdministrationPageContainer;
