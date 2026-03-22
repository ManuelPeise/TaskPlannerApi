import { IGitRepositoryCredentialsModel } from "./IGitRepositoryCredentialsModel";

export interface IGitRepositoryModel {
  id: number;
  url: string;
  name: string;
  gitRepositoryCredentialsId: number;
  gitRepositoryCredentialsModel: IGitRepositoryCredentialsModel;
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
}
