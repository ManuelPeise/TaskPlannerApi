import { TextField } from "@mui/material";
import React from "react";

interface IProps {
  value: string;
  label?: string;
  type: "text" | "password";
  fullWidth?: boolean;
  maxWidth?: number;
  disabled?: boolean;
  onChange: (value: string) => void;
}

const FormTextInput: React.FC<IProps> = (props) => {
  const { value, label, type, fullWidth, maxWidth, disabled, onChange } = props;

  return (
    <TextField
      fullWidth={fullWidth ?? true}
      value={value}
      label={label}
      type={type}
      variant="standard"
      disabled={disabled}
      onChange={(e) => onChange(e.target.value)}
      sx={{ maxWidth: maxWidth ?? "100%" }}
    />
  );
};

export default FormTextInput;
