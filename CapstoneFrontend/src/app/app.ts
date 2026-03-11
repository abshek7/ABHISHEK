import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { Auth } from './services/auth/auth';
import { Notifications } from './services/notifications/notifications';

@Component({
  selector: 'app-root', 
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    NzLayoutModule,
    NzMenuModule,
    NzButtonModule,
    NzAvatarModule,
    NzPopoverModule,
    NzPopconfirmModule,
    NzDividerModule,
    NzIconModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private auth = inject(Auth);
  private notifications = inject(Notifications);

  isAuthenticated = computed(() => this.auth.isAuthenticated());
  role = computed(() => this.auth.role());
  email = this.auth.email;
  userInitial = computed(() => this.email()?.charAt(0).toUpperCase() || 'U');
  notificationCount = computed(() => this.notifications.items().length);

  ngOnInit() {
    if (this.auth.isAuthenticated()) {
      this.notifications.loadAll();
    }
  }

  logout() {
    this.auth.clearAuth();
  }
}
