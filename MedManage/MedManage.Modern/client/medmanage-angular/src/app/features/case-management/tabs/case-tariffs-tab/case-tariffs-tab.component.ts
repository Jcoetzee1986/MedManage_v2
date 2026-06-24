import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseTariffDto, CreateCaseTariffRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-tariffs-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './case-tariffs-tab.component.html',
  styleUrls: ['./case-tariffs-tab.component.scss']
})
export class CaseTariffsTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseTariffDto[] = [];
  displayedColumns = ['tariffCode', 'tariffDescription', 'value', 'qty', 'dateOfProcedure', 'rejected', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    tariffId: [null as number | null],
    value: [null as number | null],
    qty: [1],
    agreedRateOverride: [null as number | null],
    valueIsTotal: [false],
    rejected: [false],
    dateOfProcedure: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getTariffs(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load tariffs', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseTariffRequest = {
      tariffId: val.tariffId || undefined,
      value: val.value || undefined,
      qty: val.qty || 1,
      agreedRateOverride: val.agreedRateOverride || undefined,
      valueIsTotal: val.valueIsTotal || false,
      rejected: val.rejected || false,
      dateOfProcedure: val.dateOfProcedure?.toISOString()
    };

    this.caseService.createTariff(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset({ qty: 1 });
        this.showAddForm = false;
        this.snackBar.open('Tariff added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add tariff', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseTariffDto): void {
    if (confirm('Delete this tariff?')) {
      this.caseService.deleteTariff(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Tariff deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete tariff', 'Close', { duration: 3000 })
      });
    }
  }
}
