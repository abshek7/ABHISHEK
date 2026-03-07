import { Component, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { Auth } from '../../services/auth/auth';
import { Claim } from '../../services/claim/claim';

@Component({
  selector: 'app-claims-dashboard', 
  imports: [
    CommonModule,
    NzMenuModule,
    NzLayoutModule,
    NzAvatarModule,
    NzPopoverModule,
    NzPopconfirmModule,
    NzDividerModule,
    NzIconModule,
    NzBadgeModule,
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './claims-dashboard.html',
  styleUrl: './claims-dashboard.css',
})
export class ClaimsDashboard {
  private auth = inject(Auth);
  private claimService = inject(Claim);

  userEmail = this.auth.email;
  userInitial = computed(() => this.userEmail()?.charAt(0).toUpperCase() || 'A');

  pendingClaimsCount = computed(() =>
    this.claimService.officerClaims().filter(c => c.status === 'Submitted' || c.status === 'UnderReview').length
  );

  logout() {
    this.auth.clearAuth();
  }
}
