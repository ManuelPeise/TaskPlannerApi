import { Grid } from "@mui/material";
import React, { PropsWithChildren } from "react";
import AppHeaderBar from "./AppHeaderBar";
import LoginDialog from "./LoginDialog";

const PageLayout: React.FC<PropsWithChildren> = (props) => {
  const [loginDialogOpen, setLoginDialogOpen] = React.useState(false);

  return (
    <Grid container width={"100%"}>
      <Grid size={12}>
        <AppHeaderBar handleOpenLoginDialog={() => setLoginDialogOpen(true)} />
      </Grid>
      <Grid size={12} display="flex">
        <Grid size={2} sx={{ backgroundColor: "red", height: "93vh" }}>
          <div>TEst</div>
        </Grid>
        <Grid size={10} sx={{ backgroundColor: "blue" }}>
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
