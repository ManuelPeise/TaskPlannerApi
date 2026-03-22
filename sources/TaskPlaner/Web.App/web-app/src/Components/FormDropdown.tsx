import React from "react";
import { IDropdownItem } from "../Lib/Interfaces/IDropdownItem";
import { MenuItem, Select } from "@mui/material";

interface IProps {
  value: IDropdownItem | null;
  dropdownItems: IDropdownItem[];
  disabled?: boolean;
  onChange: ((value: IDropdownItem) => void) | null;
}

const FormDropdown: React.FC<IProps> = (props) => {
  const { value, dropdownItems, disabled, onChange } = props;

  return (
    <Select
      fullWidth
      value={value?.id ?? ""}
      disabled={disabled}
      variant="standard"
      onChange={(event) => {
        const selectedItem = dropdownItems.find(
          (item) => item.id === event.target.value,
        );
        if (selectedItem && onChange) {
          onChange(selectedItem);
        }
      }}
    >
      {dropdownItems.map((item) => (
        <MenuItem key={item.id} value={item.id}>
          {item.label}
        </MenuItem>
      ))}
    </Select>
  );
};

export default FormDropdown;
