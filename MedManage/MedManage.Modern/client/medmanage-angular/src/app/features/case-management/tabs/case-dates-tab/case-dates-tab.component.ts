import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatDividerModule } from '@angular/material/divider';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseDto, CaseFacilityTypeDto, CreateCaseFacilityTypeRequest } from '../../models/case.models';
import { ReferenceDataService } from '../../../../core/services/reference-data.service';
import { ReferenceDataItem } from '../../../../core/models/reference-data.models';

@Component({
  selector: 'app-case-dates-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatDividerModule,
    MatSnackBarModule
  ],
  templateUrl: './case-dates-tab.component.html',
  styleUrls: ['./case-dates-tab.component.scss']
})
export class CaseDatesTabComponent implements OnInit {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;

  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly referenceDataService = inject(ReferenceDataService);
  private readonly snackBar = inject(MatSnackBar);

  facilityTypes: ReferenceDataItem[] = [];
  facilityTypeRecords: CaseFacilityTypeDto[] = [];
  displayedColumns = ['facilityType', 'dateAdmitted', 'dateDischarged', 'los', 'minutesOnVentilator', 'comments', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    facilityTypeId: [null as number | null, Validators.required],
    dateAdmitted: [null as Date | null, Validators.required],
    timeAdmitted: ['12:00'],
    dateDischarged: [null as Date | null],
    timeDischarged: ['12:00'],
    los: [null as number | null],
    minutesOnVentilator: [null as number | null],
    comments: ['']
  });

  ngOnInit(): void {
    this.loadFacilityTypes();
    this.loadRecords();
  }

  private loadFacilityTypes(): void {
    this.referenceDataService.getAll('facility-type').subscribe(items => this.facilityTypes = items);
  }

  private loadRecords(): void {
    this.caseService.getFacilityTypes(this.caseId).subscribe({
      next: (records) => this.facilityTypeRecords = records,
      error: () => {}
    });
  }

  getFacilityTypeName(id: number | null): string {
    if (!id) return '—';
    return this.facilityTypes.find(f => f.id === id)?.name || `Type #${id}`;
  }

  // Computed overall dates from facility type records
  get overallAdmissionDate(): string | null {
    if (this.facilityTypeRecords.length === 0) return this.caseData?.admissionDate || null;
    const dates = this.facilityTypeRecords
      .map(r => r.dateAdmitted)
      .filter(d => !!d)
      .sort();
    return dates.length > 0 ? dates[0]! : null;
  }

  get overallDischargeDate(): string | null {
    if (this.facilityTypeRecords.length === 0) return this.caseData?.dischargeDate || null;
    const dates = this.facilityTypeRecords
      .map(r => r.dateDischarged)
      .filter(d => !!d)
      .sort();
    return dates.length > 0 ? dates[dates.length - 1]! : null;
  }

  get totalLos(): number {
    return this.facilityTypeRecords.reduce((sum, r) => sum + (r.los || 0), 0);
  }

  // ─── Add / Edit ─────────────────────────────────────────────

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.form.reset({ timeAdmitted: '12:00', timeDischarged: '12:00' });
  }

  onEdit(record: CaseFacilityTypeDto): void {
    this.showForm = true;
    this.editingId = record.id;
    this.form.patchValue({
      facilityTypeId: record.facilityTypeId,
      dateAdmitted: record.dateAdmitted ? new Date(record.dateAdmitted) : null,
      timeAdmitted: '12:00',
      dateDischarged: record.dateDischarged ? new Date(record.dateDischarged) : null,
      timeDischarged: '12:00',
      los: record.los,
      minutesOnVentilator: record.minutesOnVentilator,
      comments: record.comments
    });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const f = this.form.value;
    const request: CreateCaseFacilityTypeRequest = {
      facilityTypeId: f.facilityTypeId!,
      dateAdmitted: f.dateAdmitted ? f.dateAdmitted.toISOString().split('T')[0] : undefined,
      dateDischarged: f.dateDischarged ? f.dateDischarged.toISOString().split('T')[0] : undefined,
      los: f.los ?? undefined,
      minutesOnVentilator: f.minutesOnVentilator ?? undefined,
      comments: f.comments || undefined
    };

    const obs = this.editingId
      ? this.caseService.updateFacilityType(this.caseId, this.editingId, request)
      : this.caseService.createFacilityType(this.caseId, request);

    obs.subscribe({
      next: () => {
        this.showForm = false;
        this.editingId = null;
        this.form.reset();
        this.loadRecords();
        this.snackBar.open('Treatment date saved', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDelete(record: CaseFacilityTypeDto): void {
    if (!confirm('Delete this treatment date record?')) return;
    this.caseService.deleteFacilityType(this.caseId, record.id).subscribe({
      next: () => {
        this.loadRecords();
        this.snackBar.open('Record deleted', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
    });
  }
}
