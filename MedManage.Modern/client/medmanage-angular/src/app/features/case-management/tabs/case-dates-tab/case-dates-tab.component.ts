import { Component, Input, inject, OnInit, OnDestroy } from '@angular/core';
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
import { Subject, takeUntil, merge } from 'rxjs';
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
export class CaseDatesTabComponent implements OnInit, OnDestroy {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;

  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly referenceDataService = inject(ReferenceDataService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  facilityTypes: ReferenceDataItem[] = [];
  facilityTypeRecords: CaseFacilityTypeDto[] = [];
  displayedColumns = ['facilityType', 'dateAdmitted', 'dateDischarged', 'los', 'minutesOnVentilator', 'comments', 'actions'];
  showForm = false;
  editingId: number | null = null;
  private skipLosCalc = false;

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
    this.setupLosCalculation();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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

  /**
   * Auto-calculate LOS when dates or times change.
   * Legacy logic:
   * 1. Days = floor(discharged_date - admitted_date) in whole days
   * 2. If admission hour < 12 → add 1 day (full first day)
   *    If admission hour >= 12 → add 0.5 day (half first day)
   * 3. If discharge hour < 12 → subtract 0.5 day (left before noon)
   */
  private setupLosCalculation(): void {
    merge(
      this.form.controls.dateAdmitted.valueChanges,
      this.form.controls.dateDischarged.valueChanges,
      this.form.controls.timeAdmitted.valueChanges,
      this.form.controls.timeDischarged.valueChanges
    ).pipe(takeUntil(this.destroy$)).subscribe(() => {
      this.calculateLos();
    });
  }

  /** Called from template when date or time inputs change */
  onDateChange(): void {
    this.calculateLos();
  }

  private calculateLos(): void {
    if (this.skipLosCalc) return;

    let admitted = this.form.value.dateAdmitted;
    let discharged = this.form.value.dateDischarged;
    const timeAdmitted = this.form.value.timeAdmitted || '12:00';
    const timeDischarged = this.form.value.timeDischarged || '12:00';

    if (!admitted || !discharged) return;

    // Ensure we have Date objects (may be strings from patchValue)
    if (typeof admitted === 'string') admitted = new Date(admitted);
    if (typeof discharged === 'string') discharged = new Date(discharged);

    if (isNaN(admitted.getTime()) || isNaN(discharged.getTime())) return;

    // Calculate difference in full days (date portion only)
    const admDate = new Date(admitted.getFullYear(), admitted.getMonth(), admitted.getDate());
    const disDate = new Date(discharged.getFullYear(), discharged.getMonth(), discharged.getDate());
    const diffMs = disDate.getTime() - admDate.getTime();
    if (diffMs < 0) return;

    let days = Math.floor(diffMs / (1000 * 60 * 60 * 24));

    // Parse admission time
    const admHour = parseInt(timeAdmitted.split(':')[0], 10);
    // Parse discharge time
    const disHour = parseInt(timeDischarged.split(':')[0], 10);

    // Admission time adjustment
    if (admHour < 12) {
      days += 1;      // Full first day
    } else {
      days += 0.5;    // Half first day
    }

    // Discharge time adjustment
    if (disHour < 12) {
      days -= 0.5;    // Left before noon
    }

    // Minimum 0.5
    if (days < 0.5) days = 0.5;

    this.form.patchValue({ los: days }, { emitEvent: false });
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

    // Extract time from datetime strings if available
    const admTime = this.extractTime(record.dateAdmitted);
    const disTime = this.extractTime(record.dateDischarged);

    this.skipLosCalc = true;
    this.form.patchValue({
      facilityTypeId: record.facilityTypeId,
      dateAdmitted: record.dateAdmitted ? new Date(record.dateAdmitted) : null,
      timeAdmitted: admTime,
      dateDischarged: record.dateDischarged ? new Date(record.dateDischarged) : null,
      timeDischarged: disTime,
      los: record.los,
      minutesOnVentilator: record.minutesOnVentilator,
      comments: record.comments
    });
    this.skipLosCalc = false;
  }

  private extractTime(dateStr: string | null | undefined): string {
    if (!dateStr) return '12:00';
    // Try to extract time from ISO format or datetime string
    const match = dateStr.match(/T(\d{2}:\d{2})/);
    if (match) return match[1];
    // Try "yyyy-MM-dd HH:mm" format
    const spaceMatch = dateStr.match(/\s(\d{2}:\d{2})/);
    if (spaceMatch) return spaceMatch[1];
    return '12:00';
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const f = this.form.value;
    const timeAdm = f.timeAdmitted || '12:00';
    const timeDis = f.timeDischarged || '12:00';

    // Build full datetime strings with time
    const dateAdmStr = f.dateAdmitted
      ? `${f.dateAdmitted.getFullYear()}-${String(f.dateAdmitted.getMonth() + 1).padStart(2, '0')}-${String(f.dateAdmitted.getDate()).padStart(2, '0')}T${timeAdm}:00`
      : undefined;
    const dateDisStr = f.dateDischarged
      ? `${f.dateDischarged.getFullYear()}-${String(f.dateDischarged.getMonth() + 1).padStart(2, '0')}-${String(f.dateDischarged.getDate()).padStart(2, '0')}T${timeDis}:00`
      : undefined;

    const request: CreateCaseFacilityTypeRequest = {
      facilityTypeId: f.facilityTypeId!,
      dateAdmitted: dateAdmStr,
      dateDischarged: dateDisStr,
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
        this.snackBar.open('Treatment date saved', 'Close', { duration: 3000 });

        // Reload records, then check overall dates
        this.caseService.getFacilityTypes(this.caseId).subscribe({
          next: (records) => {
            this.facilityTypeRecords = records;
            this.checkOverallDates();
          }
        });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  /**
   * After saving a record, compare individual record dates against overall case dates.
   * If a discharge is later or admission is earlier, prompt user to update.
   */
  private checkOverallDates(): void {
    if (!this.caseData) return;

    const currentOverallAdm = this.caseData.admissionDate ? new Date(this.caseData.admissionDate) : null;
    const currentOverallDis = this.caseData.dischargeDate ? new Date(this.caseData.dischargeDate) : null;

    // Compute the min admission and max discharge from all records
    let minAdm: Date | null = null;
    let maxDis: Date | null = null;

    for (const rec of this.facilityTypeRecords) {
      if (rec.dateAdmitted) {
        const d = new Date(rec.dateAdmitted);
        if (!minAdm || d < minAdm) minAdm = d;
      }
      if (rec.dateDischarged) {
        const d = new Date(rec.dateDischarged);
        if (!maxDis || d > maxDis) maxDis = d;
      }
    }

    // Check discharge date
    if (maxDis && currentOverallDis && maxDis > currentOverallDis) {
      if (confirm('Warning: One of the discharge dates is later than the overall discharge date.\nDo you want to update the discharge date?')) {
        this.updateOverallDate('discharge', maxDis);
      }
    } else if (maxDis && !currentOverallDis) {
      if (confirm('Warning: A discharge date has been set but there is no overall discharge date.\nDo you want to update the discharge date?')) {
        this.updateOverallDate('discharge', maxDis);
      }
    }

    // Check admission date
    if (minAdm && currentOverallAdm && minAdm < currentOverallAdm) {
      if (confirm('Warning: One of the admission dates is earlier than the overall admission date.\nDo you want to update the admission date?')) {
        this.updateOverallDate('admission', minAdm);
      }
    } else if (minAdm && !currentOverallAdm) {
      if (confirm('Warning: An admission date has been set but there is no overall admission date.\nDo you want to update the admission date?')) {
        this.updateOverallDate('admission', minAdm);
      }
    }
  }

  private updateOverallDate(type: 'admission' | 'discharge', date: Date): void {
    const dateStr = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`;

    const fields: Record<string, any> = {};
    if (type === 'admission') {
      fields['admissionDate'] = dateStr;
    } else {
      fields['dischargeDate'] = dateStr;
    }

    this.caseService.patch(this.caseId, fields).subscribe({
      next: () => {
        if (type === 'admission') {
          this.caseData.admissionDate = dateStr;
        } else {
          this.caseData.dischargeDate = dateStr;
        }
        this.snackBar.open(`Overall ${type} date updated`, 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open(`Failed to update overall ${type} date`, 'Close', { duration: 3000 })
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
