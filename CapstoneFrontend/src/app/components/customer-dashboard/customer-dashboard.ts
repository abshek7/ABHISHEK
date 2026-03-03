import { Component, OnInit, computed, signal, inject } from '@angular/core';
import { Router, RouterOutlet, RouterLink } from '@angular/router';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Auth } from '../../services/auth/auth';
import { Policy } from '../../services/policy/policy';
import { Claim } from '../../services/claim/claim';
import { Payment } from '../../services/payment/payment';
import { PolicyTypes } from '../../services/policy-types/policy-types';
import { PolicyRequest } from '../../services/policy-request/policy-request';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,
    NzGridModule,
    NzCardModule,
    NzButtonModule,
    NzModalModule,
    NzFormModule,
    NzInputModule,
    NzDatePickerModule,
    NzDividerModule,
    NzMenuModule,
    NzLayoutModule,
    NzAvatarModule,
    NzPopoverModule,
    NzIconModule,
    NzPopconfirmModule,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './customer-dashboard.html',
  styleUrl: './customer-dashboard.css',
})
export class CustomerDashboard implements OnInit {
  private router = inject(Router);
  private policy = inject(Policy);
  private claim = inject(Claim);
  private paymentService = inject(Payment);
  private policyTypes = inject(PolicyTypes);
  private policyRequest = inject(PolicyRequest);
  private auth = inject(Auth);
  private fb = inject(FormBuilder);
  private message = inject(NzMessageService);

  userEmail = this.auth.email;
  userInitial = computed(() => this.userEmail()?.charAt(0).toUpperCase() || 'A');

  policies = computed(() => this.policy.customerPolicies());
  claims = computed(() => this.claim.myClaims());
  payments = computed(() => this.paymentService.myPayments());
  availablePolicyTypes = computed(() => this.policyTypes.items());
  myRequests = computed(() => this.policyRequest.myRequests());

  constructor() { }

  ngOnInit() {
    this.policy.loadCustomerPolicies();
    this.claim.loadMyClaims();
    this.paymentService.loadMyPayments();
    this.policyTypes.loadAll();
    this.policyRequest.loadMyRequests();
  }

  logout() {
    this.auth.clearAuth();
  }
}
