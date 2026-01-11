export interface Employer {
  id: number;
  companyName: string;
  type: string;
  websiteLink?: string | null;
  offerLetter?: string | null; // API usually returns URL, not byte[]
  ctcOffered?: number | null;
  hrDetail: HRDetail;
  interviewStatus: string;
  location: string;
  userId: number;
}

export interface HRDetail{
  name: string;
  phoneNumber: string;
  emailId: string;
}
