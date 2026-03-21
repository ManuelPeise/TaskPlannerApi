import React from "react";
import { useAuth } from "../../Hooks/useAuth";
import {
  AppBar,
  Button,
  Grid,
  Menu,
  MenuItem,
  Toolbar,
  Typography,
} from "@mui/material";
import { useLocalization } from "../../Hooks/useLocalization";
interface IProps {
  handleOpenLoginDialog: () => void;
}

const AppHeaderBar: React.FC<IProps> = (props) => {
  const { handleOpenLoginDialog } = props;
  const { isAuthenticated, emailAddress, onLogout } = useAuth();

  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const { getResource } = useLocalization();

  const handleLogout = React.useCallback(() => {
    onLogout();
    setAnchorEl(null);
  }, [onLogout]);

  const handleOpenLogin = React.useCallback(() => {
    handleOpenLoginDialog();
    setAnchorEl(null);
  }, [handleOpenLoginDialog]);

  return (
    <AppBar position="static" sx={{ backgroundColor: "#000000" }}>
      <Toolbar>
        <Grid
          container
          size={12}
          justifyContent="space-between"
          alignItems={"center"}
        >
          <Grid size="auto">
            <Typography
              variant="h6"
              sx={{ color: "#ffffff", padding: 1 }}
              component="div"
            >
              Task Planner
            </Typography>
          </Grid>
          <Grid size="auto" justifyContent="center">
            <Button
              color="inherit"
              onClick={
                emailAddress
                  ? (event) => setAnchorEl(event.currentTarget)
                  : handleOpenLogin
              }
            >
              {emailAddress ?? getResource("common.labelAuthorize")}
            </Button>
            <Menu
              anchorEl={anchorEl}
              open={Boolean(anchorEl)}
              onClose={() => setAnchorEl(null)}
            >
              {isAuthenticated && (
                <MenuItem onClick={handleLogout}>
                  {getResource("common.labelLogout")}
                </MenuItem>
              )}
              {!isAuthenticated && (
                <MenuItem onClick={handleOpenLogin}>
                  {getResource("common.labelLogin")}
                </MenuItem>
              )}
            </Menu>
          </Grid>
        </Grid>
      </Toolbar>
    </AppBar>
  );
};

export default React.memo(AppHeaderBar);
