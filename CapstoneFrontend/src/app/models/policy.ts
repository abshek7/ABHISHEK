export type PolicyStatus = 'Draft' | 'Active' | 'Expired' | 'Cancelled';

export interface PolicyTypeResponse {
  id: string;
  name: string;
  basePremium: number;
  baseCoverageAmount: number;
  durationMonths: number;
}

export interface PolicyResponse {
  id: string;
  policyNumber: string;
  policyTypeId: string;
  policyTypeName: string;
  startDate: string;
  endDate: string;
  finalPremium: number;
  coverageAmount: number;
  status: PolicyStatus;
}

export interface CreateNominee {
  policyId: string;
  name: string;
  relationship: string;
  dateOfBirth: string;
  phone: string;
}

export interface Nominee {
  id: string;
  policyId: string;
  name: string;
  relationship: string;
  dateOfBirth: string;
  phone: string;
}

export interface RenewalRequest {
  policyId: string;
  durationMonths: number;
}

export interface Invoice {
  policyNumber: string;
  customerName: string;
  totalAmount: number;
  amountPaid: number;
  issuedDate: string;
  status: string;
}
