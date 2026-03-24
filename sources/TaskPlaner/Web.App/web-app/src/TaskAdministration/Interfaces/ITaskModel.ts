import { TaskPriorityEnum } from "../../Lib/Enums/TaskPriorityEnum";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";
import { IUserData } from "../../Lib/Interfaces/IUserData";

export interface ITaskModel {
  taskId: number;
  title: string;
  shortDescription: string;
  description: string;
  acceptanceCriteria?: string;
  taskType: TaskTypeEnum;
  status: TaskStatusEnum;
  priority: TaskPriorityEnum;
  deadLineDate?: Date;
  assignedUserId?: number;
  assignedUser?: IUserData;
  parentTaskId?: number;
  subTasks: ITaskModel[];
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
}
