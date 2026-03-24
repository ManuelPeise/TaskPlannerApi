import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { ITaskModel } from "./ITaskModel";

export interface ITaskDetailsPageModel {
  task: ITaskModel;
  userDropdownItems: IDropdownItem[];
  priorityDropdownItems: IDropdownItem[];
  statusDropdownItems: IDropdownItem[];
}
