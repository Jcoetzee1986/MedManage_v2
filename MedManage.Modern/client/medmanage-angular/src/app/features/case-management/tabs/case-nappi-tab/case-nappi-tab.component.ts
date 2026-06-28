import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CaseService } from '../../services/case.service';
import { CaseNappiDto, CreateCaseNappiRequest } from '../../models/case.models';
import { CodeLookupDialogComponent } from '../../../../shared/components/code-lookup-dialog/code-lookup-dialog.component';

export interface NappiLookupResult {
  id: number;
  code: string;
  description: string;
  price1: number | null;
}

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
    MatSnackBarModule,
    MatDialogModule,
    MatTooltipModule
  ],
  templateUrl: './case-nappi-tab.component.html',
  styleUrls: ['./case-nappi-tab.component.scss']
})
export class CaseNappiTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  items: CaseNappiDto[] = [];
  displayedColumns = ['nappiCode', 'nappiDescription', 'price1', 'value', 'quantity', 'measure', 'dispensary', 'ward', 'theater', 'tto', '_0201', 'date', 'actions'];
  showAddForm = false;
  selectedNappi: NappiLookupResult | null = null;

  codeSearchControl = new FormControl('');

  addForm = this.fb.group({
    nappiId: [null as number | null],
    value: [0 as number | null],
    quantity: [1 as number | null],
    dispensary: [false],
    ward: [false],
    theater: [false],
    tto: [false],
    _0201: [false],
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

  onShowAddForm(): void {
    this.showAddForm = true;
    this.selectedNappi = null;
    this.codeSearchControl.setValue('');
    this.addForm.reset({ quantity: 1, value: 0 });
  }

  onOpenNappiLookup(): void {
    const dialogRef = this.dialog.open(CodeLookupDialogComponent, {
      width: '700px',
      data: {
        codeType: 'nappi',
        title: 'Search NAPPI Codes'
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.selectedNappi = {
          id: result.id || result.nappiId,
          code: result.code || result.nappiCode,
          description: result.description,
          price1: result.price1 || result.currentRate || null
        };
        this.addForm.patchValue({
          nappiId: this.selectedNappi!.id,
          value: this.selectedNappi!.price1 || 0
        });
        this.codeSearchControl.setValue(this.selectedNappi!.code);
      }
    });
  }

  onAdd(): void {
    if (!this.selectedNappi) {
      this.snackBar.open('Please select a NAPPI code', 'Close', { duration: 3000 });
      return;
    }

    const val = this.addForm.value;
    const request: CreateCaseNappiRequest = {
      nappiId: this.selectedNappi.id,
      value: val.value || undefined,
      quantity: val.quantity || 1,
      dispensary: val.dispensary || false,
      ward: val.ward || false,
      theater: val.theater || false,
      tto: val.tto || false,
      date: val.date?.toISOString().split('T')[0]
    };

    this.caseService.createNappi(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset({ quantity: 1, value: 0 });
        this.selectedNappi = null;
        this.codeSearchControl.setValue('');
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
