import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { CreatePayment, EmiInstallment, PaymentResponse } from '../../models/payments';

@Injectable({
  providedIn: 'root',
})
export class Payment {
  private http = inject(HttpClient);

  private myPaymentsState = signal<PaymentResponse[]>([]);
  private emiScheduleState = signal<EmiInstallment[]>([]);
  private loadingState = signal(false);

  myPayments = computed(() => this.myPaymentsState());
  emiSchedule = computed(() => this.emiScheduleState());
  loading = computed(() => this.loadingState());

  loadMyPayments() {
    this.loadingState.set(true);
    this.http.get<PaymentResponse[]>(`${API_BASE_URL}/Customer/payments`).subscribe({
      next: data => {
        this.myPaymentsState.set(data);
        this.loadingState.set(false);
      },
      error: () => {
        this.myPaymentsState.set([]);
        this.loadingState.set(false);
      }
    });
  }

  createPayment(request: CreatePayment) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Customer/payment`, request);
  }

  loadEmiSchedule(policyId: string) {
    this.http
      .get<EmiInstallment[]>(`${API_BASE_URL}/Customer/policies/${policyId}/emi-schedule`)
      .subscribe({
        next: data => {
          this.emiScheduleState.set(data);
        },
        error: () => {
          this.emiScheduleState.set([]);
        },
      });
  }
}

