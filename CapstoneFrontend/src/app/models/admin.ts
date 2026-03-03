export interface Analytics {
  totalPolicies: number;
  activePolicies: number;
  totalClaims: number;
  pendingClaims: number;
  totalPolicyRequests: number;
  unassignedRequests: number;
  totalRevenue: number;
  totalUsers: number;
  totalAdmins: number;
  totalAgents: number;
  totalCustomers: number;
  totalClaimsOfficers: number;
}

export interface AgentWithWorkload {
  id: string;
  name: string;
  email: string;
  openRequestCount: number;
}

export interface OfficerWithWorkload {
  id: string;
  name: string;
  email: string;
  assignedClaimCount: number;
}

export interface CreateUser {
  name: string;
  email: string;
  password: string;
}

export interface CreatePolicyType {
  name: string;
  basePremium: number;
  baseCoverageAmount: number;
  durationMonths: number;
}

export interface CreatePolicyCoverage {
  policyTypeId: string;
  coverageCategory: string;
  percentageOfCoverage: number;
  weeklyCompensationPercentage?: number | null;
  maxWeeks?: number | null;
  description: string;
}

export interface AssignAgentToRequest {
  requestId: string;
  agentId: string;
}

export interface CreatePolicy {
  requestId: string;
  finalPremium: number;
  coverageAmount: number;
  startDate: string;
}

export interface RevenueReport {
  totalRevenue: number;
  monthlyBreakdown: MonthlyRevenue[];
}

export interface MonthlyRevenue {
  month: string;
  revenue: number;
}

export interface AgentPerformanceReport {
  agents: AgentPerformance[];
}

export interface AgentPerformance {
  agentId: string;
  agentName: string;
  policiesSold: number;
  totalCommission: number;
  totalRevenueGenerated: number;
}

export interface UpdateUser {
  id: string;
  name: string;
  email: string;
  phone: string;
  isActive: boolean;
}


