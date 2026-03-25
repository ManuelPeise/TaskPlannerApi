import { Box, Card, CardContent, Stack, Typography } from "@mui/material";
import React from "react";
import { useLocalization } from "../../Hooks/useLocalization";
import { Search } from "@mui/icons-material";
import { ILogMessageDashboardTile } from "../Interfaces/ILogMessageDashboardTile";

interface IProps {
  data: ILogMessageDashboardTile | null;
}

const LogDashboardTile: React.FC<IProps> = (props) => {
  const { data } = props;
  const { getResource } = useLocalization();

  if (!data) {
    return null;
  }

  return (
    <Card elevation={4} sx={{ borderRadius: 3 }}>
      <CardContent sx={{ padding: 3 }}>
        <Box display="flex" alignItems="center" gap={1} mb={2}>
          <Search fontSize="large" color="action" />
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
        <Stack spacing={1}>
          <Box display="flex" justifyContent="space-between">
            <Typography color="info.main">
              {getResource("labelInfo")}
            </Typography>
            <Typography>{data.info}</Typography>
          </Box>

          <Box display="flex" justifyContent="space-between">
            <Typography color="warning.main">
              {getResource("labelWarning")}
            </Typography>
            <Typography>{data.warning}</Typography>
          </Box>

          <Box display="flex" justifyContent="space-between">
            <Typography color="error.main">
              {getResource("labelError")}
            </Typography>
            <Typography>{data.error}</Typography>
          </Box>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default LogDashboardTile;
