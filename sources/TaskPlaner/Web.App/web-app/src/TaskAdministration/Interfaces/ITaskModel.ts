import { TaskPriorityEnum } from "../../Lib/Enums/TaskPriorityEnum";
import { TaskStatusEnum } from "../../Lib/Enums/TaskStatusEnum";
import { TaskTypeEnum } from "../../Lib/Enums/TaskTypeEnum";
import { IUserData } from "../../Lib/Interfaces/IUserData";
import { IGitRepositoryModel } from "./IGitRepositoryModel";

export interface ITaskModel {
  taskId: number;
  title: string;
  description: string;
  taskType: TaskTypeEnum;
  status: TaskStatusEnum;
  priority: TaskPriorityEnum;
  deadLineDate?: Date;
  assignedUserId?: number;
  assignedUser?: IUserData;
  parentTaskId?: number;
  subTasks: ITaskModel[];
  createdAt: string;
  createdBy: string;
  updatedAt: string;
  updatedBy: string;
  gitRepositoryId?: number;
  gitRepository?: IGitRepositoryModel;
}
