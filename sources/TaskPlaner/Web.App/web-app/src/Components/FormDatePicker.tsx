import React from "react";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";

interface IProps {
  dateValue: Date | null;
  label: string;
  disabled?: boolean;
  fullWidth?: boolean;
  minDate?: Date;
  onChange: (date: Date | null) => void;
}

const FormDatePicker: React.FC<IProps> = (props) => {
  const { dateValue, label, disabled, fullWidth, minDate, onChange } = props;

  const [value, setValue] = React.useState(dateValue ? dayjs(dateValue) : null);

  React.useEffect(() => {
    setValue(dateValue ? dayjs(dateValue) : null);
  }, [dateValue]);

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <DatePicker
        disabled={disabled}
        label={label}
        value={value}
        minDate={minDate ? dayjs(minDate) : undefined}
        onChange={(newValue) => {
          setValue(newValue);
          onChange(newValue ? newValue.toDate() : null);
        }}
        slotProps={{
          textField: { variant: "standard", fullWidth: fullWidth ?? false },
        }}
      />
    </LocalizationProvider>
  );
};

export default FormDatePicker;
