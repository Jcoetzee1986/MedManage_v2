import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { BookingService } from '../services/booking.service';
import { BookingDto, BookingSearchFilters } from '../models/booking.models';
import { DataTableComponent, DataTableColumn } from '../../../shared/components/data-table/data-table.component';

@Component({
  selector: 'app-booking-list',
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
    MatSnackBarModule,
    MatTooltipModule,
    DataTableComponent
  ],
  templateUrl: './booking-list.component.html',
  styleUrls: ['./booking-list.component.scss']
})
export class BookingListComponent implements OnInit {
  private readonly bookingService = inject(BookingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  rowData: BookingDto[] = [];
  totalCount = 0;
  pageSize = 30;
  pageIndex = 0;
  loading = false;
  currentSortBy?: string;
  currentSortDescending?: boolean;

  searchForm = this.fb.group({
    surname: [''],
    name: [''],
    memberNumber: [''],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** Column definitions for DataTableComponent */
  tableColumnDefs: DataTableColumn[] = [
    { field: 'bookingId', header: 'ID', width: '70px' },
    { field: 'travelDate', header: 'Travel Date', width: '110px', pipe: 'date' },
    { field: 'travelTime', header: 'Time', width: '80px' },
    { field: 'appointmentDate', header: 'Appointment', width: '110px', pipe: 'date' },
    { field: 'discipline', header: 'Discipline' },
    { field: 'hospital', header: 'Hospital' },
    { field: 'consultation', header: 'Consult', width: '80px', format: (v) => v ? 'Yes' : 'No' },
    { field: 'admission', header: 'Admission', width: '90px', format: (v) => v ? 'Yes' : 'No' },
    { field: 'arrived', header: 'Arrived', width: '80px', format: (v) => v ? 'Yes' : 'No' },
    { field: 'caseId', header: 'Case ID', width: '90px' }
  ];

  ngOnInit(): void {
    this.loadBookings();
  }

  loadBookings(): void {
    this.loading = true;
    const formValue = this.searchForm.value;

    const filters: BookingSearchFilters = {
      surname: formValue.surname || undefined,
      name: formValue.name || undefined,
      memberNumber: formValue.memberNumber || undefined,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      sortBy: this.currentSortBy,
      sortDescending: this.currentSortDescending
    };

    this.bookingService.search(filters).subscribe({
      next: (result) => {
        this.rowData = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load bookings', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.loadBookings();
  }

  onReset(): void {
    this.searchForm.reset();
    this.pageIndex = 0;
    this.loadBookings();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadBookings();
  }

  onSortChange(sort: Sort): void {
    if (sort.direction) {
      this.currentSortBy = sort.active;
      this.currentSortDescending = sort.direction === 'desc';
    } else {
      this.currentSortBy = undefined;
      this.currentSortDescending = undefined;
    }
    this.pageIndex = 0;
    this.loadBookings();
  }

  onRowDoubleClicked(row: any): void {
    if (row) {
      this.router.navigate(['/bookings', row.bookingId]);
    }
  }

  onNewBooking(): void {
    this.router.navigate(['/bookings/new']);
  }

  onConvertToCase(booking: BookingDto): void {
    if (!booking.caseId) {
      this.snackBar.open('Booking is not linked to a case. Edit the booking and link it first.', 'Close', { duration: 4000 });
      return;
    }

    if (!confirm(`Convert booking #${booking.bookingId} to a case? This will change the linked case status from Booking to Case.`)) {
      return;
    }

    this.bookingService.convertToCase(booking.bookingId).subscribe({
      next: () => {
        this.snackBar.open('Booking successfully converted to case', 'Close', { duration: 3000 });
        this.loadBookings();
      },
      error: (err) => {
        const message = err.error?.message || 'Failed to convert booking to case';
        this.snackBar.open(message, 'Close', { duration: 4000 });
      }
    });
  }
}
