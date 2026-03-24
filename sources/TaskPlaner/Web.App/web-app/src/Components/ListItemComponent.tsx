import { Box, Divider, ListItem } from "@mui/material";
import React, { PropsWithChildren } from "react";
import FormLabel from "./FormLabel";

interface IProps extends PropsWithChildren {
  label: string;
  divider?: boolean;
}

const ListItemComponent: React.FC<IProps> = (props) => {
  const { label, divider, children } = props;

  return (
    <Box>
      <ListItem
        sx={{
          mb: 1,
          textAlign: "start",
          height: "100%",
          justifyContent: "space-between",
          flexDirection: "row",
          alignItems: "flex-start",
        }}
      >
        <FormLabel text={label} variant="body1" marginTop={2} />
        {children}
      </ListItem>
      {divider && <Divider component="li" />}
    </Box>
  );
};

export default ListItemComponent;
