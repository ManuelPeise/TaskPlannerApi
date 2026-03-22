export interface IGitRepositoryCredentialsModel {
  id: number;
  userName: string;
  passwordHash: string;
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
}
