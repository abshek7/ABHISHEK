import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../config/api';
import { CreateNominee, Nominee as NomineeModel } from '../../models/policy';

@Injectable({
  providedIn: 'root',
})
export class Nominee {
  private http = inject(HttpClient);

  addNominee(request: CreateNominee) {
    return this.http.post<void>(`${API_BASE_URL}/Customer/nominee`, request);
  }

  getByPolicy(policyId: string) {
    return this.http.get<NomineeModel[]>(`${API_BASE_URL}/Customer/policies/${policyId}/nominees`);
  }
}

