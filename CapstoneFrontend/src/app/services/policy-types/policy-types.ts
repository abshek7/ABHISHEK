import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { PolicyTypeResponse } from '../../models/policy';

@Injectable({
  providedIn: 'root',
})
export class PolicyTypes {
  private http = inject(HttpClient);

  private itemsState = signal<PolicyTypeResponse[]>([]);
  private loadingState = signal(false);

  items = computed(() => this.itemsState());
  loading = computed(() => this.loadingState());

  loadAll() {
    if (this.loadingState()) {
      return;
    }
    this.loadingState.set(true);
    this.http.get<PolicyTypeResponse[]>(`${API_BASE_URL}/Customer/policy-types`).subscribe({
      next: data => {
        this.itemsState.set(data);
        this.loadingState.set(false);
      },
      error: () => {
        this.itemsState.set([]);
        this.loadingState.set(false);
      }
    });
  }
}

