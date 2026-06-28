import { Component, inject, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReportViewerComponent } from '../report-viewer/report-viewer.component';
import { CaseLetterParamsComponent } from '../report-params/case-letter-params/case-letter-params.component';
import { CasesBetweenDatesParamsComponent } from '../report-params/cases-between-dates-params/cases-between-dates-params.component';
import { WipExtractParamsComponent } from '../report-params/wip-extract-params/wip-extract-params.component';
import { BillingSummaryParamsComponent } from '../report-params/billing-summary-params/billing-summary-params.component';
import { ReportService } from '../services/report.service';
import {
  ReportFormat,
  ReportType,
  CaseLetterParams,
  CasesBetweenDatesParams,
  WipExtractParams,
  BillingSummaryParams,
  REPORT_DEFINITIONS
} from '../models/report.models';

@Component({
  selector: 'app-reports-page',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    ReportViewerComponent,
    CaseLetterParamsComponent,
    CasesBetweenDatesParamsComponent,
    WipExtractParamsComponent,
    BillingSummaryParamsComponent
  ],
  templateUrl: './reports-page.component.html',
  styleUrls: ['./reports-page.component.scss']
})
export class ReportsPageComponent implements OnDestroy {
  private readonly reportService = inject(ReportService);
  private readonly snackBar = inject(MatSnackBar);

  readonly reportDefinitions = REPORT_DEFINITIONS;

  loading = false;
  errorMessage: string | null = null;
  previewUrl: string | null = null;
  lastBlob: Blob | null = null;
  lastFilename: string | null = null;

  ngOnDestroy(): void {
    if (this.previewUrl) {
      this.reportService.revokePreviewUrl(this.previewUrl);
    }
  }

  // ─── Report Generation Handlers ─────────────────────────────

  onGenerateCaseLetter(params: CaseLetterParams): void {
    this.clearPreview();
    this.loading = true;

    this.reportService.generateCaseLetter(params).subscribe({
      next: (blob) => this.handleReportSuccess(blob, 'case-letter', 'pdf'),
      error: (err) => this.handleReportError(err)
    });
  }

  onGenerateCasesBetweenDates(event: { params: CasesBetweenDatesParams; format: ReportFormat }): void {
    this.clearPreview();
    this.loading = true;

    this.reportService.generateCasesBetweenDates(event.params, event.format).subscribe({
      next: (blob) => this.handleReportSuccess(blob, 'cases-between-dates', event.format),
      error: (err) => this.handleReportError(err)
    });
  }

  onGenerateWipExtract(event: { params: WipExtractParams; format: ReportFormat }): void {
    this.clearPreview();
    this.loading = true;

    this.reportService.generateWipExtract(event.params, event.format).subscribe({
      next: (blob) => this.handleReportSuccess(blob, 'wip-extract', event.format),
      error: (err) => this.handleReportError(err)
    });
  }

  onGenerateBillingSummary(event: { params: BillingSummaryParams; format: ReportFormat }): void {
    this.clearPreview();
    this.loading = true;

    this.reportService.generateBillingSummary(event.params, event.format).subscribe({
      next: (blob) => this.handleReportSuccess(blob, 'billing-summary', event.format),
      error: (err) => this.handleReportError(err)
    });
  }

  // ─── Download ───────────────────────────────────────────────

  onDownload(): void {
    if (this.lastBlob && this.lastFilename) {
      this.reportService.downloadBlob(this.lastBlob, this.lastFilename);
      this.snackBar.open('Download started', 'OK', { duration: 3000 });
    }
  }

  // ─── Private Helpers ────────────────────────────────────────

  private handleReportSuccess(blob: Blob, reportType: ReportType, format: ReportFormat): void {
    this.loading = false;
    this.errorMessage = null;
    this.lastBlob = blob;
    this.lastFilename = this.reportService.buildFilename(reportType, format);

    if (format === 'pdf') {
      this.previewUrl = this.reportService.createPreviewUrl(blob);
    } else {
      // For Excel, just download directly since inline preview isn't supported
      this.reportService.downloadBlob(blob, this.lastFilename);
      this.snackBar.open('Excel report downloaded', 'OK', { duration: 3000 });
    }
  }

  private handleReportError(err: unknown): void {
    this.loading = false;
    this.errorMessage = 'Failed to generate report. Please check your parameters and try again.';
    console.error('Report generation error:', err);
  }

  private clearPreview(): void {
    if (this.previewUrl) {
      this.reportService.revokePreviewUrl(this.previewUrl);
      this.previewUrl = null;
    }
    this.lastBlob = null;
    this.lastFilename = null;
    this.errorMessage = null;
  }
}
