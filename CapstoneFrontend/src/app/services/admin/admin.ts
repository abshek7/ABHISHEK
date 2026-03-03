import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import {
  AgentWithWorkload,
  Analytics,
  OfficerWithWorkload,
  CreateUser,
  CreatePolicyType,
  CreatePolicyCoverage,
  AssignAgentToRequest,
  CreatePolicy,
  RevenueReport,
  AgentPerformanceReport,
  UpdateUser,
} from '../../models/admin';
import { PolicyRequestResponse } from '../../models/requests';
import { ClaimResponse } from '../../models/claims';

@Injectable({
  providedIn: 'root',
})
export class Admin {
  private http = inject(HttpClient);

  private analyticsState = signal<Analytics | null>(null);
  private agentsState = signal<AgentWithWorkload[]>([]);
  private officersState = signal<OfficerWithWorkload[]>([]);
  private unassignedRequestsState = signal<PolicyRequestResponse[]>([]);
  private unassignedClaimsState = signal<ClaimResponse[]>([]);
  private allRequestsState = signal<PolicyRequestResponse[]>([]);
  private allClaimsState = signal<ClaimResponse[]>([]);

  analytics = computed(() => this.analyticsState());
  agents = computed(() => this.agentsState());
  officers = computed(() => this.officersState());
  unassignedRequests = computed(() => this.unassignedRequestsState());
  unassignedClaims = computed(() => this.unassignedClaimsState());
  allRequests = computed(() => this.allRequestsState());
  allClaims = computed(() => this.allClaimsState());

  loadAnalytics() {
    this.http.get<Analytics>(`${API_BASE_URL}/Admin/analytics`).subscribe({
      next: data => this.analyticsState.set(data),
      error: () => this.analyticsState.set(null),
    });
  }

  loadAgentsWithWorkload() {
    this.http.get<AgentWithWorkload[]>(`${API_BASE_URL}/Admin/agents-with-workload`).subscribe({
      next: data => this.agentsState.set(data),
      error: () => this.agentsState.set([]),
    });
  }

  loadOfficersWithWorkload() {
    this.http.get<OfficerWithWorkload[]>(`${API_BASE_URL}/Admin/officers-with-workload`).subscribe({
      next: data => this.officersState.set(data),
      error: () => this.officersState.set([]),
    });
  }

  loadUnassignedRequests() {
    this.http.get<PolicyRequestResponse[]>(`${API_BASE_URL}/Admin/unassigned-requests`).subscribe({
      next: data => this.unassignedRequestsState.set(data),
      error: () => this.unassignedRequestsState.set([]),
    });
  }

  loadAllRequests() {
    this.http.get<PolicyRequestResponse[]>(`${API_BASE_URL}/Admin/all-requests`).subscribe({
      next: data => this.allRequestsState.set(data),
      error: () => this.allRequestsState.set([]),
    });
  }

  loadUnassignedClaims() {
    this.http.get<ClaimResponse[]>(`${API_BASE_URL}/Admin/unassigned-claims`).subscribe({
      next: data => this.unassignedClaimsState.set(data),
      error: () => this.unassignedClaimsState.set([]),
    });
  }

  loadAllClaims() {
    this.http.get<ClaimResponse[]>(`${API_BASE_URL}/Admin/all-claims`).subscribe({
      next: data => this.allClaimsState.set(data),
      error: () => this.allClaimsState.set([]),
    });
  }

  assignAgentByWorkload(requestId: string) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/assign-agent-by-workload/${requestId}`, {});
  }

  assignClaimByWorkload(claimId: string) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/assign-claim-by-workload/${claimId}`, {});
  }

  createAgent(request: CreateUser) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/create-agent`, request);
  }

  createClaimsOfficer(request: CreateUser) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/create-claims-officer`, request);
  }

  createPolicyType(request: CreatePolicyType) {
    return this.http.post<{ policyTypeId: string }>(`${API_BASE_URL}/Admin/policy-type`, request);
  }

  addPolicyCoverage(request: CreatePolicyCoverage) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/policy-coverage`, request);
  }

  assignAgent(request: AssignAgentToRequest) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Admin/assign-agent`, request);
  }

  createPolicy(request: CreatePolicy) {
    return this.http.post<{ policyId: string }>(`${API_BASE_URL}/Admin/create-policy`, request);
  }

  getRevenueReport() {
    return this.http.get<RevenueReport>(`${API_BASE_URL}/Admin/revenue-report`);
  }

  getAgentPerformance() {
    return this.http.get<AgentPerformanceReport>(`${API_BASE_URL}/Admin/agent-performance`);
  }

  updateUser(request: UpdateUser) {
    return this.http.put<{ message: string }>(`${API_BASE_URL}/Admin/user`, request);
  }

  deleteUser(userId: string) {
    return this.http.delete<{ message: string }>(`${API_BASE_URL}/Admin/user/${userId}`);
  }
}

