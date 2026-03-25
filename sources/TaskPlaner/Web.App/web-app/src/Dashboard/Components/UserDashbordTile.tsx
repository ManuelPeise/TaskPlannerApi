import React from "react";
import { IUserDashboardTile } from "../Interfaces/IUserDashboardTile";
import { Box, Card, CardContent, Stack, Typography } from "@mui/material";
import { AccountCircle } from "@mui/icons-material";
import { useLocalization } from "../../Hooks/useLocalization";

interface IProps {
  data: IUserDashboardTile | null;
}

const UserDashboardTile: React.FC<IProps> = (props) => {
  const { data } = props;
  const { getResource } = useLocalization();

  if (!data) {
    return null;
  }

  return (
    <Card elevation={4} sx={{ borderRadius: 3 }}>
      <CardContent
        sx={{
          padding: 3,
          display: "flex",
          flexDirection: "column",
          alignContent: "space-around",
          gap: 2,
        }}
      >
        <Box display="flex" alignItems="center" gap={1} mb={2}>
          <AccountCircle fontSize="large" color="action" />
          <Typography variant="h5">
            {getResource(data.titleResourceKey)}
          </Typography>
        </Box>

        <Box
          display="flex"
          justifyContent="flex-end"
          alignItems="center"
          gap={1}
          mb={2}
        >
          <Typography variant="h4" fontWeight="bold" mb={2}>
            {data.count}
          </Typography>
        </Box>

        {/* Details */}
        <Stack spacing={1}>
          <Box display="flex" justifyContent="space-between">
            <Typography color="info.main">
              {getResource("labelActive")}
            </Typography>
            <Typography>{data.active}</Typography>
          </Box>

          <Box display="flex" justifyContent="space-between">
            <Typography color="warning.main">
              {getResource("labelInactive")}
            </Typography>
            <Typography>{data.count - data.active}</Typography>
          </Box>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default UserDashboardTile;
