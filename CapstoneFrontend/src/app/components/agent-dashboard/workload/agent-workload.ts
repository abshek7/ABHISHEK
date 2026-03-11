import { Component, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzDescriptionsModule } from 'ng-zorro-antd/descriptions';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Agent } from '../../../services/agent/agent';
import { PolicyRequestResponse } from '../../../models/requests';

@Component({
    selector: 'app-agent-workload', 
    imports: [
        CommonModule,
        ReactiveFormsModule,
        NzGridModule,
        NzCardModule,
        NzTableModule,
        NzFormModule,
        NzInputModule,
        NzButtonModule,
        NzTagModule,
        NzBadgeModule,
        NzModalModule,
        NzDescriptionsModule,
        NzInputNumberModule,
        NzRadioModule
    ],
    templateUrl: './agent-workload.html'
})
export class AgentWorkload  {
    private agentService = inject(Agent);
    private fb = inject(FormBuilder);
    private message = inject(NzMessageService);

    assignedRequests = this.agentService.assignedRequests;
    selectedRequest: PolicyRequestResponse | null = null;
    selectedCustomer: PolicyRequestResponse | null = null;
    customerModalVisible = signal(false);

    underwritingForm = this.fb.group({
        requestId: ['', [Validators.required]],
        isEligible: [true],
        overrideRiskScore: [null as number | null],
        overridePremium: [null as number | null],
        overrideCoverage: [null as number | null],
    });

    ngOnInit() {
        this.agentService.loadAssignedRequests();
    }

    selectRequest(request: PolicyRequestResponse) {
        this.selectedRequest = request;
        this.underwritingForm.patchValue({
            requestId: request.id,
            isEligible: true,
            overrideRiskScore: request.suggestedRiskScore || request.totalRiskScore,
            overridePremium: request.suggestedPremium || request.calculatedPremium,
            overrideCoverage: request.suggestedCoverage || request.coverageAmount
        });
    }

    submitUnderwriting() {
        if (this.underwritingForm.invalid) return;
        const value = this.underwritingForm.value;
        this.agentService
            .updateUnderwriting({
                requestId: value.requestId ?? '',
                isEligible: value.isEligible ?? true,
                overrideRiskScore: value.overrideRiskScore ?? undefined,
                overridePremium: value.overridePremium ?? undefined,
                overrideCoverage: value.overrideCoverage ?? undefined,
            })
            .subscribe({
                next: () => {
                    this.message.success('Underwriting decision finalized');
                    this.agentService.loadAssignedRequests();
                    this.agentService.loadCommissionSummary();
                    this.selectedRequest = null;
                    this.underwritingForm.reset({ isEligible: true });
                },
                error: (err) => {
                    this.message.error(err.error?.error || 'Failed to submit decision');
                }
            });
    }

    showCustomerDetails(request: PolicyRequestResponse) {
        this.selectedCustomer = request;
        this.customerModalVisible.set(true);
    }
}
