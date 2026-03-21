export interface IAccessRight {
  id: number;
  name: string;
  canCreate: boolean;
  canView: boolean;
  canEdit: boolean;
  canDelete: boolean;
  deny: boolean;
  isActive: boolean;
  createdAt: Date;
  createdBy: string;
  updatedAt: Date;
  updatedBy: string;
}
