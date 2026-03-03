import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { ClaimDetail, ClaimResponse, CreateClaim, ClaimReview, Settlement } from '../../models/claims';

@Injectable({
  providedIn: 'root',
})
export class Claim {
  private http = inject(HttpClient);

  private myClaimsState = signal<ClaimResponse[]>([]);
  private officerClaimsState = signal<ClaimResponse[]>([]);
  private selectedClaimState = signal<ClaimDetail | null>(null);
  private loadingMyState = signal(false);
  private loadingOfficerState = signal(false);
  private loadingDetailState = signal(false);

  myClaims = computed(() => this.myClaimsState());
  officerClaims = computed(() => this.officerClaimsState());
  selectedClaim = computed(() => this.selectedClaimState());
  loadingMy = computed(() => this.loadingMyState());
  loadingOfficer = computed(() => this.loadingOfficerState());
  loadingDetail = computed(() => this.loadingDetailState());

  loadMyClaims() {
    this.loadingMyState.set(true);
    this.http.get<ClaimResponse[]>(`${API_BASE_URL}/Customer/claims`).subscribe({
      next: data => {
        this.myClaimsState.set(data);
        this.loadingMyState.set(false);
      },
      error: () => {
        this.myClaimsState.set([]);
        this.loadingMyState.set(false);
      }
    });
  }

  createClaim(request: CreateClaim) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Customer/claim`, request);
  }

  loadOfficerClaims() {
    this.loadingOfficerState.set(true);
    this.http.get<ClaimResponse[]>(`${API_BASE_URL}/ClaimsOfficer/claims`).subscribe({
      next: data => {
        this.officerClaimsState.set(data);
        this.loadingOfficerState.set(false);
      },
      error: () => {
        this.officerClaimsState.set([]);
        this.loadingOfficerState.set(false);
      }
    });
  }

  loadClaimDetail(id: string) {
    this.loadingDetailState.set(true);
    this.http.get<ClaimDetail>(`${API_BASE_URL}/ClaimsOfficer/claims/${id}`).subscribe({
      next: data => {
        this.selectedClaimState.set(data);
        this.loadingDetailState.set(false);
      },
      error: () => {
        this.selectedClaimState.set(null);
        this.loadingDetailState.set(false);
      }
    });
  }

  submitReview(request: ClaimReview) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/ClaimsOfficer/review`, request);
  }

  createSettlement(request: Settlement) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/ClaimsOfficer/settlement`, request);
  }
}

