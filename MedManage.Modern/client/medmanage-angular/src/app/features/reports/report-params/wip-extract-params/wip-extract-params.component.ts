import { Component, EventEmitter, Output, inject } from '@angular/core';
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
export class WipExtractParamsComponent {
  @Output() generate = new EventEmitter<{ params: WipExtractParams; format: ReportFormat }>();

  private readonly fb = inject(FormBuilder);

  form = this.fb.group({
    dateFrom: [null as Date | null, Validators.required],
    dateTo: [null as Date | null, Validators.required],
    providerId: [null as number | null],
    format: ['excel' as ReportFormat]
  });

  onSubmit(): void {
    if (this.form.valid) {
      const value = this.form.value;
      this.generate.emit({
        params: {
          dateFrom: value.dateFrom!.toISOString(),
          dateTo: value.dateTo!.toISOString(),
          providerId: value.providerId ?? undefined
        },
        format: value.format ?? 'excel'
      });
    }
  }
}
