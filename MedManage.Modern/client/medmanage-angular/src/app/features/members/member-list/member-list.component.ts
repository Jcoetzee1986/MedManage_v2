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
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MemberService } from '../services/member.service';
import { MemberDto, MemberSearchRequest } from '../models/member.models';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-member-list',
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
    MatDatepickerModule,
    MatNativeDateModule,
    MatToolbarModule
  ],
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.scss']
})
export class MemberListComponent implements OnInit {
  private readonly memberService = inject(MemberService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  displayedColumns = ['memberNumber', 'lastName', 'firstName', 'idNumber', 'medicalAidName', 'memberStatusName'];
  dataSource: MemberDto[] = [];
  totalCount = 0;
  pageSize = 25;
  pageIndex = 0;
  loading = false;
  sortField = 'lastName';
  sortDirection: 'asc' | 'desc' = 'asc';

  searchForm = this.fb.group({
    lastName: [''],
    firstName: [''],
    memberNumber: [''],
    passportNumber: [''],
    idNumber: [''],
    dateOfBirth: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers(): void {
    this.loading = true;
    const formValue = this.searchForm.value;

    const request: MemberSearchRequest = {
      lastName: formValue.lastName || undefined,
      firstName: formValue.firstName || undefined,
      memberNumber: formValue.memberNumber || undefined,
      passportNumber: formValue.passportNumber || undefined,
      idNumber: formValue.idNumber || undefined,
      dateOfBirth: formValue.dateOfBirth ? formValue.dateOfBirth.toISOString() : undefined,
      mainClientId: this.authService.activeClientId ?? undefined,
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      sortField: this.sortField,
      sortDirection: this.sortDirection
    };

    this.memberService.search(request).subscribe({
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
    this.loadMembers();
  }

  onReset(): void {
    this.searchForm.reset();
    this.pageIndex = 0;
    this.loadMembers();
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadMembers();
  }

  onSortChange(sort: Sort): void {
    this.sortField = sort.active;
    this.sortDirection = sort.direction as 'asc' | 'desc' || 'asc';
    this.loadMembers();
  }

  onRowClick(member: MemberDto): void {
    this.router.navigate(['/members', member.id]);
  }

  onRowDoubleClick(member: MemberDto): void {
    this.router.navigate(['/members', member.id]);
  }

  onOpenSelected(): void {
    // If there's a selected row, navigate to it
    if (this.dataSource.length > 0) {
      this.router.navigate(['/members', this.dataSource[0].id]);
    }
  }

  onNewMember(): void {
    this.router.navigate(['/members', 'new']);
  }

  onSearchKeydown(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      event.preventDefault();
      this.onSearch();
    }
  }
}
