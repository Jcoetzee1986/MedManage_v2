import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ProviderService } from '../../../features/providers/services/provider.service';
import { ProviderDto } from '../../../features/providers/models/provider.models';

@Component({
  selector: 'app-provider-lookup-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './provider-lookup-dialog.component.html'
})
export class ProviderLookupDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly providerService = inject(ProviderService);
  private readonly dialogRef = inject(MatDialogRef<ProviderLookupDialogComponent>);

  searchForm = this.fb.group({
    practiceName: [''],
    lastName: [''],
    providerNumber: ['']
  });

  results: ProviderDto[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searching = false;
  searched = false;
  displayedColumns = ['practiceName', 'lastName', 'firstName', 'specialityName', 'providerNumber'];

  ngOnInit(): void {}

  onSearch(): void {
    this.searching = true;
    this.searched = true;
    this.pageNumber = 1;
    this.doSearch(this.searchForm.getRawValue());
  }

  onPageNext(): void {
    if (this.pageNumber * this.pageSize < this.totalCount) {
      this.pageNumber++;
      this.doSearch(this.searchForm.getRawValue());
    }
  }

  onPagePrev(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.doSearch(this.searchForm.getRawValue());
    }
  }

  private doSearch(val: { practiceName: string | null; lastName: string | null; providerNumber: string | null }): void {
    this.searching = true;
    this.providerService.search({
      practiceName: val.practiceName || undefined,
      serviceProviderSurname: val.lastName || undefined,
      practiceNr: val.providerNumber || undefined,
      pageNumber: this.pageNumber,
      pageSize: this.pageSize
    }).subscribe({
      next: (result) => {
        this.results = result.items;
        this.totalCount = result.totalCount;
        this.searching = false;
      },
      error: () => {
        this.results = [];
        this.searching = false;
      }
    });
  }

  onSelect(provider: ProviderDto): void {
    this.dialogRef.close(provider);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
