import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { RoleDto, CreateUserRequest } from '../../../core/models/admin.models';

export interface CreateUserDialogData {
  allRoles: RoleDto[];
}

@Component({
  selector: 'app-create-user-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatIconModule
  ],
  template: `
    <h2 mat-dialog-title>Create New User</h2>
    <mat-dialog-content>
      <div class="create-user-form">
        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Username</mat-label>
          <input matInput [(ngModel)]="username" required placeholder="Enter username">
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Email</mat-label>
          <input matInput [(ngModel)]="email" required type="email" placeholder="Enter email address">
        </mat-form-field>

        <mat-form-field appearance="outline" class="full-width">
          <mat-label>Temporary Password (optional)</mat-label>
          <input matInput [(ngModel)]="temporaryPassword" placeholder="Leave blank to auto-generate">
          <mat-hint>If blank, a secure password will be generated</mat-hint>
        </mat-form-field>

        <div class="roles-section">
          <label class="roles-label">Roles</label>
          <div class="roles-list">
            <mat-checkbox *ngFor="let role of data.allRoles"
                          [checked]="selectedRoles.includes(role.roleName)"
                          (change)="toggleRole(role.roleName, $event.checked)">
              {{ role.roleName }}
            </mat-checkbox>
          </div>
        </div>

        <mat-checkbox [(ngModel)]="sendWelcomeEmail" class="welcome-email-checkbox">
          Send welcome email with credentials
        </mat-checkbox>
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary"
              [disabled]="!username || !email"
              (click)="save()">
        Create User
      </button>
    </mat-dialog-actions>
  `
})
export class CreateUserDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<CreateUserDialogComponent>);
  readonly data: CreateUserDialogData = inject(MAT_DIALOG_DATA);

  username = '';
  email = '';
  temporaryPassword = '';
  selectedRoles: string[] = [];
  sendWelcomeEmail = true;

  toggleRole(roleName: string, checked: boolean | null): void {
    if (checked) {
      if (!this.selectedRoles.includes(roleName)) {
        this.selectedRoles.push(roleName);
      }
    } else {
      this.selectedRoles = this.selectedRoles.filter(r => r !== roleName);
    }
  }

  save(): void {
    const request: CreateUserRequest = {
      username: this.username,
      email: this.email,
      temporaryPassword: this.temporaryPassword || null,
      roles: this.selectedRoles,
      sendWelcomeEmail: this.sendWelcomeEmail
    };
    this.dialogRef.close(request);
  }
}
