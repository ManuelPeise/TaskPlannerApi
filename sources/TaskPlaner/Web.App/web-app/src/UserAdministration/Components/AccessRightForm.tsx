import React from "react";

import { Checkbox, Grid, List, ListItem, Typography } from "@mui/material";
import { IAccessRight } from "../../Lib/Interfaces/IAccessRightModel";
import { useLocalization } from "../../Hooks/useLocalization";

interface IProps {
  maxHeight?: number;
  accessRights: IAccessRight[];
  handleAccessRightChange: (accessRight: IAccessRight) => void;
}

interface IAccessRightProps {
  accessRight: IAccessRight;
  onChange: (accessRight: IAccessRight) => void;
}

const AccessRightItem: React.FC<IAccessRightProps> = (props) => {
  const { accessRight, onChange } = props;
  const { getResource } = useLocalization();

  return (
    <ListItem sx={{ display: "flex", justifyContent: "space-between" }} divider>
      <Grid size={12} display="flex" flexDirection="row" alignItems="center">
        <Grid size={4}>
          <Typography variant="body1">{accessRight.name}</Typography>
        </Grid>
        <Grid size={8} display="flex" justifyContent="flex-end">
          <Grid display="flex" justifyContent="flex-start" alignItems="center">
            <Checkbox
              checked={accessRight.deny}
              onChange={(event) =>
                onChange({
                  ...accessRight,
                  deny: event.target.checked,
                  canView: false,
                  canCreate: false,
                  canEdit: false,
                  canDelete: false,
                })
              }
            ></Checkbox>
            <span>{getResource("labelDeny")}</span>
          </Grid>
          <Grid display="flex" justifyContent="flex-start" alignItems="center">
            <Checkbox
              checked={accessRight.canView}
              onChange={(event) =>
                onChange({
                  ...accessRight,
                  canView: event.target.checked,
                  deny: false,
                })
              }
            ></Checkbox>
            <span>{getResource("labelView")}</span>
          </Grid>
          <Grid display="flex" justifyContent="flex-start" alignItems="center">
            <Checkbox
              checked={accessRight.canCreate}
              onChange={(event) =>
                onChange({
                  ...accessRight,
                  canCreate: event.target.checked,
                  deny: false,
                })
              }
            ></Checkbox>
            <span>{getResource("labelCreate")}</span>
          </Grid>
          <Grid display="flex" justifyContent="flex-start" alignItems="center">
            <Checkbox
              checked={accessRight.canEdit}
              onChange={(event) =>
                onChange({
                  ...accessRight,
                  canEdit: event.target.checked,
                  deny: false,
                })
              }
            ></Checkbox>
            <span>{getResource("labelEdit")}</span>
          </Grid>
          <Grid display="flex" justifyContent="flex-start" alignItems="center">
            <Checkbox
              checked={accessRight.canDelete}
              onChange={(event) =>
                onChange({
                  ...accessRight,
                  canDelete: event.target.checked,
                  deny: false,
                })
              }
            ></Checkbox>
            <span>{getResource("labelDelete")}</span>
          </Grid>
        </Grid>
      </Grid>
    </ListItem>
  );
};

const AccessRightForm: React.FC<IProps> = (props) => {
  const { maxHeight, accessRights, handleAccessRightChange } = props;

  return (
    <Grid
      size={12}
      display="flex"
      alignItems="center"
      flexDirection="column"
      maxHeight={maxHeight ?? 300}
      height={maxHeight ?? 300}
      sx={{
        overflowY: "auto",
        padding: 1,
      }}
    >
      <List sx={{ width: "100%" }}>
        {accessRights.map((accessRight) => (
          <AccessRightItem
            key={accessRight.id}
            accessRight={accessRight}
            onChange={handleAccessRightChange}
          />
        ))}
      </List>
    </Grid>
  );
};

export default AccessRightForm;
