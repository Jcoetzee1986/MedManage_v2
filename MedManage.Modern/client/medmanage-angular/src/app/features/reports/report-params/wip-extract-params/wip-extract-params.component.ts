import { Component, EventEmitter, Output, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ReportFormat, WipExtractParams } from '../../models/report.models';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-wip-extract-params',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule
  ],
  templateUrl: './wip-extract-params.component.html',
  styleUrls: ['./wip-extract-params.component.scss']
})
export class WipExtractParamsComponent implements OnInit {
  @Output() generate = new EventEmitter<{ params: WipExtractParams; format: ReportFormat }>();

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);

  form = this.fb.group({
    dateFrom: [null as Date | null, Validators.required],
    dateTo: [null as Date | null, Validators.required],
    mainClientId: [null as number | null],
    format: ['excel' as ReportFormat]
  });

  ngOnInit(): void {
    this.form.patchValue({ mainClientId: this.authService.activeClientId });
  }

  onSubmit(): void {
    if (this.form.valid) {
      const value = this.form.value;
      const formatDate = (d: Date) => d.toISOString().split('T')[0];
      this.generate.emit({
        params: {
          dateFrom: formatDate(value.dateFrom!),
          dateTo: formatDate(value.dateTo!),
          mainClientId: value.mainClientId ?? undefined
        },
        format: value.format ?? 'excel'
      });
    }
  }
}
