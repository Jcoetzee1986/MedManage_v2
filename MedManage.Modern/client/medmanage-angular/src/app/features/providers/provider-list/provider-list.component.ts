import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSortModule, Sort } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ProviderService } from '../services/provider.service';
import { ProviderDto, ProviderSearchRequest } from '../models/provider.models';

@Component({
  selector: 'app-provider-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatToolbarModule
  ],
  templateUrl: './provider-list.component.html',
  styleUrls: ['./provider-list.component.scss']
})
export class ProviderListComponent implements OnInit {
  private readonly providerService = inject(ProviderService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  displayedColumns = ['providerNumber', 'practiceName', 'firstName', 'lastName', 'specialityName', 'contactNumber'];
  dataSource: ProviderDto[] = [];
  totalCount = 0;
  pageSize = 25;
  pageIndex = 0;
  loading = false;
  sortField = 'practiceName';
  sortDirection: 'asc' | 'desc' = 'asc';
  selectedProvider: ProviderDto | null = null;

  searchForm = this.fb.group({
    providerNumber: [''],
    practiceName: [''],
    firstName: [''],
    lastName: [''],
    speciality: ['']
  });

  ngOnInit(): void {
    this.loadProviders();
  }

  loadProviders(): void {
    this.loading = true;
    const formValue = this.searchForm.value;

    const request: ProviderSearchRequest = {
      practiceNr: formValue.providerNumber || undefined,
      practiceName: formValue.practiceName || undefined,
      serviceProviderName: formValue.firstName || undefined,
      serviceProviderSurname: formValue.lastName || undefined,
      specialityName: formValue.speciality || undefined,
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      sortField: this.sortField,
      sortDirection: this.sortDirection
    };

    this.providerService.search(request).subscribe({
      next: (result) => {
        this.dataSource = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.loadProviders();
  }

  onSearchKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      event.preventDefault();
      this.onSearch();
    }
  }

  onReset(): void {
    this.searchForm.reset();
    this.pageIndex = 0;
    this.loadProviders();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadProviders();
  }

  onSortChange(sort: Sort): void {
    this.sortField = sort.active;
    this.sortDirection = sort.direction as 'asc' | 'desc' || 'asc';
    this.loadProviders();
  }

  onRowClick(provider: ProviderDto): void {
    this.selectedProvider = provider;
  }

  onRowDoubleClick(provider: ProviderDto): void {
    this.router.navigate(['/providers', provider.id]);
  }

  onOpenSelected(): void {
    if (this.selectedProvider) {
      this.router.navigate(['/providers', this.selectedProvider.id]);
    }
  }

  onNewProvider(): void {
    this.router.navigate(['/providers', 'new']);
  }
}
