import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-report-viewer',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './report-viewer.component.html',
  styleUrls: ['./report-viewer.component.scss']
})
export class ReportViewerComponent implements OnChanges, OnDestroy {
  @Input() pdfUrl: string | null = null;
  @Input() loading = false;
  @Input() errorMessage: string | null = null;

  safeUrl: SafeResourceUrl | null = null;

  constructor(private readonly sanitizer: DomSanitizer) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['pdfUrl']) {
      if (this.pdfUrl) {
        this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.pdfUrl);
      } else {
        this.safeUrl = null;
      }
    }
  }

  ngOnDestroy(): void {
    // Clean up is handled by the parent component via ReportService.revokePreviewUrl
  }
}
