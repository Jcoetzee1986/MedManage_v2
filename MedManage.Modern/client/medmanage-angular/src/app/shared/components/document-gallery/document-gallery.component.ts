import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { DocumentService } from '../../../core/services/document.service';
import { DocumentDto } from '../../../core/models/document.models';

/**
 * Reusable document viewer/gallery component.
 * Displays image thumbnails in a grid, supports click-to-preview full-size,
 * and shows file-type icons for non-image documents.
 *
 * Usage:
 *   <app-document-gallery
 *     [entityType]="'case'"
 *     [entityId]="caseId"
 *     [allowUpload]="true"
 *     [allowDelete]="true"
 *     (documentDeleted)="onDeleted($event)">
 *   </app-document-gallery>
 */
@Component({
  selector: 'app-document-gallery',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './document-gallery.component.html',
  styleUrls: ['./document-gallery.component.scss']
})
export class DocumentGalleryComponent implements OnInit, OnChanges {
  private readonly documentService = inject(DocumentService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  /** Entity type: 'case' or 'member' */
  @Input() entityType = '';

  /** Entity ID to load documents for */
  @Input() entityId = 0;

  /** Whether upload is allowed */
  @Input() allowUpload = true;

  /** Whether delete is allowed */
  @Input() allowDelete = true;

  /** Emitted when a document is deleted */
  @Output() documentDeleted = new EventEmitter<number>();

  /** Emitted when a document is uploaded */
  @Output() documentUploaded = new EventEmitter<DocumentDto>();

  documents: DocumentDto[] = [];
  loading = false;
  uploading = false;
  uploadProgress = 0;
  previewDocument: DocumentDto | null = null;

  ngOnInit(): void {
    this.loadDocuments();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if ((changes['entityType'] || changes['entityId']) && this.entityType && this.entityId) {
      this.loadDocuments();
    }
  }

  loadDocuments(): void {
    if (!this.entityType || !this.entityId) return;

    this.loading = true;
    this.documentService.getByEntity(this.entityType, this.entityId).subscribe({
      next: (docs) => {
        this.documents = docs;
        this.loading = false;
      },
      error: () => {
        this.documents = [];
        this.loading = false;
        this.snackBar.open('Failed to load documents', 'Close', { duration: 3000 });
      }
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length) return;

    const file = input.files[0];
    this.uploading = true;
    this.uploadProgress = 0;

    this.documentService.upload(file, this.entityType, this.entityId).subscribe({
      next: ({ progress, result }) => {
        this.uploadProgress = progress;
        if (result) {
          this.documents = [result, ...this.documents];
          this.uploading = false;
          this.documentUploaded.emit(result);
          this.snackBar.open('File uploaded successfully', 'Close', { duration: 3000 });
        }
      },
      error: () => {
        this.uploading = false;
        this.snackBar.open('Upload failed', 'Close', { duration: 3000 });
      }
    });

    // Reset input so the same file can be selected again
    input.value = '';
  }

  onDelete(doc: DocumentDto, event: Event): void {
    event.stopPropagation();
    if (!confirm(`Delete "${doc.fileName}"?`)) return;

    this.documentService.delete(doc.id).subscribe({
      next: () => {
        this.documents = this.documents.filter(d => d.id !== doc.id);
        this.documentDeleted.emit(doc.id);
        if (this.previewDocument?.id === doc.id) {
          this.previewDocument = null;
        }
        this.snackBar.open('Document deleted', 'Close', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Delete failed', 'Close', { duration: 3000 });
      }
    });
  }

  onDocumentClick(doc: DocumentDto): void {
    if (doc.isImage) {
      this.previewDocument = doc;
    } else {
      this.downloadDocument(doc);
    }
  }

  downloadDocument(doc: DocumentDto): void {
    const url = this.documentService.getDownloadUrl(doc.id);
    window.open(url, '_blank');
  }

  closePreview(): void {
    this.previewDocument = null;
  }

  getThumbnailUrl(doc: DocumentDto): string {
    return this.documentService.getThumbnailUrl(doc.id);
  }

  getDownloadUrl(doc: DocumentDto): string {
    return this.documentService.getDownloadUrl(doc.id);
  }

  getFileIcon(doc: DocumentDto): string {
    const ext = doc.fileType?.toLowerCase();
    switch (ext) {
      case 'pdf': return 'picture_as_pdf';
      case 'doc':
      case 'docx': return 'description';
      case 'xls':
      case 'xlsx': return 'table_chart';
      case 'txt': return 'text_snippet';
      case 'csv': return 'grid_on';
      default: return 'insert_drive_file';
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const sizes = ['B', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(1)) + ' ' + sizes[i];
  }
}
