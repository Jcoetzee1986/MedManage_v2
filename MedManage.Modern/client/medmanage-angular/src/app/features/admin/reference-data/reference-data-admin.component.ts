import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { ReferenceDataService } from '../../../core/services/reference-data.service';
import {
  ReferenceDataItem,
  ReferenceDataResource,
  ReferenceDataConfig,
  REFERENCE_DATA_CONFIGS
} from '../../../core/models/reference-data.models';
import { ReferenceDataDialogComponent } from './reference-data-dialog.component';

@Component({
  selector: 'app-reference-data-admin',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatDialogModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatTooltipModule
  ],
  templateUrl: './reference-data-admin.component.html',
  styleUrls: ['./reference-data-admin.component.scss']
})
export class ReferenceDataAdminComponent implements OnInit {
  private readonly referenceDataService = inject(ReferenceDataService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  /** All available lookup table configurations */
  configs = REFERENCE_DATA_CONFIGS;

  /** Currently selected resource config */
  selectedConfig: ReferenceDataConfig | null = null;

  /** Items loaded for the selected resource */
  items: ReferenceDataItem[] = [];

  /** Table columns */
  displayedColumns = ['id', 'name', 'description', 'actions'];

  /** Loading state */
  loading = false;

  ngOnInit(): void {
    // Default to first config
    if (this.configs.length > 0) {
      this.selectResource(this.configs[0]);
    }
  }

  /** Select a reference data resource and load its items */
  selectResource(config: ReferenceDataConfig): void {
    this.selectedConfig = config;
    this.loadItems();
  }

  /** Load items for the currently selected resource */
  loadItems(): void {
    if (!this.selectedConfig) return;

    this.loading = true;
    this.referenceDataService.getAll(this.selectedConfig.resource).subscribe({
      next: (items) => {
        this.items = items;
        this.loading = false;
      },
      error: (err) => {
        this.items = [];
        this.loading = false;
        this.snackBar.open('Failed to load items', 'Dismiss', { duration: 3000 });
      }
    });
  }

  /** Open dialog to create a new item */
  openCreateDialog(): void {
    if (!this.selectedConfig) return;

    const dialogRef = this.dialog.open(ReferenceDataDialogComponent, {
      width: '450px',
      data: {
        mode: 'create',
        resourceName: this.selectedConfig.displayName,
        item: null
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.selectedConfig) {
        this.loading = true;
        this.referenceDataService.create(this.selectedConfig.resource, result).subscribe({
          next: () => {
            this.snackBar.open('Item created successfully', 'OK', { duration: 3000 });
            this.loadItems();
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to create item', 'Dismiss', { duration: 3000 });
          }
        });
      }
    });
  }

  /** Open dialog to edit an existing item */
  openEditDialog(item: ReferenceDataItem): void {
    if (!this.selectedConfig) return;

    const dialogRef = this.dialog.open(ReferenceDataDialogComponent, {
      width: '450px',
      data: {
        mode: 'edit',
        resourceName: this.selectedConfig.displayName,
        item
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && this.selectedConfig) {
        this.loading = true;
        this.referenceDataService.update(this.selectedConfig.resource, item.id, result).subscribe({
          next: () => {
            this.snackBar.open('Item updated successfully', 'OK', { duration: 3000 });
            this.loadItems();
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to update item', 'Dismiss', { duration: 3000 });
          }
        });
      }
    });
  }

  /** Delete an item with confirmation */
  deleteItem(item: ReferenceDataItem): void {
    if (!this.selectedConfig) return;

    const confirmed = confirm(`Are you sure you want to delete "${item.name}"?`);
    if (!confirmed) return;

    this.loading = true;
    this.referenceDataService.delete(this.selectedConfig.resource, item.id).subscribe({
      next: () => {
        this.snackBar.open('Item deleted successfully', 'OK', { duration: 3000 });
        this.loadItems();
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to delete item', 'Dismiss', { duration: 3000 });
      }
    });
  }
}
