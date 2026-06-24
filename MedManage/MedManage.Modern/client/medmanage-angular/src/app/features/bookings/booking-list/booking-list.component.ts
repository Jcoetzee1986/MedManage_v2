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
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef, GridReadyEvent, GridApi } from 'ag-grid-community';
import { BookingService } from '../services/booking.service';
import { BookingDto, BookingSearchFilters } from '../models/booking.models';

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
    MatPaginatorModule,
    AgGridModule
  ],
  templateUrl: './booking-list.component.html',
  styleUrls: ['./booking-list.component.scss']
})
export class BookingListComponent implements OnInit {
  private readonly bookingService = inject(BookingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  private gridApi!: GridApi;
  rowData: BookingDto[] = [];
  totalCount = 0;
  pageSize = 25;
  pageIndex = 0;
  loading = false;

  searchForm = this.fb.group({
    memberNumber: [''],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  columnDefs: ColDef[] = [
    { field: 'bookingId', headerName: 'ID', width: 80, sortable: true },
    {
      field: 'travelDate',
      headerName: 'Travel Date',
      width: 130,
      sortable: true,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    },
    {
      field: 'travelTime',
      headerName: 'Time',
      width: 100
    },
    {
      field: 'appointmentDate',
      headerName: 'Appointment',
      width: 130,
      sortable: true,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    },
    { field: 'discipline', headerName: 'Discipline', flex: 1, sortable: true },
    { field: 'hospital', headerName: 'Hospital', flex: 1, sortable: true },
    {
      field: 'consultation',
      headerName: 'Consult',
      width: 90,
      valueFormatter: p => p.value ? 'Yes' : 'No'
    },
    {
      field: 'admission',
      headerName: 'Admission',
      width: 100,
      valueFormatter: p => p.value ? 'Yes' : 'No'
    },
    {
      field: 'arrived',
      headerName: 'Arrived',
      width: 90,
      valueFormatter: p => p.value ? 'Yes' : 'No'
    },
    { field: 'caseId', headerName: 'Case ID', width: 100 },
    {
      headerName: 'Actions',
      width: 120,
      cellRenderer: () => `<button class="convert-btn" title="Convert to Case">→ Case</button>`,
      onCellClicked: (params: any) => {
        if (params.event?.target?.classList.contains('convert-btn')) {
          this.onConvertToCase(params.data);
        }
      }
    }
  ];

  defaultColDef: ColDef = {
    resizable: true,
    filter: false
  };

  ngOnInit(): void {
    this.loadBookings();
  }

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
  }

  loadBookings(): void {
    this.loading = true;
    const formValue = this.searchForm.value;

    const filters: BookingSearchFilters = {
      memberNumber: formValue.memberNumber || undefined,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize
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

  onRowDoubleClicked(event: any): void {
    if (event.data) {
      this.router.navigate(['/bookings', event.data.bookingId]);
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
