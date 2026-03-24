import { TaskPriorityEnum } from "../../Lib/Enums/TaskPriorityEnum";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";

export interface ITaskFilterOptions {
  assignedUserId: number;
  taskTypeId?: TaskTypeEnum;
}
