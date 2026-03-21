import { UserRoleEnum } from "../Enums/UserRoleEnum";
import { IAccessRight } from "./IAccessRightModel";

export interface IUserData {
  id: number;
  name: string;
  lastName: string;
  emailAddress: string;
  isActive: boolean;
  userRole: UserRoleEnum;
  credentialsId?: number;
  accessRights: IAccessRight[];
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
}
