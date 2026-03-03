import { Component, computed, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzAlertModule } from 'ng-zorro-antd/alert';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { Auth } from '../../services/auth/auth';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, NzFormModule, NzInputModule, NzButtonModule, NzDatePickerModule, NzAlertModule, NzCardModule, NzGridModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private fb = new FormBuilder();
  constructor(private auth: Auth, private router: Router) { }

  form = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    dateOfBirth: [null, [Validators.required]],
    phone: ['', [Validators.required]],
    address: ['', [Validators.required]],
    occupation: ['', [Validators.required]],
  });

  loading = signal(false);
  error = signal('');
  success = signal('');

  canSubmit = computed(() => this.form.valid && !this.loading());

  submit() {
    if (this.form.invalid || this.loading()) {
      return;
    }
    this.loading.set(true);
    this.error.set('');
    this.success.set('');
    const value = this.form.value;
    const dobControl = value.dateOfBirth as Date | null;
    const dob = dobControl ? dobControl.toISOString().substring(0, 10) : '';
    this.auth.register({
      name: value.name ?? '',
      email: value.email ?? '',
      password: value.password ?? '',
      dateOfBirth: dob,
      phone: value.phone ?? '',
      address: value.address ?? '',
      occupation: value.occupation ?? '',
    }).subscribe({
      next: () => {
        this.success.set('Registered successfully. You can log in now.');
        this.loading.set(false);
        this.router.navigate(['/login']);
      },
      error: _ => {
        this.error.set('Registration failed');
        this.loading.set(false);
      }
    });
  }
}

