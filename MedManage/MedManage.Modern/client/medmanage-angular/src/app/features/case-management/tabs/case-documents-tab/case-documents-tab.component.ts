import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseFileDto } from '../../models/case.models';

@Component({
  selector: 'app-case-documents-tab',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './case-documents-tab.component.html',
  styleUrls: ['./case-documents-tab.component.scss']
})
export class CaseDocumentsTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseFileDto[] = [];
  displayedColumns = ['fileName', 'fileType', 'fileSize', 'dateUploaded', 'uploadedBy', 'actions'];

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getFiles(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load documents', 'Close', { duration: 3000 })
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.caseService.uploadFile(this.caseId, file).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('File uploaded', 'Close', { duration: 3000 });
          input.value = '';
        },
        error: () => this.snackBar.open('Failed to upload file', 'Close', { duration: 3000 })
      });
    }
  }

  onDownload(item: CaseFileDto): void {
    this.caseService.downloadFile(this.caseId, item.id).subscribe({
      next: (blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = item.fileName || 'download';
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: () => this.snackBar.open('Failed to download file', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseFileDto): void {
    if (confirm(`Delete file "${item.fileName}"?`)) {
      this.caseService.deleteFile(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('File deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete file', 'Close', { duration: 3000 })
      });
    }
  }

  formatSize(bytes?: number): string {
    if (!bytes) return '—';
    if (bytes < 1024) return `${bytes} B`;
    if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }
}
