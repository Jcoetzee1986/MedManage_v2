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
import { MatDialogModule } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { EpisodeService } from '../services/episode.service';
import { EpisodeDto, EpisodeSearchFilters } from '../models/episode.models';
import { DataTableComponent, DataTableColumn } from '../../../shared/components/data-table/data-table.component';

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
    DataTableComponent
  ],
  templateUrl: './episode-list.component.html',
  styleUrls: ['./episode-list.component.scss']
})
export class EpisodeListComponent implements OnInit {
  private readonly episodeService = inject(EpisodeService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  rowData: EpisodeDto[] = [];
  totalCount = 0;
  pageSize = 30;
  pageIndex = 0;
  loading = false;
  currentSortBy?: string;
  currentSortDescending?: boolean;

  searchForm = this.fb.group({
    episodeName: [''],
    memberId: [null as number | null],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** Column definitions for DataTableComponent */
  tableColumnDefs: DataTableColumn[] = [
    { field: 'episodeId', header: 'ID', width: '80px' },
    { field: 'episodeDescription', header: 'Description' },
    { field: 'memberId', header: 'Member ID', width: '120px' },
    { field: 'dateCreated', header: 'Date Created', width: '120px', pipe: 'date' },
    { field: 'dateInserted', header: 'Added', width: '120px', pipe: 'date' }
  ];

  ngOnInit(): void {
    this.loadEpisodes();
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
      pageSize: this.pageSize,
      sortBy: this.currentSortBy,
      sortDescending: this.currentSortDescending
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

  onSortChange(sort: Sort): void {
    if (sort.direction) {
      this.currentSortBy = sort.active;
      this.currentSortDescending = sort.direction === 'desc';
    } else {
      this.currentSortBy = undefined;
      this.currentSortDescending = undefined;
    }
    this.pageIndex = 0;
    this.loadEpisodes();
  }

  onRowDoubleClicked(row: any): void {
    if (row) {
      this.router.navigate(['/episodes', row.episodeId]);
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
