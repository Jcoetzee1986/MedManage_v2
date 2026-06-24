import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { AgGridModule } from 'ag-grid-angular';
import { ColDef, GridReadyEvent, GridApi } from 'ag-grid-community';
import { EpisodeService } from '../services/episode.service';
import { EpisodeDto, EpisodeSearchFilters } from '../models/episode.models';

@Component({
  selector: 'app-episode-list',
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
    MatCardModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatDialogModule,
    MatPaginatorModule,
    AgGridModule
  ],
  templateUrl: './episode-list.component.html',
  styleUrls: ['./episode-list.component.scss']
})
export class EpisodeListComponent implements OnInit {
  private readonly episodeService = inject(EpisodeService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  private gridApi!: GridApi;
  rowData: EpisodeDto[] = [];
  totalCount = 0;
  pageSize = 25;
  pageIndex = 0;
  loading = false;

  searchForm = this.fb.group({
    episodeName: [''],
    memberId: [null as number | null],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  columnDefs: ColDef[] = [
    { field: 'episodeId', headerName: 'ID', width: 80, sortable: true },
    { field: 'episodeDescription', headerName: 'Description', flex: 2, sortable: true },
    { field: 'memberId', headerName: 'Member ID', width: 120, sortable: true },
    {
      field: 'dateCreated',
      headerName: 'Date Created',
      width: 140,
      sortable: true,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    },
    {
      field: 'dateInserted',
      headerName: 'Added',
      width: 140,
      sortable: true,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    }
  ];

  defaultColDef: ColDef = {
    resizable: true,
    filter: false
  };

  ngOnInit(): void {
    this.loadEpisodes();
  }

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
  }

  loadEpisodes(): void {
    this.loading = true;
    const formValue = this.searchForm.value;

    const filters: EpisodeSearchFilters = {
      episodeName: formValue.episodeName || undefined,
      memberId: formValue.memberId || undefined,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize
    };

    this.episodeService.search(filters).subscribe({
      next: (result) => {
        this.rowData = result.items;
        this.totalCount = result.totalCount;
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load episodes', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.pageIndex = 0;
    this.loadEpisodes();
  }

  onReset(): void {
    this.searchForm.reset();
    this.pageIndex = 0;
    this.loadEpisodes();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadEpisodes();
  }

  onRowDoubleClicked(event: any): void {
    if (event.data) {
      this.router.navigate(['/episodes', event.data.episodeId]);
    }
  }

  onNewEpisode(): void {
    this.router.navigate(['/episodes/new']);
  }

  onDelete(episode: EpisodeDto): void {
    if (!confirm(`Delete episode "${episode.episodeDescription}"?`)) {
      return;
    }

    this.episodeService.delete(episode.episodeId).subscribe({
      next: () => {
        this.snackBar.open('Episode deleted', 'Close', { duration: 3000 });
        this.loadEpisodes();
      },
      error: () => {
        this.snackBar.open('Failed to delete episode', 'Close', { duration: 3000 });
      }
    });
  }
}
