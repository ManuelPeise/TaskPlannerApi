import { Button } from "@mui/material";
import React from "react";

interface IProps {
  size?: "small" | "medium" | "large";
  variant?: "text" | "outlined" | "contained";
  label: string;
  disabled?: boolean;
  fullWidth?: boolean;
  action: () => void | Promise<void>;
}

const FormButton: React.FC<IProps> = (props) => {
  const { size, variant, label, disabled, fullWidth, action } = props;

  return (
    <Button
      size={size ?? "medium"}
      onClick={action}
      disabled={disabled}
      variant={variant ?? "contained"}
      fullWidth={fullWidth ?? false}
    >
      {label}
    </Button>
  );
};

export default FormButton;
