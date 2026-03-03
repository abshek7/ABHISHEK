import { Component, OnInit, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Admin } from '../../../services/admin/admin';
import { RevenueReport, AgentPerformanceReport } from '../../../models/admin';

@Component({
    selector: 'app-admin-analytics',
    standalone: true,
    imports: [
        CommonModule,
        NzGridModule,
        NzCardModule,
        NzTableModule,
        NzBadgeModule,
        NzStatisticModule,
        NzIconModule
    ],
    templateUrl: './admin-analytics.html'
})
export class AdminAnalytics implements OnInit {
    private admin = inject(Admin);

    analytics = this.admin.analytics;
    revenueReport = signal<RevenueReport | null>(null);
    agentPerformance = signal<AgentPerformanceReport | null>(null);

    topAgent = computed(() => {
        const report = this.agentPerformance();
        if (!report || report.agents.length === 0) return null;
        return [...report.agents].sort((a, b) => b.totalRevenueGenerated - a.totalRevenueGenerated)[0];
    });

    totalCommissions = computed(() => {
        const report = this.agentPerformance();
        if (!report) return 0;
        return report.agents.reduce((sum, a) => sum + a.totalCommission, 0);
    });

    ngOnInit() {
        this.admin.loadAnalytics();
        this.admin.getRevenueReport().subscribe(data => this.revenueReport.set(data));
        this.admin.getAgentPerformance().subscribe(data => this.agentPerformance.set(data));
    }
}
