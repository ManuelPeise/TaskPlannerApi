import { Typography } from "@mui/material";
import React from "react";

interface IProps {
  text: string | number;
  variant?:
    | "body1"
    | "body2"
    | "h1"
    | "h2"
    | "h3"
    | "h4"
    | "h5"
    | "h6"
    | "subtitle1"
    | "subtitle2";
  marginTop?: number;
  color?: string;
  bold?: boolean;
}

const FormLabel: React.FC<IProps> = (props) => {
  const { text, variant, marginTop, color, bold } = props;
  return (
    <Typography
      variant={variant}
      sx={{
        marginBottom: 1,
        marginTop: marginTop,
        color: color,
        fontWeight: bold ? "bold" : "normal",
      }}
    >
      {text}
    </Typography>
  );
};

export default FormLabel;
