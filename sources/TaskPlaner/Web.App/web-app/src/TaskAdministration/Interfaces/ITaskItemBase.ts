import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";

export interface ITaskItemBase {
  id: number;
  title: string;
  status: TaskStatusEnum;
  shortDescription: string;
  assignedUserId?: number;
  taskTypeId?: TaskTypeEnum;
}
