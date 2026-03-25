import { IDashboardTileEndpointPerformanceDataSet } from "./IDashboardTileEndpointPerformanceDataSet";
import { ILogMessageDashboardTile } from "./ILogMessageDashboardTile";
import { IUserDashboardTile } from "./IUserDashboardTile";

export interface IDashboardData {
  logMessageTile: ILogMessageDashboardTile;
  userTile: IUserDashboardTile;
  taskControllerPerformanceData: IDashboardTileEndpointPerformanceDataSet;
}
