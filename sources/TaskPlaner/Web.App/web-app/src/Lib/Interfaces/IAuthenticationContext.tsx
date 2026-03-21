import { IUserData } from "./IUserData";

export interface IAuthenticationContext {
  isAuthenticated: boolean;
  isLoading: boolean;
  currentUser: IUserData | null;
  onLogin: (email: string, password: string) => Promise<void>;
  onLogout: () => void;
}
