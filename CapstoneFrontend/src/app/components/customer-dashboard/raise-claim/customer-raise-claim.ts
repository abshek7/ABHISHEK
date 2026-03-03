import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzUploadFile, NzUploadModule } from 'ng-zorro-antd/upload';
import { Policy } from '../../../services/policy/policy';
import { Claim } from '../../../services/claim/claim';
import { CoverageCategory } from '../../../models/claims';
import { Nominee } from '../../../services/nominee/nominee';
import { Nominee as NomineeModel } from '../../../models/policy';
import { FileService } from '../../../services/file/file';

@Component({
  selector: 'app-customer-raise-claim',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzDatePickerModule,
    NzButtonModule,
    NzCardModule,
    NzAlertModule,
    NzGridModule,
    NzDividerModule,
    NzInputNumberModule,
    NzIconModule,
    NzTagModule,
    NzUploadModule
  ],
  templateUrl: './customer-raise-claim.html',
  styleUrl: './customer-raise-claim.css',
}) export class CustomerRaiseClaim {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private policy = inject(Policy);
  private claim = inject(Claim);
  private nomineeService = inject(Nominee);
  private message = inject(NzMessageService);
  private fileService = inject(FileService);

  form = this.fb.group({
    policyId: ['', Validators.required],
    coverageCategory: ['', Validators.required],
    incidentDate: [null as Date | null, Validators.required],
    description: ['', Validators.required],
    claimAmount: [null as number | null, [Validators.required, Validators.min(1)]],
  });

  loading = signal(false);
  nominees = signal<NomineeModel[]>([]);
  fileList: NzUploadFile[] = [];

  coverageCategories: CoverageCategory[] = [
    'AccidentalDeath',
    'PermanentTotalDisability',
    'PermanentPartialDisability',
    'TemporaryTotalDisability',
  ];

  policies = this.policy.customerPolicies;

  hasNominees = computed(() => this.nominees().length > 0);

  constructor() {
    this.policy.loadCustomerPolicies();
  }

  // ✅ FIXED — used by button
  canSubmit(): boolean {
    return this.form.valid && !this.loading() && this.fileList.length > 0;
  }

  beforeUpload = (file: NzUploadFile): boolean => {
    this.fileList = [file];
    return false;
  };

  onPolicyChange(policyId: string) {

    this.nominees.set([]);

    if (!policyId) return;

    this.nomineeService.getByPolicy(policyId).subscribe({
      next: data => this.nominees.set(data),
      error: () => this.nominees.set([]),
    });
  }

  submit() {

    if (!this.canSubmit()) return;

    this.loading.set(true);

    const value = this.form.value;

    const date =
      value.incidentDate instanceof Date
        ? value.incidentDate.toISOString()
        : '';

    const file = this.fileList[0] as any;

    this.fileService.upload(file).subscribe({
      next: (uploadRes: { filePath: string }) => {
        this.claim.createClaim({
          policyId: value.policyId ?? '',
          coverageCategory: value.coverageCategory as CoverageCategory,
          incidentDate: date,
          description: value.description ?? '',
          claimAmount: value.claimAmount ?? 0,
          documentFilePath: uploadRes.filePath
        })
          .subscribe({
            next: () => {
              this.message.success('Claim request submitted successfully');
              this.loading.set(false);
              this.router.navigate(['/customer/main/claims']);
            },
            error: (err) => {
              this.message.error(err.error?.error || 'Failed to submit claim');
              this.loading.set(false);
            },
          });
      },
      error: () => {
        this.message.error('Failed to upload report');
        this.loading.set(false);
      }
    });
  }
}