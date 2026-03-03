import { Component, OnInit, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { Auth } from '../../services/auth/auth';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    NzGridModule,
    NzMenuModule,
    NzLayoutModule,
    NzPopoverModule,
    NzAvatarModule,
    NzDividerModule,
    NzIconModule,
    NzButtonModule,
    NzPopconfirmModule
  ],
  templateUrl: './admin-dashboard.html',
  styleUrls: ['./admin-dashboard.css']
})
export class AdminDashboard implements OnInit {
  private auth = inject(Auth);

  userEmail = this.auth.email;
  userInitial = computed(() => this.userEmail()?.charAt(0).toUpperCase() || 'A');

  ngOnInit() {
    // Shared admin initialization if any
  }

  logout() {
    this.auth.clearAuth();
  }
}
