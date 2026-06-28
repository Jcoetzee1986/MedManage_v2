import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatSortModule, Sort } from '@angular/material/sort';
import { TariffService } from '../services/tariff.service';
import {
  BaseTariffDto, CreateBaseTariffRequest,
  TariffRateDto, CreateTariffRateRequest,
  TariffNameDto, CreateTariffNameRequest
} from '../models/tariff.models';

@Component({
  selector: 'app-tariff-admin',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatSortModule
  ],
  templateUrl: './tariff-admin.component.html',
  styleUrls: ['./tariff-admin.component.scss']
})
export class TariffAdminComponent implements OnInit {
  private readonly tariffService = inject(TariffService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  // ─── Base Tariffs ────────────────────────────────────────────
  baseTariffs: BaseTariffDto[] = [];
  baseTariffColumns = ['code', 'description', 'speciality', 'category', 'isActive', 'actions'];
  showBaseForm = false;
  editingBaseId: number | null = null;

  baseForm = this.fb.group({
    code: ['', Validators.required],
    description: ['', Validators.required],
    category: [''],
    speciality: [''],
    isActive: [true]
  });

  // ─── Tariff Rates ───────────────────────────────────────────
  rates: TariffRateDto[] = [];
  rateColumns = ['tariffCode', 'value', 'dateFrom', 'dateTo', 'rateType', 'actions'];
  showRateForm = false;
  editingRateId: number | null = null;

  rateForm = this.fb.group({
    tariffId: [null as number | null, Validators.required],
    value: [null as number | null, [Validators.required, Validators.min(0)]],
    dateFrom: ['', Validators.required],
    dateTo: [''],
    rateType: ['']
  });

  // ─── Tariff Names ───────────────────────────────────────────
  names: TariffNameDto[] = [];
  nameColumns = ['tariffCode', 'name', 'language', 'actions'];
  showNameForm = false;
  editingNameId: number | null = null;

  nameForm = this.fb.group({
    tariffId: [null as number | null, Validators.required],
    name: ['', Validators.required],
    language: ['']
  });

  ngOnInit(): void {
    this.loadBaseTariffs();
    this.loadRates();
    this.loadNames();
  }

  // ─── Base Tariff Methods ─────────────────────────────────────

  loadBaseTariffs(): void {
    this.tariffService.getBaseTariffs().subscribe({
      next: (data) => this.baseTariffs = data,
      error: () => this.snackBar.open('Failed to load base tariffs', 'Close', { duration: 3000 })
    });
  }

  onAddBase(): void {
    this.showBaseForm = true;
    this.editingBaseId = null;
    this.baseForm.reset({ isActive: true, speciality: '' });
  }

  onEditBase(item: BaseTariffDto): void {
    this.showBaseForm = true;
    this.editingBaseId = item.id;
    this.baseForm.patchValue({
      code: item.code,
      description: item.description,
      category: item.category || '',
      speciality: (item as any).speciality || '',
      isActive: item.isActive
    });
  }

  onCancelBase(): void {
    this.showBaseForm = false;
    this.editingBaseId = null;
    this.baseForm.reset({ isActive: true, speciality: '' });
  }

  onSaveBase(): void {
    if (this.baseForm.invalid) return;

    const formValue = this.baseForm.value;
    const request: CreateBaseTariffRequest = {
      code: formValue.code!,
      description: formValue.description!,
      category: formValue.category || null,
      speciality: formValue.speciality || null,
      isActive: formValue.isActive ?? true
    };

    const obs = this.editingBaseId
      ? this.tariffService.updateBaseTariff(this.editingBaseId, request)
      : this.tariffService.createBaseTariff(request);

    obs.subscribe({
      next: () => {
        this.showBaseForm = false;
        this.editingBaseId = null;
        this.baseForm.reset({ isActive: true });
        this.loadBaseTariffs();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDeleteBase(item: BaseTariffDto): void {
    if (confirm(`Are you sure you want to delete tariff "${item.code}"?`)) {
      this.tariffService.deleteBaseTariff(item.id).subscribe({
        next: () => {
          this.loadBaseTariffs();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }

  // ─── Rate Methods ────────────────────────────────────────────

  loadRates(): void {
    this.tariffService.getRates().subscribe({
      next: (data) => this.rates = data,
      error: () => this.snackBar.open('Failed to load rates', 'Close', { duration: 3000 })
    });
  }

  onAddRate(): void {
    this.showRateForm = true;
    this.editingRateId = null;
    this.rateForm.reset();
  }

  onEditRate(item: TariffRateDto): void {
    this.showRateForm = true;
    this.editingRateId = item.id;
    this.rateForm.patchValue({
      tariffId: item.tariffId,
      value: item.value,
      dateFrom: item.dateFrom,
      dateTo: item.dateTo || '',
      rateType: item.rateType || ''
    });
  }

  onCancelRate(): void {
    this.showRateForm = false;
    this.editingRateId = null;
    this.rateForm.reset();
  }

  onSaveRate(): void {
    if (this.rateForm.invalid) return;

    const formValue = this.rateForm.value;
    const request: CreateTariffRateRequest = {
      tariffId: formValue.tariffId!,
      value: formValue.value!,
      dateFrom: formValue.dateFrom!,
      dateTo: formValue.dateTo || null,
      rateType: formValue.rateType || null
    };

    const obs = this.editingRateId
      ? this.tariffService.updateRate(this.editingRateId, request)
      : this.tariffService.createRate(request);

    obs.subscribe({
      next: () => {
        this.showRateForm = false;
        this.editingRateId = null;
        this.rateForm.reset();
        this.loadRates();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDeleteRate(item: TariffRateDto): void {
    if (confirm('Are you sure you want to delete this rate?')) {
      this.tariffService.deleteRate(item.id).subscribe({
        next: () => {
          this.loadRates();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }

  // ─── Name Methods ────────────────────────────────────────────

  loadNames(): void {
    this.tariffService.getNames().subscribe({
      next: (data) => this.names = data,
      error: () => this.snackBar.open('Failed to load names', 'Close', { duration: 3000 })
    });
  }

  onAddName(): void {
    this.showNameForm = true;
    this.editingNameId = null;
    this.nameForm.reset();
  }

  onEditName(item: TariffNameDto): void {
    this.showNameForm = true;
    this.editingNameId = item.id;
    this.nameForm.patchValue({
      tariffId: item.tariffId,
      name: item.name,
      language: item.language || ''
    });
  }

  onCancelName(): void {
    this.showNameForm = false;
    this.editingNameId = null;
    this.nameForm.reset();
  }

  onSaveName(): void {
    if (this.nameForm.invalid) return;

    const formValue = this.nameForm.value;
    const request: CreateTariffNameRequest = {
      tariffId: formValue.tariffId!,
      name: formValue.name!,
      language: formValue.language || null
    };

    const obs = this.editingNameId
      ? this.tariffService.updateName(this.editingNameId, request)
      : this.tariffService.createName(request);

    obs.subscribe({
      next: () => {
        this.showNameForm = false;
        this.editingNameId = null;
        this.nameForm.reset();
        this.loadNames();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDeleteName(item: TariffNameDto): void {
    if (confirm('Are you sure you want to delete this name?')) {
      this.tariffService.deleteName(item.id).subscribe({
        next: () => {
          this.loadNames();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
