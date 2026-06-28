import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReferenceDataDropdownComponent } from '../../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../../services/case.service';
import { CaseFacilityTypeDto, CreateCaseFacilityTypeRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-facility-types-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-facility-types-tab.component.html',
  styleUrls: ['./case-facility-types-tab.component.scss']
})
export class CaseFacilityTypesTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseFacilityTypeDto[] = [];
  displayedColumns = ['facilityTypeName', 'dateAdmitted', 'dateDischarged', 'los', 'minutesOnVentilator', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    facilityTypeId: [null as number | null],
    dateAdmitted: [null as Date | null],
    dateDischarged: [null as Date | null],
    los: [null as number | null],
    minutesOnVentilator: [null as number | null],
    comments: ['']
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getFacilityTypes(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load facility types', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseFacilityTypeRequest = {
      facilityTypeId: val.facilityTypeId || undefined,
      dateAdmitted: val.dateAdmitted?.toISOString(),
      dateDischarged: val.dateDischarged?.toISOString(),
      los: val.los || undefined,
      minutesOnVentilator: val.minutesOnVentilator || undefined,
      comments: val.comments || undefined
    };

    this.caseService.createFacilityType(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('Facility type added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add facility type', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseFacilityTypeDto): void {
    if (confirm('Delete this facility type?')) {
      this.caseService.deleteFacilityType(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Facility type deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
