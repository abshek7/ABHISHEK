import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { Agent } from '../../../services/agent/agent';

@Component({
  selector: 'app-agent-customers',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzCardModule, NzBadgeModule],
  templateUrl: './agent-customers.html',
})
export class AgentCustomers implements OnInit {
  private agentService = inject(Agent);

  customers = this.agentService.assignedCustomers;

  ngOnInit() {
    this.agentService.loadAssignedCustomers();
  }
}
