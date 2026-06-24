import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { CaseLetterParams } from '../../models/report.models';

@Component({
  selector: 'app-case-letter-params',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './case-letter-params.component.html',
  styleUrls: ['./case-letter-params.component.scss']
})
export class CaseLetterParamsComponent {
  @Output() generate = new EventEmitter<CaseLetterParams>();

  private readonly fb = inject(FormBuilder);

  form = this.fb.group({
    caseId: [null as number | null, [Validators.required, Validators.min(1)]]
  });

  onSubmit(): void {
    if (this.form.valid) {
      this.generate.emit({
        caseId: this.form.value.caseId!
      });
    }
  }
}
