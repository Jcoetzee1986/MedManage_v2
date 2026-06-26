import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatBadgeModule } from '@angular/material/badge';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AdminService } from '../../../core/services/admin.service';
import { UserDto, RoleDto, CreateUserRequest, AdminResetPasswordRequest } from '../../../core/models/admin.models';
import { RoleAssignDialogComponent } from './role-assign-dialog.component';
import { CreateUserDialogComponent } from './create-user-dialog.component';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatChipsModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatTooltipModule,
    MatBadgeModule,
    MatDialogModule
  ],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  private readonly adminService = inject(AdminService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  users: UserDto[] = [];
  roles: RoleDto[] = [];
  loading = false;
  displayedColumns = ['userName', 'email', 'status', 'roles', 'lastLogin', 'actions'];

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers(): void {
    this.loading = true;
    this.adminService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load users', 'Dismiss', { duration: 3000 });
      }
    });
  }

  loadRoles(): void {
    this.adminService.getRoles().subscribe({
      next: (roles) => this.roles = roles,
      error: () => {}
    });
  }

  openCreateUserDialog(): void {
    const dialogRef = this.dialog.open(CreateUserDialogComponent, {
      width: '500px',
      data: { allRoles: this.roles }
    });

    dialogRef.afterClosed().subscribe((result: CreateUserRequest | undefined) => {
      if (result) {
        this.adminService.createUser(result).subscribe({
          next: () => {
            this.snackBar.open(`User "${result.username}" created successfully`, 'OK', { duration: 3000 });
            this.loadUsers();
          },
          error: () => {
            this.snackBar.open('Failed to create user', 'Dismiss', { duration: 3000 });
          }
        });
      }
    });
  }

  approveUser(user: UserDto): void {
    this.adminService.approveUser(user.userId).subscribe({
      next: () => {
        user.isApproved = true;
        this.snackBar.open(`${user.userName} approved`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to approve user', 'Dismiss', { duration: 3000 });
      }
    });
  }

  lockUser(user: UserDto): void {
    if (!confirm(`Lock account for ${user.userName}?`)) return;
    this.adminService.lockUser(user.userId).subscribe({
      next: () => {
        user.isLockedOut = true;
        this.snackBar.open(`${user.userName} locked`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to lock user', 'Dismiss', { duration: 3000 });
      }
    });
  }

  unlockUser(user: UserDto): void {
    this.adminService.unlockUser(user.userId).subscribe({
      next: () => {
        user.isLockedOut = false;
        this.snackBar.open(`${user.userName} unlocked`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to unlock user', 'Dismiss', { duration: 3000 });
      }
    });
  }

  resetPassword(user: UserDto): void {
    if (!confirm(`Reset password for ${user.userName}? A new password will be generated and emailed to the user.`)) return;
    const request: AdminResetPasswordRequest = { newPassword: null, sendEmail: true };
    this.adminService.adminResetPassword(user.userId, request).subscribe({
      next: () => {
        this.snackBar.open(`Password reset for ${user.userName}. Email sent.`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to reset password', 'Dismiss', { duration: 3000 });
      }
    });
  }

  clearFailedAttempts(user: UserDto): void {
    this.adminService.clearFailedAttempts(user.userId).subscribe({
      next: () => {
        user.failedPasswordAttemptCount = 0;
        user.isLockedOut = false;
        this.snackBar.open(`Failed attempts cleared for ${user.userName}`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to clear attempts', 'Dismiss', { duration: 3000 });
      }
    });
  }

  permanentlyBlock(user: UserDto): void {
    if (!confirm(`Permanently block ${user.userName}? This will deactivate the account. This action cannot be easily undone.`)) return;
    this.adminService.permanentlyBlockUser(user.userId).subscribe({
      next: () => {
        user.isPermanentlyBlocked = true;
        this.snackBar.open(`${user.userName} permanently blocked`, 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to block user', 'Dismiss', { duration: 3000 });
      }
    });
  }

  openRoleDialog(user: UserDto): void {
    const dialogRef = this.dialog.open(RoleAssignDialogComponent, {
      width: '400px',
      data: {
        user,
        allRoles: this.roles
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.adminService.assignRoles(user.userId, result).subscribe({
          next: () => {
            user.roles = result;
            this.snackBar.open(`Roles updated for ${user.userName}`, 'OK', { duration: 3000 });
          },
          error: () => {
            this.snackBar.open('Failed to assign roles', 'Dismiss', { duration: 3000 });
          }
        });
      }
    });
  }

  getStatusIcon(user: UserDto): string {
    if (user.isPermanentlyBlocked) return 'block';
    if (user.isLockedOut) return 'lock';
    if (!user.isApproved) return 'pending';
    return 'check_circle';
  }

  getStatusColor(user: UserDto): string {
    if (user.isPermanentlyBlocked) return 'blocked';
    if (user.isLockedOut) return 'warn';
    if (!user.isApproved) return 'accent';
    return 'primary';
  }

  getStatusText(user: UserDto): string {
    if (user.isPermanentlyBlocked) return 'Deactivated';
    if (user.isLockedOut) return 'Locked';
    if (!user.isApproved) return 'Pending Approval';
    return 'Active';
  }
}
