import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzCardModule } from 'ng-zorro-antd/card';
import { Admin } from '../../services/admin/admin';

@Component({
  selector: 'app-reports-page',
  standalone: true,
  imports: [CommonModule, NzTableModule, NzCardModule],
  templateUrl: './reports-page.html',
  styleUrl: './reports-page.css',
})
export class ReportsPage implements OnInit {
  private admin = inject(Admin);

  analytics = this.admin.analytics;
  agents = this.admin.agents;
  officers = this.admin.officers;

  ngOnInit() {
    this.admin.loadAnalytics();
    this.admin.loadAgentsWithWorkload();
    this.admin.loadOfficersWithWorkload();
  }
}

