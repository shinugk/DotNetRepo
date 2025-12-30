export interface UserProfile {
  id: number;
  name: string;
  googleId: string;
  profilePictureUrl: string;
  email: string;
  age?: number | null;
  phoneNumber?: string | null;
  skills?: string | null;
  currentCompany?: string | null;
  resume?: string | null;
}