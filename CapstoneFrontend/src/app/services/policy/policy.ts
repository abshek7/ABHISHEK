import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { PolicyResponse, RenewalRequest, Invoice } from '../../models/policy';

@Injectable({
  providedIn: 'root',
})
export class Policy {
  private http = inject(HttpClient);

  private customerPoliciesState = signal<PolicyResponse[]>([]);
  private agentPoliciesState = signal<PolicyResponse[]>([]);
  private loadingCustomerState = signal(false);
  private loadingAgentState = signal(false);

  customerPolicies = computed(() => this.customerPoliciesState());
  agentPolicies = computed(() => this.agentPoliciesState());
  loadingCustomer = computed(() => this.loadingCustomerState());
  loadingAgent = computed(() => this.loadingAgentState());

  loadCustomerPolicies() {
    this.loadingCustomerState.set(true);
    this.http.get<PolicyResponse[]>(`${API_BASE_URL}/Customer/policies`).subscribe({
      next: data => {
        this.customerPoliciesState.set(data);
        this.loadingCustomerState.set(false);
      },
      error: () => {
        this.customerPoliciesState.set([]);
        this.loadingCustomerState.set(false);
      }
    });
  }

  loadAgentPolicies() {
    this.loadingAgentState.set(true);
    this.http.get<PolicyResponse[]>(`${API_BASE_URL}/Agent/assigned-policies`).subscribe({
      next: data => {
        this.agentPoliciesState.set(data);
        this.loadingAgentState.set(false);
      },
      error: () => {
        this.agentPoliciesState.set([]);
        this.loadingAgentState.set(false);
      }
    });
  }

  renewPolicy(request: RenewalRequest) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Customer/renew-policy`, request);
  }

  getInvoice(policyId: string) {
    return this.http.get<Invoice>(`${API_BASE_URL}/Customer/policies/${policyId}/invoice`);
  }
}
