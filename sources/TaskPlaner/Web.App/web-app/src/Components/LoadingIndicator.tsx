import { Box, LinearProgress } from "@mui/material";
import React from "react";

interface IProps {
  isLoading: boolean;
}

const LoadingIndicator: React.FC<IProps> = (props) => {
  const { isLoading } = props;

  if (!isLoading) {
    return null;
  }

  return (
    <Box sx={{ position: "fixed", top: 0, left: 0, right: 0 }}>
      <LinearProgress
        variant="indeterminate"
        sx={{
          height: 6,
          backgroundColor: "#eee",
          "& .MuiLinearProgress-bar": {
            background: "#FFD580", // light orange
          },
        }}
      />
    </Box>
  );
};

export default LoadingIndicator;
