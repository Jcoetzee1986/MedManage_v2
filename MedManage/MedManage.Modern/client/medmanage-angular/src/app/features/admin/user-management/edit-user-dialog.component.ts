import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { UserDto } from '../../../core/models/admin.models';

export interface EditUserDialogData {
  user: UserDto;
}

export interface EditUserResult {
  userName: string;
  email: string;
}

@Component({
  selector: 'app-edit-user-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  template: `
    <h2 mat-dialog-title>Edit User — {{ data.user.userName }}</h2>
    <mat-dialog-content>
      <form [formGroup]="form" class="form-grid">
        <mat-form-field appearance="outline" class="span-2">
          <mat-label>Username</mat-label>
          <input matInput formControlName="userName">
        </mat-form-field>
        <mat-form-field appearance="outline" class="span-2">
          <mat-label>Email</mat-label>
          <input matInput formControlName="email" type="email">
        </mat-form-field>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-stroked-button (click)="onCancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="onSave()" [disabled]="form.invalid">Save</button>
    </mat-dialog-actions>
  `
})
export class EditUserDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<EditUserDialogComponent>);
  readonly data: EditUserDialogData = inject(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);

  form = this.fb.group({
    userName: [this.data.user.userName, Validators.required],
    email: [this.data.user.email || '']
  });

  onSave(): void {
    if (this.form.invalid) return;
    const val = this.form.value;
    this.dialogRef.close({
      userName: val.userName,
      email: val.email
    } as EditUserResult);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
