import React from "react";
import { useAuth } from "../Hooks/useAuth";
import { useLocalization } from "../Hooks/useLocalization";
import { Grid, Typography } from "@mui/material";

const Home: React.FC = () => {
  const { currentUser } = useAuth();
  const { getResource } = useLocalization();

  const greeting = React.useMemo((): string => {
    const currentHour = new Date().getHours();

    if (currentHour < 10) {
      return getResource("labelMorningGreeting").replace(
        "{User}",
        `${currentUser?.name ? currentUser.name : ""} ${currentUser?.lastName ? currentUser.lastName : ""}`,
      );
    } else if (currentHour >= 10 && currentHour < 18) {
      return getResource("labelMiddayGreeting").replace(
        "{User}",
        `${currentUser?.name ? currentUser.name : ""} ${currentUser?.lastName ? currentUser.lastName : ""}`,
      );
    } else {
      return getResource("labelAfternoonGreeting").replace(
        "{User}",
        `${currentUser?.name ? currentUser.name : ""} ${currentUser?.lastName ? currentUser.lastName : ""}`,
      );
    }
  }, [currentUser, getResource]);

  return (
    <Grid
      container
      spacing={2}
      display="flex"
      justifyContent="center"
      alignItems="center"
      height="100%"
    >
      <Grid size={12} textAlign="center">
        <Typography variant="h4">{greeting}</Typography>
      </Grid>
    </Grid>
  );
};

export default Home;
