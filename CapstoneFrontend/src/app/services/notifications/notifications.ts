import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { Notification } from '../../models/notifications';

@Injectable({
  providedIn: 'root',
})
export class Notifications {
  private http = inject(HttpClient);

  private itemsState = signal<Notification[]>([]);

  items = computed(() => this.itemsState());

  loadAll() {
    this.http.get<Notification[]>(`${API_BASE_URL}/Customer/notifications`).subscribe({
      next: data => {
        this.itemsState.set(data);
      },
      error: () => {
        this.itemsState.set([]);
      },
    });
  }
}

