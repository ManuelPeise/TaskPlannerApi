import { Box, Checkbox } from "@mui/material";
import React from "react";

interface IProps {
  checked: boolean;
  checkboxLabel?: string;
  disabled?: boolean;
  size?: "small" | "medium";
  onChange: (checked: boolean) => void;
}

const FormCheckbox: React.FC<IProps> = (props) => {
  const { checked, size, onChange, checkboxLabel } = props;

  return (
    <Box sx={{ display: "flex", alignItems: "center" }}>
      <Checkbox
        disabled={props.disabled}
        size={size}
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
      />
      {checkboxLabel && <span>{checkboxLabel}</span>}
    </Box>
  );
};

export default FormCheckbox;
