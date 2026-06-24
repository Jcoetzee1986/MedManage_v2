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
import { CodeLookupService, CodeLookupResult, CodeType } from '../../../core/services/code-lookup.service';

/** Data passed to the dialog when opening */
export interface CodeLookupDialogData {
  /** Which code type to search: 'cpt', 'icd', or 'nappi' */
  codeType: CodeType;
  /** Optional dialog title override */
  title?: string;
  /** Optional effective date for NAPPI filtering */
  effectiveDate?: Date;
}

/**
 * Reusable code lookup dialog component for searching CPT, ICD, and NAPPI codes.
 * Opens as a Material dialog with typeahead search and returns the selected code on confirm.
 *
 * Usage:
 *   const dialogRef = this.dialog.open(CodeLookupDialogComponent, {
 *     width: '650px',
 *     data: { codeType: 'cpt', title: 'Select CPT Code' }
 *   });
 *   dialogRef.afterClosed().subscribe((result: CodeLookupResult | null) => { ... });
 */
@Component({
  selector: 'app-code-lookup-dialog',
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
  templateUrl: './code-lookup-dialog.component.html',
  styleUrls: ['./code-lookup-dialog.component.scss']
})
export class CodeLookupDialogComponent implements OnInit, OnDestroy {
  private readonly codeLookupService = inject(CodeLookupService);
  private readonly dialogRef = inject(MatDialogRef<CodeLookupDialogComponent>);
  private readonly data: CodeLookupDialogData = inject(MAT_DIALOG_DATA);

  private readonly destroy$ = new Subject<void>();

  searchControl = new FormControl('');
  results: CodeLookupResult[] = [];
  loading = false;
  selectedItem: CodeLookupResult | null = null;
  displayedColumns = ['code', 'description'];

  private readonly defaultTitles: Record<CodeType, string> = {
    cpt: 'CPT Code Lookup',
    icd: 'ICD Code Lookup',
    nappi: 'NAPPI Code Lookup'
  };

  get title(): string {
    return this.data.title || this.defaultTitles[this.data.codeType];
  }

  get searchPlaceholder(): string {
    switch (this.data.codeType) {
      case 'cpt': return 'Search by CPT code or description...';
      case 'icd': return 'Search by ICD code or description...';
      case 'nappi': return 'Search by NAPPI code or description...';
    }
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
        return this.codeLookupService.search(
          this.data.codeType,
          trimmed,
          this.data.effectiveDate
        );
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

  onRowSelect(item: CodeLookupResult): void {
    this.selectedItem = item;
  }

  onRowDoubleClick(item: CodeLookupResult): void {
    this.selectedItem = item;
    this.onConfirm();
  }

  onConfirm(): void {
    this.dialogRef.close(this.selectedItem);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  isSelected(item: CodeLookupResult): boolean {
    return this.selectedItem?.id === item.id;
  }
}
