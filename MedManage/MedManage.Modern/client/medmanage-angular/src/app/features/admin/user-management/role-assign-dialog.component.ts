import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatListModule } from '@angular/material/list';
import { UserDto, RoleDto } from '../../../core/models/admin.models';

export interface RoleAssignDialogData {
  user: UserDto;
  allRoles: RoleDto[];
}

@Component({
  selector: 'app-role-assign-dialog',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatListModule
  ],
  template: `
    <h2 mat-dialog-title>Assign Roles — {{ data.user.userName }}</h2>
    <mat-dialog-content>
      <mat-selection-list [(ngModel)]="selectedRoles">
        <mat-list-option *ngFor="let role of data.allRoles" [value]="role.roleName">
          {{ role.roleName }}
          <span *ngIf="role.description" class="role-desc"> — {{ role.description }}</span>
        </mat-list-option>
      </mat-selection-list>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .role-desc {
      color: rgba(0, 0, 0, 0.54);
      font-size: 12px;
    }
    mat-dialog-content {
      min-width: 300px;
    }
  `]
})
export class RoleAssignDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<RoleAssignDialogComponent>);
  readonly data: RoleAssignDialogData = inject(MAT_DIALOG_DATA);

  selectedRoles: string[] = [...this.data.user.roles];

  save(): void {
    this.dialogRef.close(this.selectedRoles);
  }
}
