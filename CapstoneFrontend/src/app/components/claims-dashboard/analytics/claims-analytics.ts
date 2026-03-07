import { Component, inject, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { Claim } from '../../../services/claim/claim';

@Component({
    selector: 'app-claims-analytics', 
    imports: [
        CommonModule,
        NzGridModule,
        NzCardModule,
        NzStatisticModule
    ],
    templateUrl: './claims-analytics.html'
})
export class ClaimsAnalytics {
    private claimService = inject(Claim);

    claims = this.claimService.officerClaims;

    activeClaimsCount = computed(() => this.claims().length);
    settledCount = computed(() => this.claims().filter(c => c.status === 'Settled').length);
    settlementRatio = computed(() => {
        const total = this.activeClaimsCount();
        return total > 0 ? (this.settledCount() / total) * 100 : 0;
    });
}
