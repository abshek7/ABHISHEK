import { Component, computed, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { Auth } from '../../services/auth/auth';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-login', 
  imports: [ReactiveFormsModule, NzFormModule, NzInputModule, NzButtonModule, NzAlertModule, NzCardModule, NzGridModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private fb = new FormBuilder();
  constructor(private auth: Auth, private router: Router) { }

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  loading = signal(false);
  error = signal('');

  canSubmit = computed(() => this.form.valid && !this.loading());

  submit() {
    if (this.form.invalid || this.loading()) {
      return;
    }
    this.loading.set(true);
    this.error.set('');
    const value = this.form.value;
    this.auth.login({
      email: value.email ?? '',
      password: value.password ?? '',
    }).subscribe({
      next: response => {
        this.auth.setAuth(response);
        const role = response.role;
        if (role === 'Admin') {
          this.router.navigate(['/dashboard/admin']);
        } else if (role === 'Agent') {
          this.router.navigate(['/dashboard/agent']);
        } else if (role === 'Customer') {
          this.router.navigate(['/dashboard/customer']);
        } else if (role === 'ClaimsOfficer') {
          this.router.navigate(['/dashboard/claims-officer']);
        } else {
          this.router.navigate(['/welcome']);
        }
      },
      error: err => {
        this.error.set(err.error?.error || 'Invalid credentials or server error');
        this.loading.set(false);
      },
      complete: () => {
        this.loading.set(false);
      }
    });
  }
}

