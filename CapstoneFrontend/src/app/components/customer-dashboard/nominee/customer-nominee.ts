import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Nominee } from '../../../services/nominee/nominee';
import { Policy } from '../../../services/policy/policy';

@Component({
  selector: 'app-customer-nominee',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzButtonModule,
    NzSelectModule,
    NzDatePickerModule,
    NzCardModule,
    NzGridModule,
    NzDividerModule
  ],
  templateUrl: './customer-nominee.html',
  styleUrl: './customer-nominee.css',
})
export class CustomerNominee implements OnInit {
  private fb = inject(FormBuilder);
  private nomineeService = inject(Nominee);
  private policyService = inject(Policy);
  private message = inject(NzMessageService);

  form = this.fb.group({
    policyId: ['', [Validators.required]],
    name: ['', [Validators.required]],
    relationship: ['Spouse', [Validators.required]],
    dateOfBirth: [null as Date | null, [Validators.required]],
    phone: ['', [Validators.required, Validators.pattern(/^[0-9+() -]{7,20}$/)]],
  });

  loading = signal(false);
  policies = this.policyService.customerPolicies;

  ngOnInit() {
    this.policyService.loadCustomerPolicies();
  }

  submitNominee() {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);
    const value = this.form.value;
    const dob = value.dateOfBirth ? (value.dateOfBirth as Date).toISOString() : '';

    this.nomineeService
      .addNominee({
        policyId: value.policyId ?? '',
        name: value.name ?? '',
        relationship: value.relationship ?? '',
        dateOfBirth: dob,
        phone: value.phone ?? '',
      })
      .subscribe({
        next: () => {
          this.message.success('Nominee registered successfully');
          this.loading.set(false);
          this.form.reset({ relationship: 'Spouse' });
        },
        error: (err) => {
          this.message.error(err.error?.error || 'Registration failed');
          this.loading.set(false);
        },
      });
  }
}

