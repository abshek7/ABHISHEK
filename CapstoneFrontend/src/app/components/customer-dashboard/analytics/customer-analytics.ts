import { Component, inject, OnInit, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { Policy } from '../../../services/policy/policy';
import { Claim } from '../../../services/claim/claim';

@Component({
    selector: 'app-customer-analytics',
    standalone: true,
    imports: [CommonModule, NzCardModule, NzGridModule],
    templateUrl: './customer-analytics.html',
    styleUrl: './customer-analytics.css'
})
export class CustomerAnalytics implements OnInit {
    private policyService = inject(Policy);
    private claimService = inject(Claim);

    policies = this.policyService.customerPolicies;
    claims = this.claimService.myClaims;

    inProgressClaimsCount = computed(() =>
        this.claims().filter(c => c.status !== 'Approved' && c.status !== 'Rejected').length
    );

    ngOnInit() {
        this.policyService.loadCustomerPolicies();
        this.claimService.loadMyClaims();
    }
}
