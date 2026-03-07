import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzMessageService } from 'ng-zorro-antd/message';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { Policy } from '../../../services/policy/policy';
import { Agent } from '../../../services/agent/agent';
import { PolicyTypes } from '../../../services/policy-types/policy-types';
import { CreatePolicyDirect } from '../../../models/agent';

@Component({
    selector: 'app-agent-policies', 
    imports: [
        CommonModule,
        ReactiveFormsModule,
        NzGridModule,
        NzCardModule,
        NzTableModule,
        NzFormModule,
        NzInputModule,
        NzSelectModule,
        NzButtonModule,
        NzInputNumberModule,
        NzTagModule,
        NzBadgeModule,
        NzModalModule
    ],
    templateUrl: './agent-policies.html'
})
export class AgentPolicies  {
    private policyService = inject(Policy);
    private agentService = inject(Agent);
    private policyTypesService = inject(PolicyTypes);
    private fb = inject(FormBuilder);
    private message = inject(NzMessageService);

    assignedPolicies = this.policyService.agentPolicies;
    commission = this.agentService.commission;
    policyTypes = this.policyTypesService.items;
    isDirectModalVisible = signal(false);

    directIssuanceForm = this.fb.group({
        customerId: ['', [Validators.required]],
        policyTypeId: ['', [Validators.required]],
        finalPremium: [100, [Validators.required, Validators.min(1)]],
        coverageAmount: [1000, [Validators.required, Validators.min(100)]],
        startDate: [new Date().toISOString(), [Validators.required]],
        paymentFrequency: ['Annual', [Validators.required]],
        nomineeName: ['', [Validators.required]],
        nomineeRelation: ['', [Validators.required]],
    });

    ngOnInit() {
        this.policyService.loadAgentPolicies();
        this.agentService.loadCommissionSummary();
        this.policyTypesService.loadAll();
    }

    submitDirectIssuance() {
        if (this.directIssuanceForm.invalid) return;
        this.agentService.createPolicyDirect(this.directIssuanceForm.value as CreatePolicyDirect).subscribe({
            next: () => {
                this.message.success('Policy issued directly to customer');
                this.isDirectModalVisible.set(false);
                this.directIssuanceForm.reset();
                this.policyService.loadAgentPolicies();
                this.agentService.loadCommissionSummary();
            },
            error: () => this.message.error('Direct issuance failed'),
        });
    }
}
