import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { AgentCommissionSummary, Underwriting, CreatePolicyDirect, AssignedCustomer } from '../../models/agent';
import { PolicyRequestResponse } from '../../models/requests';

@Injectable({
  providedIn: 'root',
})
export class Agent {
  private http = inject(HttpClient);

  private commissionState = signal<AgentCommissionSummary | null>(null);
  private assignedRequestsState = signal<PolicyRequestResponse[]>([]);
  private assignedCustomersState = signal<AssignedCustomer[]>([]);

  commission = computed(() => this.commissionState());
  assignedRequests = computed(() => this.assignedRequestsState());
  assignedCustomers = computed(() => this.assignedCustomersState());

  loadCommissionSummary() {
    this.http.get<AgentCommissionSummary>(`${API_BASE_URL}/Agent/commission-summary`).subscribe({
      next: data => this.commissionState.set(data),
      error: () => this.commissionState.set(null),
    });
  }

  updateUnderwriting(request: Underwriting) {
    return this.http.put<{ message: string }>(`${API_BASE_URL}/Agent/underwriting`, request);
  }

  createPolicyDirect(request: CreatePolicyDirect) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Agent/create-policy-direct`, request);
  }

  loadAssignedRequests() {
    this.http
      .get<PolicyRequestResponse[]>(`${API_BASE_URL}/Agent/assigned-requests`)
      .subscribe({
        next: data => this.assignedRequestsState.set(data),
        error: () => this.assignedRequestsState.set([]),
      });
  }

  loadAssignedCustomers() {
    this.http
      .get<AssignedCustomer[]>(`${API_BASE_URL}/Agent/assigned-customers`)
      .subscribe({
        next: data => this.assignedCustomersState.set(data),
        error: () => this.assignedCustomersState.set([]),
      });
  }
}

