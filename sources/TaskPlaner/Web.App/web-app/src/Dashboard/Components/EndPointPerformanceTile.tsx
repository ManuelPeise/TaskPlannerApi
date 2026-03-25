import React from "react";
import { useLocalization } from "../../Hooks/useLocalization";
import { Box, Card, CardContent, Typography } from "@mui/material";
import { StackedLineChart } from "@mui/icons-material";
import { IDashboardTileEndpointPerformanceDataSet } from "../Interfaces/IDashboardTileEndpointPerformanceDataSet";
import { LineChart, Line, XAxis, YAxis, Tooltip, Legend } from "recharts";

interface IProps {
  data: IDashboardTileEndpointPerformanceDataSet | null;
}

const EndPointPerformanceTile: React.FC<IProps> = (props) => {
  const { data } = props;
  const { getResource } = useLocalization();

  const formatDateTick = React.useCallback((dateString: string) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = String(date.getFullYear()).slice(-2);
    return `${day}-${month}-${year}`;
  }, []);

  const mergePerformanceData = React.useCallback(
    (endpointPerformanceData: any[]) => {
      if (!endpointPerformanceData || endpointPerformanceData.length === 0) {
        return [];
      }
      // Assume all endpoints have the same dates in the same order
      const dates = endpointPerformanceData[0].performanceData.map(
        (d: any) => d.date,
      );

      return dates.map((date: string, i: number) => {
        const row: any = { date };
        endpointPerformanceData.forEach((endpoint) => {
          // Use avg or count as needed; here we use avg
          row[getResource(endpoint.endpointLabel)] =
            endpoint.performanceData[i]?.avg ?? 0;
        });
        return row;
      });
    },
    [getResource],
  );

  if (!data) {
    return null;
  }

  return (
    <Card elevation={4} sx={{ borderRadius: 3, height: "100%" }}>
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
          <StackedLineChart fontSize="large" color="action" />
          <Typography variant="h5">
            {getResource(data.titleResourceKey)}
          </Typography>
        </Box>

        <LineChart
          width="100%"
          height={400}
          data={mergePerformanceData(data.endpointPerformanceData)}
        >
          <XAxis dataKey="date" tickFormatter={formatDateTick} />
          <YAxis />
          <Tooltip />
          <Legend formatter={getResource} />
          {data.endpointPerformanceData.map((endpoint) => (
            <Line
              key={getResource(endpoint.endpointLabel)}
              type="bump"
              dataKey={getResource(endpoint.endpointLabel)}
              strokeWidth={2}
              stroke={endpoint.colorKey}
              dot={false}
            />
          ))}
        </LineChart>
      </CardContent>
    </Card>
  );
};

export default EndPointPerformanceTile;
