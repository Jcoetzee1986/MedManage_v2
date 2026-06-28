import { Component, inject, OnInit, OnDestroy } from '@angular/core';
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
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { Subject, takeUntil } from 'rxjs';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { HasRoleDirective } from '../../../shared/directives/has-role.directive';
import { CaseService } from '../services/case.service';
import { CaseDto, CaseSearchRequest } from '../models/case.models';
import { CaseCopyDialogComponent, CaseCopyDialogData } from '../case-copy-dialog/case-copy-dialog.component';
import { MedicalAidService } from '../../medical-aids/services/medical-aid.service';
import { AuthService } from '../../../core/services/auth.service';
import { DataTableComponent, DataTableColumn } from '../../../shared/components/data-table/data-table.component';

interface MedicalAidOption {
  id: number;
  name: string;
}

@Component({
  selector: 'app-case-list',
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
    MatSelectModule,
    MatToolbarModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatPaginatorModule,
    ReferenceDataDropdownComponent,
    HasRoleDirective,
    DataTableComponent
  ],
  templateUrl: './case-list.component.html',
  styleUrls: ['./case-list.component.scss']
})
export class CaseListComponent implements OnInit, OnDestroy {
  private readonly caseService = inject(CaseService);
  private readonly medicalAidService = inject(MedicalAidService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly destroy$ = new Subject<void>();

  /** Whether we're showing "My Cases" (initial load) or search results */
  isMyView = true;

  /** Currently selected row */
  selectedCase: CaseDto | null = null;

  /** Medical aid options for the dropdown */
  medicalAids: MedicalAidOption[] = [];

  /** Loading indicator for exports */
  exporting = false;

  /** Pagination state */
  totalCount = 0;
  pageSize = 30;
  pageIndex = 0;

  /** Sort state */
  currentSortBy?: string;
  currentSortDescending?: boolean;

  /** Search filter form — matches legacy search fields */
  searchForm = this.fb.group({
    authNumber: [''],
    memberNumber: [''],
    surname: [''],
    name: [''],
    practiceName: [''],
    dateCreatedEnabled: [false],
    dateCreatedFrom: [null as Date | null],
    dateCreatedTo: [null as Date | null],
    medicalAidEnabled: [false],
    medicalAidId: [null as number | null],
    statusId: [null as number | null],
    authTypeId: [null as number | null]
  });

  /** Table columns for mat-table */
  tableColumns = ['caseStatus', 'authNumber', 'memberNumber', 'surname', 'name', 'referTo', 'admissionDate', 'dischargeDate', 'caseTypeName', 'dateCreated'];

  /** Column definitions for the reusable data-table */
  tableColumnDefs: DataTableColumn[] = [
    { field: 'caseStatusName', header: 'Status', width: '100px', fallbacks: ['caseStatus'] },
    { field: 'authNumber', header: 'Auth/Case #', width: '120px' },
    { field: 'memberNumber', header: 'Member #', width: '110px' },
    { field: 'memberSurname', header: 'Surname', fallbacks: ['surname'] },
    { field: 'memberName', header: 'Name', fallbacks: ['name'] },
    { field: 'referToPracticeName', header: 'Refer To', fallbacks: ['referToName', 'referTo'] },
    { field: 'admissionDate', header: 'Admitted', width: '100px', pipe: 'date', fallbacks: ['dateAdmitted'] },
    { field: 'dischargeDate', header: 'Discharged', width: '100px', pipe: 'date', fallbacks: ['dateDischarged'] },
    { field: 'caseTypeName', header: 'Type', width: '100px' },
    { field: 'dateCreated', header: 'Created', width: '100px', pipe: 'date' }
  ];

  /** Row data */
  rowData: CaseDto[] = [];

  ngOnInit(): void {
    this.loadMedicalAids();
    this.loadMyCases();

    // Reload medical aids when the active client changes
    this.authService.activeClient$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.loadMedicalAids();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /** Load current user's cases */
  private loadMyCases(): void {
    this.caseService.getMyCases()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (cases) => {
          this.rowData = cases;
        },
        error: () => {
          this.rowData = [];
        }
      });
  }

