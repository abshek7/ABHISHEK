import { Component,  inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Payment } from '../../../services/payment/payment';

@Component({
  selector: 'app-customer-payments', 
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzButtonModule,
    NzTableModule,
    NzInputNumberModule,
    NzRadioModule,
    NzBadgeModule,
    NzCardModule,
    NzMenuModule,
    NzLayoutModule
  ],
  templateUrl: './customer-payments.html',
  styleUrl: './customer-payments.css',
})
export class CustomerPayments  {
  private fb = inject(FormBuilder);
  private paymentService = inject(Payment);
  private message = inject(NzMessageService);

  private route = inject(ActivatedRoute);

  selectedMenuItem = signal('process');

  form = this.fb.group({
    policyId: ['', [Validators.required]],
    amount: [0, [Validators.required, Validators.min(1)]],
    paymentMethod: ['Card', [Validators.required]],
  });

  payments = this.paymentService.myPayments;
  loading = this.paymentService.loading;
  methods = ['Card', 'NetBanking', 'UPI', 'Cash'];


  ngOnInit() {
    this.paymentService.loadMyPayments();

    // Check for pre-fill data
    this.route.queryParams.subscribe(params => {
      if (params['policyId'] || params['amount']) {
        this.form.patchValue({
          policyId: params['policyId'] || '',
          amount: params['amount'] || 0
        });
      }
    });
  }

  submitPayment() {
    if (this.form.invalid || this.loading()) return;
    const value = this.form.value;
    this.paymentService
      .createPayment({
        policyId: value.policyId ?? '',
        amount: value.amount ?? 0,
        paymentMethod: value.paymentMethod ?? '',
      })
      .subscribe({
        next: () => {
          this.message.success('Payment successfully processed');
          this.paymentService.loadMyPayments();
          this.form.reset({ paymentMethod: 'Card', amount: 0 });
        },
        error: (err) => {
          this.message.error(err.error?.error || 'Payment failed');
        },
      });
  }
}

