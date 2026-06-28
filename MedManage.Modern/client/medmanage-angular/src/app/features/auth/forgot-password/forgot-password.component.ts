import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { ForgotPasswordRequest } from '../../../core/models/auth.models';

@Component({
  selector: 'app-forgot-password',
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
  templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  forgotPasswordForm: FormGroup;
  loading = false;
  emailSent = false;
  successMessage: string = '';
  errorMessage: string = '';

  constructor() {
    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit(): void {
    if (this.forgotPasswordForm.invalid || this.emailSent) {
      return;
    }

    this.loading = true;
    this.successMessage = '';
    this.errorMessage = '';

    const request: ForgotPasswordRequest = { 
      email: this.forgotPasswordForm.value.email 
    };

    this.authService.forgotPassword(request).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.emailSent = true;
          this.successMessage = 'A 6-digit PIN has been sent to your email. Please check your inbox.';
          this.snackBar.open('Reset PIN sent to your email!', 'Close', {
            duration: 5000
          });
        } else {
          this.errorMessage = response.message || 'Failed to send reset PIN. Please try again.';
        }
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage = error.error?.message || 'An error occurred. Please try again later.';
      }
    });
  }

  goToResetPassword(): void {
    const email = this.forgotPasswordForm.value.email;
    this.router.navigate(['/reset-password'], { 
      queryParams: { email } 
    });
  }
}
