import { Component, Input, Output, EventEmitter, ViewChild, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort } from '@angular/material/sort';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

/**
 * Column definition for the reusable data-table component.
 */
export interface DataTableColumn {
  /** Property name on the row object */
  field: string;
  /** Display header text */
  header: string;
  /** Optional width (e.g., '120px', '15%') */
  width?: string;
  /** Whether column is sortable (default: true) */
  sortable?: boolean;
  /** Optional value formatter function */
  format?: (value: any, row: any) => string;
  /** Optional pipe type: 'date', 'currency', 'number' */
  pipe?: 'date' | 'currency' | 'number';
  /** Pipe format argument (e.g., 'dd/MM/yyyy' for date) */
  pipeFormat?: string;
  /** Currency code for currency pipe (default: 'ZAR') */
  currencyCode?: string;
  /** Fallback fields to try if primary field is null/undefined */
  fallbacks?: string[];
  /** Text alignment: 'left' | 'center' | 'right' */
  align?: 'left' | 'center' | 'right';
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './data-table.component.html',
  styleUrls: ['./data-table.component.scss']
})
export class DataTableComponent implements OnChanges {
  /** Column definitions */
  @Input() columns: DataTableColumn[] = [];

  /** Row data array */
  @Input() data: any[] = [];

  /** Total count for server-side pagination */
  @Input() totalCount = 0;

  /** Current page size */
  @Input() pageSize = 30;

  /** Current page index */
  @Input() pageIndex = 0;

  /** Available page size options */
  @Input() pageSizeOptions: number[] = [10, 30, 50];

  /** Whether to show pagination */
  @Input() showPaginator = true;

  /** Whether data is loading */
  @Input() loading = false;

  /** Message to show when no data */
  @Input() emptyMessage = 'No records found.';

  /** Whether to enable sorting */
  @Input() sortable = true;

  /** Whether rows are selectable on click */
  @Input() selectable = true;

  /** The currently selected row */
  @Input() selectedRow: any = null;

  /** Emitted when a row is double-clicked */
  @Output() rowDblClick = new EventEmitter<any>();

  /** Emitted when a row is single-clicked */
  @Output() rowClick = new EventEmitter<any>();

  /** Emitted when page changes */
  @Output() pageChange = new EventEmitter<PageEvent>();

  /** Emitted when sort changes */
  @Output() sortChange = new EventEmitter<Sort>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  /** Computed column names for mat-table */
  get displayedColumns(): string[] {
    return this.columns.map(c => c.field);
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reset paginator when data changes externally
    if (changes['pageIndex'] && this.paginator) {
      this.paginator.pageIndex = this.pageIndex;
    }
  }

  /** Get cell value with fallback chain */
  getCellValue(row: any, col: DataTableColumn): string {
    let value = row[col.field];

    // Try fallback fields
    if ((value === null || value === undefined || value === '') && col.fallbacks) {
      for (const fallback of col.fallbacks) {
        const fbVal = row[fallback];
        if (fbVal !== null && fbVal !== undefined && fbVal !== '') {
          value = fbVal;
          break;
        }
      }
    }

    if (value === null || value === undefined) return '';

    // Apply format function
    if (col.format) {
      return col.format(value, row);
    }

    // Apply pipe-style formatting
    if (col.pipe === 'date' && value) {
      try {
        const d = new Date(value);
        if (!isNaN(d.getTime())) {
          const day = String(d.getDate()).padStart(2, '0');
          const month = String(d.getMonth() + 1).padStart(2, '0');
          const year = d.getFullYear();
          return `${day}/${month}/${year}`;
        }
      } catch {
        return String(value);
      }
    }

    if (col.pipe === 'currency' && value != null) {
      const code = col.currencyCode || 'R';
      return `${code} ${Number(value).toFixed(2)}`;
    }

    if (col.pipe === 'number' && value != null) {
      return Number(value).toLocaleString();
    }

    return String(value);
  }

  onRowClick(row: any): void {
    if (this.selectable) {
      this.selectedRow = row;
      this.rowClick.emit(row);
    }
  }

  onRowDblClick(row: any): void {
    this.rowDblClick.emit(row);
  }

  onPageChange(event: PageEvent): void {
    this.pageChange.emit(event);
  }

  onSortChange(sort: Sort): void {
    this.sortChange.emit(sort);
  }

  isSelected(row: any): boolean {
    return this.selectedRow === row;
  }
}
