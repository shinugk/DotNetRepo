export interface Employer {
  id: number;
  companyName: string;
  type: string;
  websiteLink?: string | null;
  offerLetter?: string | null; // API usually returns URL, not byte[]
  ctcOffered?: number | null;
  interviewStatus: string;
  location: string;
  userId: number;
}
