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
import { NzSelectModule } from 'ng-zorro-antd/select';
import { Auth } from '../../services/auth/auth';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, NzFormModule, NzInputModule, NzButtonModule, NzDatePickerModule, NzAlertModule, NzCardModule, NzGridModule, NzSelectModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private fb = new FormBuilder();

  occupations = [
    'SoftwareEngineer', 'DataScientist', 'UIUXDesigner', 'Teacher', 'OfficeWorker',
    'Doctor', 'Nurse', 'Lawyer', 'Accountant', 'Driver', 'TruckDriver', 'DeliveryAgent',
    'Pilot', 'AirTrafficController', 'ConstructionWorker', 'FactoryWorker', 'Police',
    'Firefighter', 'SecurityGuard', 'Miner', 'Farmer', 'FullStackTrainer', 'Student',
    'Researcher', 'Unemployed', 'Retired', 'Artist', 'Athlete', 'Military', 'Diver', 'Other'
  ];

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2), Validators.pattern(/^[a-zA-Z\s]+$/)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
    dateOfBirth: [null as Date | null, [Validators.required]],
    phone: ['', [Validators.required, Validators.pattern(/^(\+\d{1,3}[- ]?)?\d{10}$/)]],
    address: ['', [Validators.required, Validators.minLength(5)]],
    occupation: ['', [Validators.required]],
    customOccupation: ['', []],
  });

  constructor(private auth: Auth, private router: Router) {
    this.form.get('occupation')?.valueChanges.subscribe(val => {
      const customCtrl = this.form.get('customOccupation');
      if (val === 'Other') {
        customCtrl?.setValidators([Validators.required, Validators.minLength(2), Validators.pattern(/^[a-zA-Z\s\-]+$/)]);
      } else {
        customCtrl?.clearValidators();
      }
      customCtrl?.updateValueAndValidity();
    });
  }

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

    const finalOccupation = value.occupation === 'Other' ? value.customOccupation : value.occupation;

    this.auth.register({
      name: value.name ?? '',
      email: value.email ?? '',
      password: value.password ?? '',
      dateOfBirth: dob,
      phone: value.phone ?? '',
      address: value.address ?? '',
      occupation: finalOccupation ?? '',
    }).subscribe({
      next: () => {
        this.success.set('Registered successfully. You can log in now.');
        this.loading.set(false);
        this.router.navigate(['/login']);
      },
      error: err => {
        this.error.set(err.error?.error || 'Registration failed');
        this.loading.set(false);
      }
    });
  }
}

