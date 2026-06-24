import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject, takeUntil } from 'rxjs';
import { MemberService } from '../services/member.service';
import { MemberDto } from '../models/member.models';
import { MemberBasicTabComponent } from '../tabs/member-basic-tab/member-basic-tab.component';
import { MemberChronicIllnessTabComponent } from '../tabs/member-chronic-illness-tab/member-chronic-illness-tab.component';
import { MemberProductsTabComponent } from '../tabs/member-products-tab/member-products-tab.component';
import { MemberNotesTabComponent } from '../tabs/member-notes-tab/member-notes-tab.component';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSnackBarModule,
    MemberBasicTabComponent,
    MemberChronicIllnessTabComponent,
    MemberProductsTabComponent,
    MemberNotesTabComponent
  ],
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly memberService = inject(MemberService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  memberData: MemberDto | null = null;
  memberId!: number;
  loading = true;
  isNew = false;

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === 'new') {
      this.isNew = true;
      this.loading = false;
    } else if (idParam) {
      this.memberId = +idParam;
      this.loadMember();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadMember(): void {
    this.loading = true;
    this.memberService.getById(this.memberId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.memberData = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load member', 'Close', { duration: 3000 });
        }
      });
  }

  onBackToList(): void {
    this.router.navigate(['/members']);
  }

  onDeleteMember(): void {
    if (confirm('Are you sure you want to delete this member?')) {
      this.memberService.delete(this.memberId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Member deleted', 'Close', { duration: 3000 });
            this.router.navigate(['/members']);
          },
          error: () => {
            this.snackBar.open('Failed to delete member', 'Close', { duration: 3000 });
          }
        });
    }
  }

  onMemberSaved(member: MemberDto): void {
    this.memberData = member;
    if (this.isNew) {
      this.isNew = false;
      this.memberId = member.id;
      this.router.navigate(['/members', member.id], { replaceUrl: true });
    }
    this.snackBar.open('Member saved successfully', 'Close', { duration: 3000 });
  }
}
