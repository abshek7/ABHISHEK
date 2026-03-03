export type RequestStatus = 'New' | 'Assigned' | 'UnderReview' | 'Approved' | 'Rejected' | 'Accepted';

export interface PolicyRequestResponse {
  id: string;
  policyTypeId: string;
  policyTypeName: string;
  assignedAgentId: string | null;
  assignedAgentName: string | null;
  requestDate: string;
  status: string;
  totalRiskScore: number | null;
  calculatedPremium: number | null;
  coverageAmount: number | null;
  isEligible: boolean | null;
  suggestedRiskScore?: number;
  suggestedPremium?: number;
  suggestedCoverage?: number;
  nomineeName?: string;
  nomineeRelation?: string;
  isPaid?: boolean;

  customerName?: string;
  customerEmail?: string;
  customerPhone?: string;
  customerDateOfBirth?: string;
  customerOccupation?: string;
  personalHabits?: string;
  medicalHistory?: string;
}

export interface ConfirmPurchase {
  requestId: string;
  nomineeName: string;
  nomineeRelation: string;
  nomineePhone?: string;
  nomineeDob?: string;
}

export interface CreatePolicyRequest {
  policyTypeId: string;
  personalHabits?: string | null;
  medicalHistory?: string | null;
  documentFilePath?: string | null;
}

