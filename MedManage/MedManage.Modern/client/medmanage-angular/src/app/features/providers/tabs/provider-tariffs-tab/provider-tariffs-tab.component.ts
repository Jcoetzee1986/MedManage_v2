import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProviderService } from '../../services/provider.service';
import { ProviderTariffDto, CreateProviderTariffRequest } from '../../models/provider.models';

@Component({
  selector: 'app-provider-tariffs-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './provider-tariffs-tab.component.html',
  styleUrls: ['./provider-tariffs-tab.component.scss']
})
export class ProviderTariffsTabComponent implements OnInit {
  @Input() providerId!: number;

  private readonly providerService = inject(ProviderService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: ProviderTariffDto[] = [];
  displayedColumns = ['tariffCode', 'tariffName', 'dateFrom', 'dateTo', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    tariffId: [null as number | null, Validators.required],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.providerService.getTariffs(this.providerId).subscribe({
      next: (data) => this.items = data,
      error: () => this.snackBar.open('Failed to load tariff assignments', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.form.reset();
  }

  onEdit(item: ProviderTariffDto): void {
    this.showForm = true;
    this.editingId = item.id;
    this.form.patchValue({
      tariffId: item.tariffId,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null
    });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const formValue = this.form.value;
    const request: CreateProviderTariffRequest = {
      tariffId: formValue.tariffId!,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : null,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : null
    };

    const obs = this.editingId
      ? this.providerService.updateTariff(this.providerId, this.editingId, request)
      : this.providerService.createTariff(this.providerId, request);

    obs.subscribe({
      next: () => {
        this.showForm = false;
        this.editingId = null;
        this.form.reset();
        this.loadItems();
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: ProviderTariffDto): void {
    if (confirm('Are you sure you want to remove this tariff assignment?')) {
      this.providerService.deleteTariff(this.providerId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
