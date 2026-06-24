import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { BookingService } from '../services/booking.service';
import { BookingDto, CreateBookingDto, UpdateBookingDto } from '../models/booking.models';

@Component({
  selector: 'app-booking-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './booking-form.component.html',
  styleUrls: ['./booking-form.component.scss']
})
export class BookingFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly bookingService = inject(BookingService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  isEditMode = false;
  bookingId: number | null = null;
  loading = false;
  currentBooking: BookingDto | null = null;

  form = this.fb.group({
    travelDate: [null as Date | null],
    travelTime: [''],
    appointmentDate: [null as Date | null],
    referringPracticeId: [null as number | null],
    memberId: [null as number | null],
    caseId: [null as number | null],
    discipline: [''],
    consultation: [false],
    admission: [false],
    currentPracticeId: [null as number | null],
    hospital: [''],
    arrived: [false],
    tisch: [''],
    comments: ['']
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam && idParam !== 'new') {
      this.isEditMode = true;
      this.bookingId = +idParam;
      this.loadBooking(this.bookingId);
    }
  }

  private loadBooking(id: number): void {
    this.loading = true;
    this.bookingService.getById(id).subscribe({
      next: (booking) => {
        this.currentBooking = booking;
        this.form.patchValue({
          travelDate: booking.travelDate ? new Date(booking.travelDate) : null,
          travelTime: booking.travelTime || '',
          appointmentDate: booking.appointmentDate ? new Date(booking.appointmentDate) : null,
          referringPracticeId: booking.referringPracticeId,
          memberId: booking.memberId,
          caseId: booking.caseId,
          discipline: booking.discipline || '',
          consultation: booking.consultation || false,
          admission: booking.admission || false,
          currentPracticeId: booking.currentPracticeId,
          hospital: booking.hospital || '',
          arrived: booking.arrived || false,
          tisch: booking.tisch || '',
          comments: booking.comments || ''
        });
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load booking', 'Close', { duration: 3000 });
        this.loading = false;
        this.router.navigate(['/bookings']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const val = this.form.value;

    if (this.isEditMode && this.bookingId) {
      const dto: UpdateBookingDto = {
        bookingId: this.bookingId,
        travelDate: val.travelDate ? this.formatDate(val.travelDate) : null,
        travelTime: val.travelTime || null,
        appointmentDate: val.appointmentDate ? this.formatDate(val.appointmentDate) : null,
        referringPracticeId: val.referringPracticeId || null,
        memberId: val.memberId || null,
        caseId: val.caseId || null,
        discipline: val.discipline || null,
        consultation: val.consultation || false,
        admission: val.admission || false,
        currentPracticeId: val.currentPracticeId || null,
        hospital: val.hospital || null,
        arrived: val.arrived || false,
        tisch: val.tisch || null,
        comments: val.comments || null
      };

      this.bookingService.update(this.bookingId, dto).subscribe({
        next: () => {
          this.snackBar.open('Booking updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/bookings']);
        },
        error: () => {
          this.snackBar.open('Failed to update booking', 'Close', { duration: 3000 });
        }
      });
    } else {
      const dto: CreateBookingDto = {
        travelDate: val.travelDate ? this.formatDate(val.travelDate) : null,
        travelTime: val.travelTime || null,
        appointmentDate: val.appointmentDate ? this.formatDate(val.appointmentDate) : null,
        referringPracticeId: val.referringPracticeId || null,
        memberId: val.memberId || null,
        caseId: val.caseId || null,
        discipline: val.discipline || null,
        consultation: val.consultation || false,
        admission: val.admission || false,
        currentPracticeId: val.currentPracticeId || null,
        hospital: val.hospital || null,
        arrived: val.arrived || false,
        tisch: val.tisch || null,
        comments: val.comments || null
      };

      this.bookingService.create(dto).subscribe({
        next: (created) => {
          this.snackBar.open('Booking created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/bookings', created.bookingId]);
        },
        error: () => {
          this.snackBar.open('Failed to create booking', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/bookings']);
  }

  onDelete(): void {
    if (!this.bookingId) return;

    if (confirm('Are you sure you want to delete this booking?')) {
      this.bookingService.delete(this.bookingId).subscribe({
        next: () => {
          this.snackBar.open('Booking deleted', 'Close', { duration: 3000 });
          this.router.navigate(['/bookings']);
        },
        error: () => {
          this.snackBar.open('Failed to delete booking', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onConvertToCase(): void {
    if (!this.bookingId || !this.currentBooking?.caseId) {
      this.snackBar.open('Booking must be linked to a case before conversion.', 'Close', { duration: 4000 });
      return;
    }

    if (!confirm('Convert this booking to a case? This will change the linked case status.')) {
      return;
    }

    this.bookingService.convertToCase(this.bookingId).subscribe({
      next: () => {
        this.snackBar.open('Booking successfully converted to case', 'Close', { duration: 3000 });
        this.router.navigate(['/bookings']);
      },
      error: (err) => {
        const message = err.error?.message || 'Failed to convert booking to case';
        this.snackBar.open(message, 'Close', { duration: 4000 });
      }
    });
  }

  private formatDate(date: Date): string {
    return date.toISOString().split('T')[0];
  }
}
