import { Button } from "@mui/material";
import React from "react";

interface IProps {
  label: string;
  disabled?: boolean;
  fullWidth?: boolean;
  action: () => void | Promise<void>;
}

const FormButton: React.FC<IProps> = (props) => {
  const { label, disabled, fullWidth, action } = props;

  return (
    <Button
      onClick={action}
      disabled={disabled}
      variant="contained"
      fullWidth={fullWidth ?? true}
    >
      {label}
    </Button>
  );
};

export default FormButton;
