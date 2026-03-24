import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { ITaskItemBase } from "./ITaskItemBase";
import { ITaskModel } from "./ITaskModel";

export interface ITaskPageModel {
  taskModels: ITaskItemBase[];
  userDropdownItems: IDropdownItem[];
}
