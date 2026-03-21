export interface IAuthenticationContext {
  isAuthenticated: boolean;
  emailAddress?: string;
  onLogin: (email: string, password: string) => Promise<void>;
  onLogout: () => void;
}