  /** Load medical aids for the dropdown filter */
  private loadMedicalAids(): void {
    const mainClientId = this.authService.activeClientId ?? undefined;

    this.medicalAidService.getActive(mainClientId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (aids) => {
          this.medicalAids = aids.map(a => ({
            id: a.medicalAidId,
            name: a.medicalAidName || `Medical Aid ${a.medicalAidId}`
          }));
        },
        error: () => {
          this.medicalAids = [];
        }
      });
  }


  /** Build the search request from form values */
  private buildSearchRequest(
    pageNumber: number,
    pageSize: number,
    sortBy?: string,
    sortDescending?: boolean
  ): CaseSearchRequest {
    const formValue = this.searchForm.value;

    const request: CaseSearchRequest = {
      pageNumber,
      pageSize,
      sortBy,
      sortDescending
    };

    if (formValue.authNumber) {
      request.authNumber = formValue.authNumber;
    }
    if (formValue.memberNumber) {
      request.memberNumber = formValue.memberNumber;
    }
    if (formValue.surname) {
      request.memberSurname = formValue.surname;
    }
    if (formValue.name) {
      request.memberName = formValue.name;
    }
    if (formValue.practiceName) {
      request.practiceName = formValue.practiceName;
    }
    if (formValue.dateCreatedEnabled && formValue.dateCreatedFrom) {
      request.dateCreatedFrom = this.formatDate(formValue.dateCreatedFrom);
    }
    if (formValue.dateCreatedEnabled && formValue.dateCreatedTo) {
      request.dateCreatedTo = this.formatDate(formValue.dateCreatedTo);
    }
    if (formValue.medicalAidEnabled && formValue.medicalAidId) {
      request.medicalAidId = formValue.medicalAidId;
    }
    if (formValue.statusId) {
      request.statusId = formValue.statusId;
    }
    if (formValue.authTypeId) {
      request.authTypeId = formValue.authTypeId;
    }

    return request;
  }

  /** Format a Date to YYYY-MM-DD string for the API */
  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  /** Execute search with current filters */
  onSearch(resetPage = true): void {
    if (resetPage) {
      this.pageIndex = 0;
    }
    this.isMyView = false;
    const searchRequest = this.buildSearchRequest(this.pageIndex + 1, this.pageSize, this.currentSortBy, this.currentSortDescending);

    this.caseService.search(searchRequest)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          this.rowData = result.items;
          this.totalCount = result.totalCount;
        },
        error: () => {
          this.rowData = [];
          this.totalCount = 0;
        }
      });
  }

  /** Handle paginator page change */
  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.onSearch(false);
  }

  /** Handle sort change from data table */
  onSortChange(sort: Sort): void {
    if (sort.direction) {
      this.currentSortBy = sort.active;
      this.currentSortDescending = sort.direction === 'desc';
    } else {
      this.currentSortBy = undefined;
      this.currentSortDescending = undefined;
    }

    if (this.isMyView) {
      // Client-side sort for "My Cases" view (all data is already loaded)
      if (!sort.active || !sort.direction) return;
      const dir = sort.direction === 'asc' ? 1 : -1;
      this.rowData = [...this.rowData].sort((a: any, b: any) => {
        const valA = (a[sort.active] ?? '').toString().toLowerCase();
        const valB = (b[sort.active] ?? '').toString().toLowerCase();
        return valA.localeCompare(valB) * dir;
      });
    } else {
      // Server-side sort for search results
      this.onSearch(true);
    }
  }

  /** Reset search form and return to "My Cases" view */
  onReset(): void {
    this.searchForm.reset();
    this.isMyView = true;
    this.selectedCase = null;
    this.pageIndex = 0;
    this.totalCount = 0;
    this.currentSortBy = undefined;
    this.currentSortDescending = undefined;
    this.loadMyCases();
  }

  /** Navigate to case detail on double-click (mat-table) */
  onRowDoubleClicked2(row: CaseDto): void {
    const caseId = row.caseId || row.id;
    this.router.navigate(['/cases', caseId]);
  }

  /** Open/Edit the selected case */
  onOpenCase(): void {
    if (!this.selectedCase) {
      this.snackBar.open('Please select a case first.', 'OK', { duration: 3000 });
      return;
    }
    const caseId = this.selectedCase.caseId || this.selectedCase.id;
    this.router.navigate(['/cases', caseId]);
  }

  /** Navigate to create new case */
  onNewCase(): void {
    this.router.navigate(['/cases/new']);
  }

  /** Open copy dialog for the selected case */
  onCopyCase(): void {
    if (!this.selectedCase) {
      this.snackBar.open('Please select a case to copy.', 'OK', { duration: 3000 });
      return;
    }

    const caseId = this.selectedCase.caseId || this.selectedCase.id;
    const dialogData: CaseCopyDialogData = {
      caseId,
      caseNumber: this.selectedCase.authNumber
    };

    const dialogRef = this.dialog.open(CaseCopyDialogComponent, {
      width: '500px',
      data: dialogData
    });

    dialogRef.afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        if (result) {
          this.caseService.copyCase(caseId, result)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
              next: (newCase) => {
                this.snackBar.open(
                  `Case copied successfully. New case: ${newCase.authNumber || newCase.caseId || newCase.id}`,
                  'Open',
                  { duration: 5000 }
                ).onAction().subscribe(() => {
                  const newId = newCase.caseId || newCase.id;
                  this.router.navigate(['/cases', newId]);
                });
                // Refresh list
                this.onSearch();
              },
              error: () => {
                this.snackBar.open('Failed to copy case.', 'OK', { duration: 3000 });
              }
            });
        }
      });
  }

  /** Export current data to CSV (simple) */
  onExportExcel(): void {
    if (this.rowData.length === 0) return;

    // Use column definitions to resolve field values with fallbacks (same as table rendering)
    const headers = this.tableColumnDefs.map(c => c.header).join(',');
    const rows = this.rowData.map(r => {
      return this.tableColumnDefs.map(col => {
        let value = (r as any)[col.field];
        if ((value === null || value === undefined || value === '') && col.fallbacks) {
          for (const fb of col.fallbacks) {
            value = (r as any)[fb];
            if (value !== null && value !== undefined && value !== '') break;
          }
        }
        return `"${((value ?? '').toString().replace(/"/g, '""'))}"`;
      }).join(',');
    });

    const csv = ['sep=,', headers, ...rows].join('\r\n');
    const bom = '\uFEFF';
    const blob = new Blob([bom + csv], { type: 'text/csv;charset=utf-8' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `cases-export-${new Date().toISOString().split('T')[0]}.csv`;
    a.click();
    URL.revokeObjectURL(url);
  }

  /** Generate PDF/Excel report via server-side API */
  onPrintReport(): void {
    this.exporting = true;
    const request = this.buildSearchRequest(1, 10000);

    this.caseService.exportReport(request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (blob) => {
          this.exporting = false;
          const url = window.URL.createObjectURL(blob);
          const link = document.createElement('a');
          link.href = url;
          link.download = `cases-report-${new Date().toISOString().split('T')[0]}.pdf`;
          link.click();
          window.URL.revokeObjectURL(url);
        },
        error: () => {
          this.exporting = false;
          this.snackBar.open('Failed to generate report.', 'OK', { duration: 3000 });
        }
      });
  }
}
