import { Component, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzMessageService } from 'ng-zorro-antd/message';
import { PolicyTypes } from '../../../services/policy-types/policy-types';
import { PolicyRequest } from '../../../services/policy-request/policy-request';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
@Component({
  selector: 'app-customer-create-policy',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzButtonModule,
    NzCardModule,
    NzAlertModule,
    NzGridModule,
    NzTagModule,
    NzIconModule,
    NzDividerModule
  ],
  templateUrl: './customer-create-policy.html',
  styleUrl: './customer-create-policy.css',
})
export class CustomerCreatePolicy {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private policyTypesService = inject(PolicyTypes);
  private policyRequestService = inject(PolicyRequest);
  private message = inject(NzMessageService);

  habitsList = [
    'None', 'OccasionalAlcohol', 'RegularAlcohol', 'HeavyAlcohol',
    'Smoking', 'HeavySmoking', 'Vaping',
    'ExtremeSports', 'BungeeJumping', 'Skydiving',
    'NightDriving', 'FrequentTraveler', 'SubstanceAbuse',
    'SedentaryLifestyle', 'ActiveLifestyle'
  ];

  medicalList = [
    'Healthy', 'MinorIssues', 'Asthma', 'Diabetes',
    'Type1Diabetes', 'Type2Diabetes', 'Hypertension', 'HeartDisease',
    'Epilepsy', 'VisionProblems', 'HearingImpairment', 'ChronicPain',
    'Obesity', 'CancerRemission', 'ActiveCancer', 'TerminalIllness',
    'OrganTransplant', 'NeurologicalDisorder'
  ];

  form = this.fb.group({
    policyTypeId: ['', [Validators.required]],
    personalHabits: [[] as string[]],
    medicalHistory: [[] as string[]],
  });

  loading = signal(false);
  policyTypes = computed(() => this.policyTypesService.items());

  constructor() {
    this.policyTypesService.loadAll();
  }

  canSubmit = computed(() => this.form.valid && !this.loading());

  selectPlan(id: string) {
    this.form.patchValue({ policyTypeId: id });
  }

  submit() {
    if (this.form.invalid || this.loading()) return;
    this.loading.set(true);
    const value = this.form.value;
    const habitsStr = Array.isArray(value.personalHabits) ? value.personalHabits.join(',') : '';
    const medicalStr = Array.isArray(value.medicalHistory) ? value.medicalHistory.join(',') : '';

    this.policyRequestService
      .create({
        policyTypeId: value.policyTypeId ?? '',
        personalHabits: habitsStr,
        medicalHistory: medicalStr,
      })
      .subscribe({
        next: () => {
          this.message.success('Protection request submitted for assessment');
          this.loading.set(false);
          this.router.navigate(['/customer/main/policies']);
        },
        error: (err) => {
          this.message.error(err.error?.error || 'Request submission failed');
          this.loading.set(false);
        },
      });
  }
}

