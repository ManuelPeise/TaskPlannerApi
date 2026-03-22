import { IDropdownItem } from "../../Lib/Interfaces/IDropdownItem";
import { ITaskModel } from "./ITaskModel";

export interface ITaskPageModel {
  taskModels: ITaskModel[];
  gitDropdownItems: IDropdownItem[];
  userDropdownItems: IDropdownItem[];
}
