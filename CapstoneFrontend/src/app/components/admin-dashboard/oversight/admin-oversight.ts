import { Component, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Admin } from '../../../services/admin/admin';

@Component({
    selector: 'app-admin-oversight', 
    imports: [
        CommonModule,
        NzGridModule,
        NzCardModule,
        NzTableModule,
        NzTagModule,
        NzBadgeModule
    ],
    templateUrl: './admin-oversight.html'
})
export class AdminOversight   {
    private admin = inject(Admin);
    private message = inject(NzMessageService);

    allRequests = this.admin.allRequests;
    allClaims = this.admin.allClaims;
    loading = signal(false);

    inProgressRequests = computed(() =>
        this.allRequests().filter(r => r.assignedAgentId != null && ['Assigned', 'UnderReview'].includes(r.status))
    );

    inProgressClaims = computed(() =>
        this.allClaims().filter(c => c.officerId != null && ['UnderReview'].includes(c.status))
    );

    ngOnInit() {
        this.admin.loadAllRequests();
        this.admin.loadAllClaims();
    }
}
