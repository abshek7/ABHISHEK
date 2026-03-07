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
import { Agent } from '../../services/agent/agent';

@Component({
  selector: 'app-agent-dashboard', 
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
  templateUrl: './agent-dashboard.html',
  styleUrl: './agent-dashboard.css',
})
export class AgentDashboard {
  private auth = inject(Auth);
  private agentService = inject(Agent);

  userEmail = this.auth.email;
  userInitial = computed(() => this.userEmail()?.charAt(0).toUpperCase() || 'A');

  pendingRequestsCount = computed(() =>
    this.agentService.assignedRequests().filter(r => r.status === 'Assigned').length
  );

  assignedCustomersCount = computed(() =>
    this.agentService.assignedCustomers().length
  );

  logout() {
    this.auth.clearAuth();
  }
}
