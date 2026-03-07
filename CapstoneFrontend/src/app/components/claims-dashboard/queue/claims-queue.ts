import { Component,  inject, signal, computed } from '@angular/core';
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
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Claim } from '../../../services/claim/claim';

@Component({
    selector: 'app-claims-queue', 
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
        NzInputNumberModule,
        NzRadioModule,
        NzDividerModule,
        NzIconModule
    ],
    templateUrl: './claims-queue.html'
})
export class ClaimsQueue {
    private claimService = inject(Claim);
    private fb = inject(FormBuilder);
    private message = inject(NzMessageService);

    claims = this.claimService.officerClaims;
    selected = this.claimService.selectedClaim;
    submitting = signal(false);

    reviewForm = this.fb.group({
        disabilityPercentage: [null as number | null],
        recoveryWeeks: [null as number | null],
        fraudRiskScore: [null as number | null],
        notes: [''],
        approvedAmount: [null as number | null],
        approve: [true, [Validators.required]],
    });

    ngOnInit() {
        this.claimService.loadOfficerClaims();
    }

    selectClaim(id: string) {
        this.claimService.loadClaimDetail(id);
        this.reviewForm.patchValue({ approve: true });
    }

    submitReview() {
        if (!this.selected() || this.submitting()) return;
        const value = this.reviewForm.value;
        this.submitting.set(true);
        this.claimService
            .submitReview({
                claimId: this.selected()!.id,
                disabilityPercentage: value.disabilityPercentage ?? null,
                recoveryWeeks: value.recoveryWeeks ?? null,
                fraudRiskScore: value.fraudRiskScore ?? null,
                notes: value.notes ?? null,
                approvedAmount: value.approvedAmount ?? null,
                approve: value.approve ?? true,
            })
            .subscribe({
                next: () => {
                    this.message.success('Claim review completed');
                    this.submitting.set(false);
                    this.claimService.loadOfficerClaims();
                    this.reviewForm.reset({ approve: true });
                },
                error: (err) => {
                    this.message.error(err.error?.error || 'Review submission failed');
                    this.submitting.set(false);
                },
            });
    }
}
