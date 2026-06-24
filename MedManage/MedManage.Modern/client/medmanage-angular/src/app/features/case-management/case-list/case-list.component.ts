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
import { AgGridModule } from 'ag-grid-angular';
import {
  ColDef,
  GridReadyEvent,
  GridApi,
  IServerSideDatasource,
  IServerSideGetRowsParams
} from 'ag-grid-community';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../services/case.service';
import { CaseDto, CaseSearchRequest } from '../models/case.models';

@Component({
  selector: 'app-case-list',
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
    AgGridModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-list.component.html',
  styleUrls: ['./case-list.component.scss']
})
export class CaseListComponent implements OnInit {
  private readonly caseService = inject(CaseService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  private gridApi!: GridApi;

  /** Search filter form */
  searchForm = this.fb.group({
    authNumber: [''],
    memberName: [''],
    memberNumber: [''],
    caseStatusId: [null as number | null],
    caseTypeId: [null as number | null],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** ag-Grid column definitions */
  columnDefs: ColDef[] = [
    { field: 'caseNumber', headerName: 'Case #', width: 120, sortable: true },
    { field: 'authNumber', headerName: 'Auth #', width: 130, sortable: true },
    { field: 'memberName', headerName: 'Member', flex: 1, sortable: true },
    { field: 'memberNumber', headerName: 'Member #', width: 120 },
    { field: 'caseStatusName', headerName: 'Status', width: 120 },
    { field: 'caseTypeName', headerName: 'Type', width: 120 },
    { field: 'referToName', headerName: 'Refer To', width: 150 },
    { field: 'dateOfService', headerName: 'Service Date', width: 120, valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : '' },
    { field: 'dateCreated', headerName: 'Created', width: 120, valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : '' }
  ];

  /** ag-Grid default column settings */
  defaultColDef: ColDef = {
    resizable: true,
    filter: false
  };

  /** Server-side row model type */
  rowModelType: 'serverSide' = 'serverSide';

  ngOnInit(): void {}

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
    this.gridApi.setGridOption('serverSideDatasource', this.createDatasource());
  }

  /** Create server-side datasource for ag-Grid */
  private createDatasource(): IServerSideDatasource {
    return {
      getRows: (params: IServerSideGetRowsParams) => {
        const startRow = params.request.startRow ?? 0;
        const endRow = params.request.endRow ?? 50;
        const pageSize = endRow - startRow;
        const pageNumber = Math.floor(startRow / pageSize) + 1;

        const sortModel = params.request.sortModel;
        const sortField = sortModel?.length ? sortModel[0].colId : undefined;
        const sortDirection = sortModel?.length ? sortModel[0].sort as 'asc' | 'desc' : undefined;

        const formValue = this.searchForm.value;
        const searchRequest: CaseSearchRequest = {
          authNumber: formValue.authNumber || undefined,
          memberName: formValue.memberName || undefined,
          memberNumber: formValue.memberNumber || undefined,
          caseStatusId: formValue.caseStatusId || undefined,
          caseTypeId: formValue.caseTypeId || undefined,
          dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
          dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
          pageNumber,
          pageSize,
          sortField,
          sortDirection
        };

        this.caseService.search(searchRequest).subscribe({
          next: (result) => {
            params.success({
              rowData: result.items,
              rowCount: result.totalCount
            });
          },
          error: () => {
            params.fail();
          }
        });
      }
    };
  }

  /** Execute search with current filters */
  onSearch(): void {
    this.gridApi?.refreshServerSide({ purge: true });
  }

  /** Reset search form */
  onReset(): void {
    this.searchForm.reset();
    this.gridApi?.refreshServerSide({ purge: true });
  }

  /** Navigate to case detail */
  onRowDoubleClicked(event: any): void {
    if (event.data) {
      this.router.navigate(['/cases', event.data.id]);
    }
  }

  /** Navigate to create new case */
  onNewCase(): void {
    this.router.navigate(['/cases/new']);
  }
}
