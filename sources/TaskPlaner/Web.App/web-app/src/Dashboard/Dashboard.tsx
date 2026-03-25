import React from "react";
import useLocalStorage, { LocalStorageKeys } from "../Hooks/useLocalStorage";
import useStateFulApiService from "../Hooks/useStateFulApiService";
import { ITokenData } from "../Lib/Interfaces/ITokenData";
import { useLocalization } from "../Hooks/useLocalization";
import { Grid } from "@mui/material";
import LogDashboardTile from "./Components/LogDashboardTile";
import { IDashboardData } from "./Interfaces/IDashboardData";
import UserDashboardTile from "./Components/UserDashbordTile";
import EndPointPerformanceTile from "./Components/EndPointPerformanceTile";

interface IProps {
  data: IDashboardData;
}

const DashboardContainer: React.FC = () => {
  const store = useLocalStorage<ITokenData>(LocalStorageKeys.Token);

  const currentTimeStamp = new Date();
  const today = currentTimeStamp.toISOString().split("T")[0];
  const aWeekAgo = new Date(
    currentTimeStamp.getTime() - 7 * 24 * 60 * 60 * 1000,
  )
    .toISOString()
    .split("T")[0];

  const dashboardApi = useStateFulApiService<IDashboardData>({
    method: "GET",
    requestUrl:
      process.env.REACT_APP_API_URL +
      `dashboard/getdashboarddata?from=${aWeekAgo}&to=${today}`,
    token: store.getItem()?.jwt ?? null,
  });

  if (!dashboardApi.response) {
    return null;
  }

  return <Dashboard data={dashboardApi.response} />;
};

const Dashboard: React.FC<IProps> = (props) => {
  const { data } = props;
  //   const { getResource } = useLocalization();

  return (
    <Grid container spacing={2} width="100%">
      <Grid container spacing={4} width="100%" height="100%">
        {data.logMessageTile && (
          <Grid size={{ xs: 12, sm: 6, md: 6 }} height={250}>
            <LogDashboardTile data={data.logMessageTile} />
          </Grid>
        )}
        {data.userTile && (
          <Grid size={{ xs: 12, sm: 6, md: 6 }} height={250}>
            <UserDashboardTile data={data.userTile} />
          </Grid>
        )}
        {data.taskControllerPerformanceData && (
          <Grid size={{ xs: 12, sm: 12, md: 12 }} height={600}>
            <EndPointPerformanceTile
              data={data.taskControllerPerformanceData}
            />
          </Grid>
        )}
      </Grid>
    </Grid>
  );
};

export default DashboardContainer;
