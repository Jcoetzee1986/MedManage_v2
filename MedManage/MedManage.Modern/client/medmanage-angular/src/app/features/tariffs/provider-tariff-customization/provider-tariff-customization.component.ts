import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { TariffService } from '../services/tariff.service';
import {
  ProviderTariffAssignmentDto, CreateProviderTariffAssignmentRequest,
  ProviderCustomTariffDto, CreateProviderCustomTariffRequest,
  TariffLookupResult
} from '../models/tariff.models';
import { TariffLookupComponent } from '../../../shared/components/tariff-lookup/tariff-lookup.component';

@Component({
  selector: 'app-provider-tariff-customization',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    MatTabsModule,
    MatDialogModule
  ],
  templateUrl: './provider-tariff-customization.component.html',
  styleUrls: ['./provider-tariff-customization.component.scss']
})
export class ProviderTariffCustomizationComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly tariffService = inject(TariffService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  providerId!: number;

  // ─── Tariff Assignments ──────────────────────────────────────
  assignments: ProviderTariffAssignmentDto[] = [];
  assignmentColumns = ['tariffCode', 'tariffDescription', 'dateFrom', 'dateTo', 'actions'];
  showAssignmentForm = false;
  editingAssignmentId: number | null = null;

  assignmentForm = this.fb.group({
    tariffId: [null as number | null, Validators.required],
    tariffDisplay: [''],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  // ─── Custom Tariffs ──────────────────────────────────────────
  customTariffs: ProviderCustomTariffDto[] = [];
  customTariffColumns = ['tariffCode', 'tariffDescription', 'customValue', 'dateFrom', 'dateTo', 'actions'];
  showCustomForm = false;
  editingCustomId: number | null = null;

  customForm = this.fb.group({
    tariffId: [null as number | null, Validators.required],
    tariffDisplay: [''],
    customValue: [null as number | null, [Validators.required, Validators.min(0)]],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  ngOnInit(): void {
    this.providerId = Number(this.route.snapshot.paramMap.get('providerId'));
    this.loadAssignments();
    this.loadCustomTariffs();
  }

  // ─── Tariff Assignment Methods ───────────────────────────────

  loadAssignments(): void {
    this.tariffService.getProviderTariffs(this.providerId).subscribe({
      next: (data) => this.assignments = data,
      error: () => this.snackBar.open('Failed to load tariff assignments', 'Close', { duration: 3000 })
    });
  }

  onAddAssignment(): void {
    this.showAssignmentForm = true;
    this.editingAssignmentId = null;
    this.assignmentForm.reset();
  }

  onEditAssignment(item: ProviderTariffAssignmentDto): void {
    this.showAssignmentForm = true;
    this.editingAssignmentId = item.id;
    this.assignmentForm.patchValue({
      tariffId: item.tariffId,
      tariffDisplay: `${item.tariffCode} - ${item.tariffDescription}`,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null
    });
  }

  onCancelAssignment(): void {
    this.showAssignmentForm = false;
    this.editingAssignmentId = null;
    this.assignmentForm.reset();
  }

  onLookupTariffForAssignment(): void {
    const dialogRef = this.dialog.open(TariffLookupComponent, {
      width: '650px',
      data: { title: 'Select Tariff for Assignment' }
    });

    dialogRef.afterClosed().subscribe((result: TariffLookupResult | null) => {
      if (result) {
        this.assignmentForm.patchValue({
          tariffId: result.id,
          tariffDisplay: `${result.code} - ${result.description}`
        });
      }
    });
  }

  onSaveAssignment(): void {
    if (this.assignmentForm.invalid) return;

    const formValue = this.assignmentForm.value;
    const request: CreateProviderTariffAssignmentRequest = {
      tariffId: formValue.tariffId!,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : null,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : null
    };

    const obs = this.editingAssignmentId
      ? this.tariffService.updateProviderTariff(this.providerId, this.editingAssignmentId, request)
      : this.tariffService.createProviderTariff(this.providerId, request);

    obs.subscribe({
      next: () => {
        this.showAssignmentForm = false;
        this.editingAssignmentId = null;
        this.assignmentForm.reset();
        this.loadAssignments();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDeleteAssignment(item: ProviderTariffAssignmentDto): void {
    if (confirm('Are you sure you want to remove this tariff assignment?')) {
      this.tariffService.deleteProviderTariff(this.providerId, item.id).subscribe({
        next: () => {
          this.loadAssignments();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }

  // ─── Custom Tariff Methods ───────────────────────────────────

  loadCustomTariffs(): void {
    this.tariffService.getProviderCustomTariffs(this.providerId).subscribe({
      next: (data) => this.customTariffs = data,
      error: () => this.snackBar.open('Failed to load custom tariffs', 'Close', { duration: 3000 })
    });
  }

  onAddCustom(): void {
    this.showCustomForm = true;
    this.editingCustomId = null;
    this.customForm.reset();
  }

  onEditCustom(item: ProviderCustomTariffDto): void {
    this.showCustomForm = true;
    this.editingCustomId = item.id;
    this.customForm.patchValue({
      tariffId: item.tariffId,
      tariffDisplay: `${item.tariffCode} - ${item.tariffDescription}`,
      customValue: item.customValue,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null
    });
  }

  onCancelCustom(): void {
    this.showCustomForm = false;
    this.editingCustomId = null;
    this.customForm.reset();
  }

  onLookupTariffForCustom(): void {
    const dialogRef = this.dialog.open(TariffLookupComponent, {
      width: '650px',
      data: { title: 'Select Tariff for Custom Override' }
    });

    dialogRef.afterClosed().subscribe((result: TariffLookupResult | null) => {
      if (result) {
        this.customForm.patchValue({
          tariffId: result.id,
          tariffDisplay: `${result.code} - ${result.description}`
        });
      }
    });
  }

  onSaveCustom(): void {
    if (this.customForm.invalid) return;

    const formValue = this.customForm.value;
    const request: CreateProviderCustomTariffRequest = {
      tariffId: formValue.tariffId!,
      customValue: formValue.customValue!,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : null,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : null
    };

    const obs = this.editingCustomId
      ? this.tariffService.updateProviderCustomTariff(this.providerId, this.editingCustomId, request)
      : this.tariffService.createProviderCustomTariff(this.providerId, request);

    obs.subscribe({
      next: () => {
        this.showCustomForm = false;
        this.editingCustomId = null;
        this.customForm.reset();
        this.loadCustomTariffs();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDeleteCustom(item: ProviderCustomTariffDto): void {
    if (confirm('Are you sure you want to delete this custom tariff?')) {
      this.tariffService.deleteProviderCustomTariff(this.providerId, item.id).subscribe({
        next: () => {
          this.loadCustomTariffs();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
