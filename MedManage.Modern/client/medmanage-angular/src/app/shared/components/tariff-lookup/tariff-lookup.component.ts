import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { TariffLookupResult } from '../../../features/tariffs/models/tariff.models';

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

export interface TariffLookupDialogData {
  title?: string;
  preloadedResults?: TariffLookupResult[];
}

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
export class TariffLookupComponent implements OnDestroy {
  private readonly http = inject(HttpClient);
  private readonly dialogRef = inject(MatDialogRef<TariffLookupComponent>);
  private readonly data: TariffLookupDialogData | null = inject(MAT_DIALOG_DATA, { optional: true });
  private readonly destroy$ = new Subject<void>();
  private readonly baseUrl = `${environment.apiUrl}/tariff`;

  codeControl = new FormControl('');
  descriptionControl = new FormControl('');
  results: TariffLookupResult[] = [];
  loading = false;
  searched = false;
  selectedItem: TariffLookupResult | null = null;
  displayedColumns = ['code', 'description', 'category'];
  hasPreloadedResults = false;

  get title(): string {
    return this.data?.title || 'Select Tariff';
  }

  constructor() {
    // If preloaded results were passed, show them immediately
    if (this.data?.preloadedResults && this.data.preloadedResults.length > 0) {
      this.results = this.data.preloadedResults;
      this.searched = true;
      this.hasPreloadedResults = true;
      // Show rate column when preloaded results have rates (from SP)
      this.displayedColumns = ['code', 'description', 'category', 'rate'];
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearch(): void {
    const code = (this.codeControl.value || '').trim();
    const description = (this.descriptionControl.value || '').trim();

    if (!code && !description) return;

    this.loading = true;
    this.searched = true;

    let params = new HttpParams();
    if (code) params = params.set('code', code);
    if (description) params = params.set('description', description);

    this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/search`, { params })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (r) => {
          this.results = (r.data || []).map((item: any) => ({
            id: item.tariffId || 0,
            code: item.tariffCode || '',
            description: item.tariffDescription || '',
            category: item.category || null,
            currentRate: item.tariffAmount || null
          }));
          this.loading = false;
        },
        error: () => {
          this.results = [];
          this.loading = false;
        }
      });
  }

  onRowSelect(item: TariffLookupResult): void {
    this.selectedItem = item;
  }

  onConfirm(): void {
    if (this.selectedItem) {
      this.dialogRef.close(this.selectedItem);
    }
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  isSelected(item: TariffLookupResult): boolean {
    return this.selectedItem?.id === item.id;
  }
}
