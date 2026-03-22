import { Divider, Grid, List, ListItemButton } from "@mui/material";
import React, { PropsWithChildren } from "react";
import AppHeaderBar from "./AppHeaderBar";
import LoginDialog from "./LoginDialog";
import { useAuth } from "../../Hooks/useAuth";
import { useNavigate } from "react-router-dom";

interface INavigationItem {
  label: string;
  route: string;
  isDisabled: boolean;
}

const PageLayout: React.FC<PropsWithChildren> = (props) => {
  const [loginDialogOpen, setLoginDialogOpen] = React.useState(false);
  const { currentUser, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const navigationItems: INavigationItem[] = [];

  if (
    currentUser?.accessRights.find((ar) => ar.name === "UserAdministration")
      ?.canView
  ) {
    navigationItems.push({
      label: "User Administration",
      route: "/user-administration",
      isDisabled: false,
    });
  }

  React.useEffect(() => {
    if (!isAuthenticated) {
      setLoginDialogOpen(true);
    }
  }, [isAuthenticated]);

  return (
    <Grid container width={"100%"}>
      <Grid size={12}>
        <AppHeaderBar handleOpenLoginDialog={() => setLoginDialogOpen(true)} />
      </Grid>
      <Grid size={12} display="flex">
        <Grid size={2} sx={{ backgroundColor: "#000000", height: "93vh" }}>
          <Grid size={12}>
            <Divider sx={{ backgroundColor: "#ffffff", height: 1 }} />
            <List disablePadding>
              {navigationItems.map((item) => (
                <Grid size={12} key={item.route}>
                  <ListItemButton
                    selected={window.location.pathname === item.route}
                    sx={{
                      color: "#ffffff",
                      padding: 2,
                      "&.Mui-selected": { backgroundColor: "#474343" },
                      "&:hover": { color: "#e9e0e000" },
                    }}
                    disabled={item.isDisabled}
                    onClick={() => navigate(item.route)}
                  >
                    {item.label}
                  </ListItemButton>
                  <Divider sx={{ backgroundColor: "#ffffff", height: 1 }} />
                </Grid>
              ))}
            </List>
          </Grid>
        </Grid>
        <Grid size={10} sx={{ padding: 2 }}>
          {props.children}
        </Grid>
      </Grid>
      <LoginDialog
        open={loginDialogOpen}
        onClose={() => setLoginDialogOpen(false)}
      />
    </Grid>
  );
};

export default PageLayout;
