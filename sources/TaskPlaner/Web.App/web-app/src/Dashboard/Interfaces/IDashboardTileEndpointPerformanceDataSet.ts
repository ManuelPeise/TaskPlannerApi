export interface IDashboardTileEndpointPerformanceDataSet {
  titleResourceKey: string;
  endpointPerformanceData: IDashboardEndpointPerformanceData[];
}

export interface IDashboardEndpointPerformanceData {
  endpointLabel: string;
  colorKey: string;
  performanceData: ILineChartPointData[];
}

export interface ILineChartPointData {
  count: number;
  avg: number;
  date: Date;
  color: string;
}
