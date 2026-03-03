import { Nominee } from './policy';

export type CoverageCategory =
  | 'AccidentalDeath'
  | 'PermanentTotalDisability'
  | 'PermanentPartialDisability'
  | 'TemporaryTotalDisability';

export type ClaimStatus = 'Submitted' | 'UnderReview' | 'Approved' | 'Rejected' | 'Settled';

export interface CreateClaim {
  policyId: string;
  coverageCategory: CoverageCategory;
  incidentDate: string;
  description: string;
  claimAmount: number;
  documentFilePath?: string | null;
}

export interface ClaimResponse {
  id: string;
  policyId: string;
  coverageCategory: CoverageCategory;
  incidentDate: string;
  description: string;
  claimAmount: number;
  approvedAmount: number | null;
  status: ClaimStatus;
  officerId: string | null;
}

export interface ClaimDetail {
  id: string;
  policyId: string;
  policyNumber: string;
  customerId: string;
  customerName: string | null;
  occupation: string | null;
  coverageCategory: CoverageCategory;
  incidentDate: string;
  description: string;
  claimAmount: number;
  approvedAmount: number | null;
  status: ClaimStatus;
  officerId: string | null;
  documentFilePath: string | null;
  totalRiskScore: number | null;
  medicalHistory: string | null;
  personalHabits: string | null;
  nominees: Nominee[];
}

export interface ClaimReview {
  claimId: string;
  disabilityPercentage: number | null;
  recoveryWeeks: number | null;
  fraudRiskScore: number | null;
  notes: string | null;
  approvedAmount: number | null;
  approve: boolean;
}

export interface Settlement {
  claimId: string;
  settlementAmount: number;
}


