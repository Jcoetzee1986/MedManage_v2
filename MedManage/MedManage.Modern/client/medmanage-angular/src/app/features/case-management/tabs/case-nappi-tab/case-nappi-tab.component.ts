import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseNappiDto, CreateCaseNappiRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-nappi-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './case-nappi-tab.component.html',
  styleUrls: ['./case-nappi-tab.component.scss']
})
export class CaseNappiTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseNappiDto[] = [];
  displayedColumns = ['nappiCode', 'nappiDescription', 'value', 'quantity', 'dispensary', 'ward', 'theater', 'tto', 'date', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    nappiId: [null as number | null],
    value: [null as number | null],
    quantity: [null as number | null],
    dispensary: [false],
    ward: [false],
    theater: [false],
    tto: [false],
    date: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getNappi(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load NAPPI codes', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseNappiRequest = {
      nappiId: val.nappiId || undefined,
      value: val.value || undefined,
      quantity: val.quantity || undefined,
      dispensary: val.dispensary || false,
      ward: val.ward || false,
      theater: val.theater || false,
      tto: val.tto || false,
      date: val.date?.toISOString()
    };

    this.caseService.createNappi(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('NAPPI code added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add NAPPI code', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseNappiDto): void {
    if (confirm('Delete this NAPPI code?')) {
      this.caseService.deleteNappi(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('NAPPI code deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
