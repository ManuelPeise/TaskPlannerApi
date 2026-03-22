import { Checkbox, Grid } from "@mui/material";
import React from "react";

export interface ICheckboxOption {
  label: string;
  value: number;
  checked?: boolean;
}

interface IProps {
  options: ICheckboxOption[];
  onChange: (selectedValue: ICheckboxOption) => void;
}

const FormCheckboxGroup: React.FC<IProps> = (props) => {
  const { options, onChange } = props;

  return (
    <Grid size={12} display="flex" justifyContent="flex-end">
      {options.map((option) => (
        <Grid key={option.value}>
          <Checkbox
            checked={option.checked}
            onChange={() => onChange(option)}
          />
          <span>{option.label}</span>
        </Grid>
      ))}
    </Grid>
  );
};

export default FormCheckboxGroup;
