import React from "react";
import { ICheckboxOption } from "../../Components/FormCheckboxGroup";
import { Checkbox, Grid, List, Typography } from "@mui/material";

interface IProps {
  label?: string;
  options: ICheckboxOption[];
  handleOptionChange: (option: ICheckboxOption) => void;
}

const UserRoles: React.FC<IProps> = (props) => {
  const { label, options, handleOptionChange } = props;

  return (
    <Grid
      size={12}
      display="flex"
      alignItems="center"
      justifyContent="flex-end"
      flexDirection="row"
    >
      {label && (
        <Grid size={4}>
          <Typography variant="body1">{label}</Typography>
        </Grid>
      )}
      <List sx={{ width: "100%" }}>
        <Grid
          size={12}
          display="flex"
          justifyContent="flex-end"
          flexDirection="row"
        >
          {options.map((option) => (
            <Grid
              display="flex"
              justifyContent="flex-start"
              alignItems="center"
              key={option.value}
            >
              <Checkbox
                checked={option.checked}
                onChange={() => handleOptionChange(option)}
              />
              <span>{option.label}</span>
            </Grid>
          ))}
        </Grid>
      </List>
    </Grid>
  );
};

export default UserRoles;
