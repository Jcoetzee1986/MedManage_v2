import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ImportService } from '../../../core/services/import.service';
import { ImportResultDto, ImportHistoryDto } from '../../../core/models/import.models';

@Component({
  selector: 'app-imports',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatProgressBarModule,
    MatSnackBarModule,
    MatTabsModule,
    MatTableModule,
    MatChipsModule,
    MatSelectModule,
    MatFormFieldModule
  ],
  templateUrl: './imports.component.html',
  styleUrls: ['./imports.component.scss']
})
export class ImportsComponent implements OnInit {
  private readonly importService = inject(ImportService);
  private readonly snackBar = inject(MatSnackBar);

  // Upload state
  uploading = false;
  uploadProgress = 0;
  currentImportType = '';

  // Results
  lastResult: ImportResultDto | null = null;

  // History
  history: ImportHistoryDto[] = [];
  historyLoading = false;
  historyFilter = '';

  historyColumns = ['importDate', 'importType', 'fileName', 'totalRecords', 'importedRecords', 'skippedRecords', 'errorRecords', 'status'];

  ngOnInit(): void {
    this.loadHistory();
  }

  onFileSelected(event: Event, importType: 'members' | 'nappi' | 'billing'): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    this.startImport(file, importType);
    input.value = ''; // Reset for re-selection
  }

  private startImport(file: File, importType: 'members' | 'nappi' | 'billing'): void {
    this.uploading = true;
    this.uploadProgress = 0;
    this.currentImportType = importType;
    this.lastResult = null;

    const importFn = importType === 'members'
      ? this.importService.importMembers(file)
      : importType === 'nappi'
        ? this.importService.importNappiCodes(file)
        : this.importService.importBilling(file);

    importFn.subscribe({
      next: ({ progress, result }) => {
        this.uploadProgress = progress;
        if (result) {
          this.lastResult = result;
          this.uploading = false;
          this.loadHistory();
          this.snackBar.open(
            `Import complete: ${result.importedRecords} imported, ${result.errorRecords} errors`,
            'OK',
            { duration: 5000 }
          );
        }
      },
      error: (err) => {
        this.uploading = false;
        this.uploadProgress = 0;
        const message = err?.error?.message || 'Import failed';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
      }
    });
  }

  loadHistory(): void {
    this.historyLoading = true;
    this.importService.getHistory(this.historyFilter || undefined).subscribe({
      next: (data) => {
        this.history = data;
        this.historyLoading = false;
      },
      error: () => {
        this.historyLoading = false;
        this.snackBar.open('Failed to load import history', 'Dismiss', { duration: 3000 });
      }
    });
  }

  onFilterChange(value: string): void {
    this.historyFilter = value;
    this.loadHistory();
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Completed': return 'primary';
      case 'CompletedWithErrors': return 'accent';
      case 'Failed': return 'warn';
      default: return '';
    }
  }
}
