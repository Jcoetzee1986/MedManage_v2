import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { ResetPasswordRequest } from '../../../core/models/auth.models';

@Component({
  selector: 'app-reset-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './reset-password.component.html'
})
export class ResetPasswordComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  resetPasswordForm: FormGroup;
  loading = false;
  pinVerifying = false;
  pinVerified = false;
  errorMessage: string = '';
  hideNewPassword = true;
  hideConfirmPassword = true;

  constructor() {
    this.resetPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      pin: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  ngOnInit(): void {
    // Pre-fill email from query params if available
    this.route.queryParams.subscribe(params => {
      if (params['email']) {
        this.resetPasswordForm.patchValue({ email: params['email'] });
      }
    });
  }

  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('newPassword');
    const confirmPassword = control.get('confirmPassword');

    if (!password || !confirmPassword) {
      return null;
    }

    return password.value === confirmPassword.value ? null : { passwordMismatch: true };
  }

  verifyPin(): void {
    const pinControl = this.resetPasswordForm.get('pin');
    const emailControl = this.resetPasswordForm.get('email');
    
    if (!pinControl || !emailControl || pinControl.invalid || !pinControl.value || !emailControl.value) {
      return;
    }

    this.pinVerifying = true;
    this.pinVerified = false;

    this.authService.verifyResetPin({
      email: emailControl.value,
      pin: pinControl.value
    }).subscribe({
      next: (valid) => {
        this.pinVerifying = false;
        if (valid) {
          this.pinVerified = true;
          pinControl.setErrors(null);
        } else {
          this.pinVerified = false;
          pinControl.setErrors({ invalidPin: true });
        }
      },
      error: () => {
        this.pinVerifying = false;
        this.pinVerified = false;
        pinControl.setErrors({ invalidPin: true });
      }
    });
  }

  onSubmit(): void {
    if (this.resetPasswordForm.invalid) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const request: ResetPasswordRequest = this.resetPasswordForm.value;

    this.authService.resetPassword(request).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.snackBar.open('Password reset successful! You can now login with your new password.', 'Close', {
            duration: 5000
          });
          this.router.navigate(['/login']);
        } else {
          this.errorMessage = response.message || 'Password reset failed. Please try again.';
        }
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage = error.error?.message || 'An error occurred. Please try again.';
      }
    });
  }
}
