import { Typography } from "@mui/material";
import React from "react";

interface IProps {
  text: string | number;
  variant?: "body1" | "body2" | "h1" | "h2" | "h3" | "h4" | "h5" | "h6";
  marginTop?: number;
}

const FormLabel: React.FC<IProps> = (props) => {
  const { text, variant, marginTop } = props;
  return (
    <Typography
      variant={variant}
      sx={{ marginBottom: 1, marginTop: marginTop }}
    >
      {text}
    </Typography>
  );
};

export default FormLabel;
