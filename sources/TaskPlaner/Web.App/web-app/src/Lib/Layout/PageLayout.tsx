import { Divider, Grid, List, ListItemButton } from "@mui/material";
import React, { PropsWithChildren } from "react";
import AppHeaderBar from "./AppHeaderBar";
import LoginDialog from "./LoginDialog";
import { useAuth } from "../../Hooks/useAuth";
import { useNavigate, useLocation } from "react-router-dom";

interface INavigationItem {
  label: string;
  route: string;
  isDisabled: boolean;
}

interface IProps extends PropsWithChildren {}
const PageLayout: React.FC<IProps> = (props) => {
  const [loginDialogOpen, setLoginDialogOpen] = React.useState(false);
  const { currentUser, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const isPrivate = React.useMemo(() => {
    return !/^\/account\/activate\/.+/.test(location.pathname);
  }, [location.pathname]);

  const navigationItems = React.useMemo((): INavigationItem[] => {
    const items: INavigationItem[] = [];

    items.push({
      label: "User Administration",
      route: "/user-administration",
      isDisabled:
        !isAuthenticated ||
        !currentUser?.accessRights.find(
          (ar) => ar.name === "UserAdministration",
        )?.canView,
    });
    return items;
  }, [isAuthenticated, currentUser]);

  React.useEffect(() => {
    if (isPrivate && !isAuthenticated) {
      setLoginDialogOpen(true);
    }
  }, [isPrivate, isAuthenticated]);

  return (
    <Grid container width={"100%"}>
      <Grid size={12}>
        <AppHeaderBar
          isPrivate={isPrivate}
          handleOpenLoginDialog={() => setLoginDialogOpen(true)}
        />
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
