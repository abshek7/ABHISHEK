import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzMessageService } from 'ng-zorro-antd/message';
import { Admin } from '../../../services/admin/admin';

@Component({
    selector: 'app-admin-user-management', 
    imports: [
        CommonModule,
        ReactiveFormsModule,
        NzGridModule,
        NzCardModule,
        NzFormModule,
        NzInputModule,
        NzButtonModule,
        NzTableModule,
        NzTagModule,
        NzIconModule,
        NzPopconfirmModule,
        NzModalModule
    ],
    templateUrl: './admin-user-management.html'
})
export class AdminUserManagement implements OnInit {
    private admin = inject(Admin);
    private fb = inject(FormBuilder);
    private message = inject(NzMessageService);

    agentsWithWorkload = this.admin.agents;
    officersWithWorkload = this.admin.officers;
    loading = signal(false);
    isEditModalVisible = signal(false);

    createUserForm = this.fb.group({
        name: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
    });

    editUserForm = this.fb.group({
        id: ['', [Validators.required]],
        name: ['', [Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        phone: [''],
        isActive: [true],
    });

    ngOnInit() {
        this.admin.loadAgentsWithWorkload();
        this.admin.loadOfficersWithWorkload();
    }

    createAgent() {
        if (this.createUserForm.invalid) return;
        this.loading.set(true);
        this.admin.createAgent(this.createUserForm.value as any).subscribe({
            next: () => {
                this.message.success('Agent account established successfully');
                this.loading.set(false);
                this.createUserForm.reset();
                this.admin.loadAgentsWithWorkload();
            },
            error: (err) => {
                this.message.error(err.error?.error || 'Failed to create agent');
                this.loading.set(false);
            },
        });
    }

    createClaimsOfficer() {
        if (this.createUserForm.invalid) return;
        this.loading.set(true);
        this.admin.createClaimsOfficer(this.createUserForm.value as any).subscribe({
            next: () => {
                this.message.success('Claims Officer account established successfully');
                this.loading.set(false);
                this.createUserForm.reset();
                this.admin.loadOfficersWithWorkload();
            },
            error: (err) => {
                this.message.error(err.error?.error || 'Failed to create officer');
                this.loading.set(false);
            },
        });
    }

    deleteUser(userId: string) {
        this.admin.deleteUser(userId).subscribe({
            next: () => {
                this.message.success('User removed from system');
                this.admin.loadAgentsWithWorkload();
                this.admin.loadOfficersWithWorkload();
            },
            error: () => this.message.error('Failed to remove user'),
        });
    }

    openEditModal(user: any) {
        this.editUserForm.patchValue({
            id: user.id,
            name: user.name,
            email: user.email,
            phone: user.phone || '',
            isActive: user.isActive ?? true,
        });
        this.isEditModalVisible.set(true);
    }

    updateUser() {
        if (this.editUserForm.invalid) return;
        this.admin.updateUser(this.editUserForm.value as any).subscribe({
            next: () => {
                this.message.success('User profile updated');
                this.isEditModalVisible.set(false);
                this.admin.loadAgentsWithWorkload();
                this.admin.loadOfficersWithWorkload();
            },
            error: () => this.message.error('Update failed'),
        });
    }
}
