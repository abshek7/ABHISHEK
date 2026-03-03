import { Component, OnInit, OnDestroy, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { FormsModule, ReactiveFormsModule, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Policy } from '../../../services/policy/policy';
import { PolicyRequest } from '../../../services/policy-request/policy-request';
import { Router } from '@angular/router';
import { Invoice } from '../../../models/policy';

@Component({
  selector: 'app-customer-policy-list',
  standalone: true,
  imports: [
    CommonModule,
    NzTableModule,
    NzTagModule,
    NzInputModule,
    NzIconModule,
    NzBadgeModule,
    NzButtonModule,
    NzModalModule,
    NzInputNumberModule,
    NzDescriptionsModule,
    NzFormModule,
    NzGridModule,
    NzCardModule,
    NzDividerModule,
    NzDatePickerModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './customer-policy-list.html',
  styleUrl: './customer-policy-list.css',
})
export class CustomerPolicyList implements OnInit, OnDestroy {
  filterText = signal('');
  private policy = inject(Policy);
  private policyRequest = inject(PolicyRequest);
  private message = inject(NzMessageService);
  private fb = inject(FormBuilder);
  private router = inject(Router);

  isRenewModalVisible = signal(false);
  isInvoiceModalVisible = signal(false);
  isNomineeModalVisible = signal(false);

  selectedPolicyId = signal<string | null>(null);
  selectedRequestId = signal<string | null>(null);

  renewalDuration = signal(12);
  currentInvoice = signal<Invoice | null>(null);
  loading = signal(false);
  submitting = signal(false);

  nomineeForm: FormGroup;

  policies = computed(() => {
    const text = this.filterText().toLowerCase();
    const list = this.policy.customerPolicies();
    if (!text) return list;
    return list.filter(
      p =>
        p.policyNumber.toLowerCase().includes(text) ||
        p.policyTypeName.toLowerCase().includes(text)
    );
  });

  myRequests = computed(() => this.policyRequest.myRequests());

  constructor() {
    this.nomineeForm = this.fb.group({
      nomineeName: ['', [Validators.required]],
      nomineeRelation: ['', [Validators.required]],
      nomineePhone: ['', [Validators.required, Validators.pattern(/^[0-9]{10}$/)]],
      nomineeDob: [null, [Validators.required]]
    });
  }

  ngOnInit() {
    this.policy.loadCustomerPolicies();
    this.policyRequest.loadMyRequests();
  }

  ngOnDestroy() {
    this.policyRequest.stopPolling();
  }

  onFilter(value: string) {
    this.filterText.set(value);
  }

  openRenewModal(policyId: string) {
    this.selectedPolicyId.set(policyId);
    this.isRenewModalVisible.set(true);
  }

  submitRenewal() {
    const policyId = this.selectedPolicyId();
    if (!policyId) return;

    this.loading.set(true);
    this.policy.renewPolicy({
      policyId,
      durationMonths: this.renewalDuration()
    }).subscribe({
      next: () => {
        this.message.success('Policy renewal request submitted');
        this.isRenewModalVisible.set(false);
        this.policy.loadCustomerPolicies();
        this.loading.set(false);
      },
      error: () => {
        this.message.error('Renewal failed');
        this.loading.set(false);
      }
    });
  }

  viewInvoice(policyId: string) {
    this.loading.set(true);
    this.policy.getInvoice(policyId).subscribe({
      next: (invoice) => {
        this.currentInvoice.set(invoice);
        this.isInvoiceModalVisible.set(true);
        this.loading.set(false);
      },
      error: () => {
        this.message.error('Could not retrieve invoice');
        this.loading.set(false);
      }
    });
  }

  confirmAndPay(requestId: string) {
    this.selectedRequestId.set(requestId);
    this.nomineeForm.reset();
    this.isNomineeModalVisible.set(true);
  }

  handleNomineeCancel() {
    this.isNomineeModalVisible.set(false);
    this.selectedRequestId.set(null);
  }

  submitPurchase() {
    if (this.nomineeForm.valid && this.selectedRequestId()) {
      this.submitting.set(true);
      const { nomineeName, nomineeRelation, nomineePhone, nomineeDob } = this.nomineeForm.value;
      this.policyRequest.confirmPurchase({
        requestId: this.selectedRequestId()!,
        nomineeName,
        nomineeRelation,
        nomineePhone,
        nomineeDob
      }).subscribe({
        next: (res: any) => {
          this.message.success('Policy activated! Your protection has begun.');
          this.isNomineeModalVisible.set(false);
          this.submitting.set(false);

          const req = this.myRequests().find(r => r.id === this.selectedRequestId());
          const amount = req?.calculatedPremium || req?.suggestedPremium || 0;
          const policyId = res.policyId || this.selectedRequestId();

          this.router.navigate(['/dashboard/customer/payments'], {
            queryParams: {
              policyId: policyId,
              amount: amount
            }
          });

          this.policy.loadCustomerPolicies();
          this.policyRequest.loadMyRequests();
        },
        error: (err: any) => {
          this.message.error(err.error?.error || 'Activation failed');
          this.submitting.set(false);
        }
      });
    } else {
      Object.values(this.nomineeForm.controls).forEach(control => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}

