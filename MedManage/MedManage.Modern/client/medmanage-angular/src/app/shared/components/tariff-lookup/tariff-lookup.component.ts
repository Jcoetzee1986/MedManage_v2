import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, debounceTime, distinctUntilChanged, switchMap, takeUntil, of } from 'rxjs';
import { TariffService } from '../../../features/tariffs/services/tariff.service';
import { TariffLookupResult } from '../../../features/tariffs/models/tariff.models';

export interface TariffLookupDialogData {
  title?: string;
}

/**
 * Reusable tariff code lookup dialog component.
 * Opens as a Material dialog and allows searching tariff codes via autocomplete.
 * Returns the selected TariffLookupResult on selection, or null on cancel.
 *
 * Usage:
 *   const dialogRef = this.dialog.open(TariffLookupComponent, {
 *     width: '600px',
 *     data: { title: 'Select Tariff Code' }
 *   });
 *   dialogRef.afterClosed().subscribe((result: TariffLookupResult | null) => { ... });
 */
@Component({
  selector: 'app-tariff-lookup',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './tariff-lookup.component.html',
  styleUrls: ['./tariff-lookup.component.scss']
})
export class TariffLookupComponent implements OnInit, OnDestroy {
  private readonly tariffService = inject(TariffService);
  private readonly dialogRef = inject(MatDialogRef<TariffLookupComponent>);
  private readonly data: TariffLookupDialogData | null = inject(MAT_DIALOG_DATA, { optional: true });

  private readonly destroy$ = new Subject<void>();

  searchControl = new FormControl('');
  results: TariffLookupResult[] = [];
  loading = false;
  selectedItem: TariffLookupResult | null = null;
  displayedColumns = ['code', 'description', 'category', 'currentRate'];

  get title(): string {
    return this.data?.title || 'Tariff Code Lookup';
  }

  ngOnInit(): void {
    this.searchControl.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(300),
      distinctUntilChanged(),
      switchMap(query => {
        const trimmed = (query || '').trim();
        if (trimmed.length < 2) {
          return of([]);
        }
        this.loading = true;
        return this.tariffService.lookup(trimmed);
      })
    ).subscribe({
      next: (results) => {
        this.results = results;
        this.loading = false;
      },
      error: () => {
        this.results = [];
        this.loading = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onRowSelect(item: TariffLookupResult): void {
    this.selectedItem = item;
  }

  onConfirm(): void {
    this.dialogRef.close(this.selectedItem);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  isSelected(item: TariffLookupResult): boolean {
    return this.selectedItem?.id === item.id;
  }
}
