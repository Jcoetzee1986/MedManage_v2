import { Component, Input, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { Subject } from 'rxjs';
import { CaseService } from '../../services/case.service';
import { CaseTariffDto, CreateCaseTariffRequest } from '../../models/case.models';
import { TariffService } from '../../../tariffs/services/tariff.service';
import { TariffLookupResult } from '../../../tariffs/models/tariff.models';
import { TariffLookupComponent } from '../../../../shared/components/tariff-lookup/tariff-lookup.component';

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
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule
  ],
  templateUrl: './case-tariffs-tab.component.html',
  styleUrls: ['./case-tariffs-tab.component.scss']
})
export class CaseTariffsTabComponent implements OnInit, OnDestroy {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly tariffService = inject(TariffService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly destroy$ = new Subject<void>();

  items: CaseTariffDto[] = [];
  displayedColumns = ['status', 'tariffCode', 'value', 'qty', 'fullValue', 'agreedRate', 'discount', 'totalOvercharged', 'totalPayable', 'dateOfProcedure', 'actions'];
  showAddForm = false;

  codeSearchControl = new FormControl('');

  addForm = this.fb.group({
    tariffId: [null as number | null],
    code: [''],
    value: [0 as number | null],
    qty: [1],
    agreedRateOverride: [null as number | null],
    valueIsTotal: [false],
    rejected: [false],
    dateOfProcedure: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadItems(): void {
    this.caseService.getTariffs(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load tariffs', 'Close', { duration: 3000 })
    });
  }

  onShowAddForm(): void {
    this.showAddForm = true;
    this.codeSearchControl.setValue('');
    this.addForm.reset({ qty: 1, value: 0 });
  }

  onOpenFullLookup(): void {
    const dialogRef = this.dialog.open(TariffLookupComponent, {
      width: '700px',
      data: { title: 'Select Tariff' }
    });

    dialogRef.afterClosed().subscribe((result: TariffLookupResult | null) => {
      if (result) {
        this.codeSearchControl.setValue(result.code);
      }
    });
  }

  onAdd(): void {
    const code = (this.codeSearchControl.value || '').trim();
    if (!code) {
      this.snackBar.open('Please enter a tariff code', 'Close', { duration: 3000 });
      return;
    }

    // Call the case-context SP to look up the tariff with rates
    const val = this.addForm.value;
    this.tariffService.lookupForCase(this.caseId, code).subscribe({
      next: (results) => {
        if (results.length === 0) {
          this.snackBar.open('No tariff found for this code', 'Close', { duration: 3000 });
          return;
        }

        if (results.length === 1) {
          // Auto-save with the single result
          this.saveTariff(results[0].id);
        } else {
          // Multiple specialities — let user choose from pre-filtered list
          const dialogRef = this.dialog.open(TariffLookupComponent, {
            width: '700px',
            data: {
              title: 'Select Tariff — Multiple specialities found',
              preloadedResults: results
            }
          });
          dialogRef.afterClosed().subscribe((selected: TariffLookupResult | null) => {
            if (selected) {
              this.saveTariff(selected.id);
            }
          });
        }
      },
      error: () => this.snackBar.open('Failed to look up tariff', 'Close', { duration: 3000 })
    });
  }

  private saveTariff(tariffId: number): void {
    const val = this.addForm.value;
    const request: CreateCaseTariffRequest = {
      tariffId: tariffId,
      value: val.value || undefined,
      qty: val.qty || 1,
      agreedRateOverride: val.agreedRateOverride || undefined,
      valueIsTotal: val.valueIsTotal || false,
      rejected: val.rejected || false,
      dateOfProcedure: val.dateOfProcedure?.toISOString().split('T')[0]
    };

    this.caseService.createTariff(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset({ qty: 1, value: 0 });
        this.codeSearchControl.setValue('');
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

  getFullValue(item: CaseTariffDto): number {
    return (item.value || 0) * (item.qty || 1);
  }
}
