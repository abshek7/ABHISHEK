import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { CreatePolicyRequest, PolicyRequestResponse, ConfirmPurchase } from '../../models/requests';

@Injectable({
  providedIn: 'root',
})
export class PolicyRequest {
  private http = inject(HttpClient);

  private myRequestsState = signal<PolicyRequestResponse[]>([]);
  private loadingState = signal(false);

  private pollingInterval: any;

  myRequests = computed(() => this.myRequestsState());
  loading = computed(() => this.loadingState());

  loadMyRequests() {
    this.loadingState.set(true);
    this.http.get<PolicyRequestResponse[]>(`${API_BASE_URL}/Customer/policy-requests`).subscribe({
      next: data => {
        this.myRequestsState.set(data);
        this.loadingState.set(false);
        this.checkPolling();
      },
      error: () => {
        this.myRequestsState.set([]);
        this.loadingState.set(false);
      }
    });
  }

  private checkPolling() {
    const hasPending = this.myRequests().some(r =>
      ['New', 'Assigned', 'UnderReview'].includes(r.status));

    if (hasPending) {
      this.startPolling();
    } else {
      this.stopPolling();
    }
  }

  private startPolling() {
    if (this.pollingInterval) return;
    this.pollingInterval = setInterval(() => {
      this.http.get<PolicyRequestResponse[]>(`${API_BASE_URL}/Customer/policy-requests`).subscribe({
        next: data => {
          this.myRequestsState.set(data);
          this.checkPolling();
        }
      });
    }, 5000);
  }

  stopPolling() {
    if (this.pollingInterval) {
      clearInterval(this.pollingInterval);
      this.pollingInterval = null;
    }
  }

  create(request: CreatePolicyRequest) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Customer/policy-request`, request);
  }

  confirmPurchase(confirmation: ConfirmPurchase) {
    return this.http.post<{ message: string, policyId: string }>(`${API_BASE_URL}/Customer/confirm-purchase`, confirmation);
  }
}

