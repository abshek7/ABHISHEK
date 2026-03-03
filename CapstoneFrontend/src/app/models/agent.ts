export interface Underwriting {
  requestId: string;
  isEligible: boolean;
  overrideRiskScore?: number;
  overridePremium?: number;
  overrideCoverage?: number;
}

export interface AgentCommissionSummary {
  totalCommission: number;
  policiesSold: number;
  totalRevenueGenerated?: number;
  earnedCommission?: number;
}
export interface CreatePolicyDirect {
  customerId: string;
  policyTypeId: string;
  finalPremium: number;
  coverageAmount: number;
  startDate: string;
  nomineeName: string;
  nomineeRelation: string;
}

export interface AssignedCustomer {
  customerId: string;
  name: string;
  riskScore?: number;
}
