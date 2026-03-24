import { TextareaAutosize } from "@mui/material";
import React from "react";

interface IProps {
  value: string;
  label?: string;
  disabled?: boolean;
  height?: number;
  onChange: (value: string) => void;
}

const FormTextArea: React.FC<IProps> = (props) => {
  const { value, label, disabled, height, onChange } = props;

  return (
    <TextareaAutosize
      placeholder={label}
      value={value}
      disabled={disabled}
      onChange={(e) => onChange(e.target.value)}
      style={{
        width: "100%",
        maxWidth: "100%",
        height: height || 100,
      }}
    />
  );
};

export default FormTextArea;
