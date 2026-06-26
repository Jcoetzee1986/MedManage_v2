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
import { MemberService } from '../../../features/members/services/member.service';
import { MemberDto } from '../../../features/members/models/member.models';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-member-lookup-dialog',
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
  templateUrl: './member-lookup-dialog.component.html'
})
export class MemberLookupDialogComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly memberService = inject(MemberService);
  private readonly authService = inject(AuthService);
  private readonly dialogRef = inject(MatDialogRef<MemberLookupDialogComponent>);

  searchForm = this.fb.group({
    lastName: [''],
    firstName: [''],
    memberNumber: [''],
    idNumber: ['']
  });

  results: MemberDto[] = [];
  totalCount = 0;
  pageNumber = 1;
  pageSize = 10;
  searching = false;
  searched = false;
  displayedColumns = ['memberNumber', 'lastName', 'firstName', 'idNumber', 'medicalAidName'];

  ngOnInit(): void {}

  onSearch(): void {
    const val = this.searchForm.getRawValue();
    this.searching = true;
    this.searched = true;
    this.pageNumber = 1;
    this.doSearch(val);
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

  private doSearch(val: { lastName: string | null; firstName: string | null; memberNumber: string | null; idNumber: string | null }): void {
    this.searching = true;
    this.memberService.search({
      lastName: val.lastName || undefined,
      firstName: val.firstName || undefined,
      memberNumber: val.memberNumber || undefined,
      idNumber: val.idNumber || undefined,
      mainClientId: this.authService.activeClientId || undefined,
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

  onSelect(member: MemberDto): void {
    this.dialogRef.close(member);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
