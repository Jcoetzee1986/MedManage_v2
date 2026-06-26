import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { Router } from '@angular/router';
import { CaseDto } from '../../models/case.models';
import { MemberService } from '../../../members/services/member.service';
import { MemberDto } from '../../../members/models/member.models';
import { NavigationContextService } from '../../../../core/services/navigation-context.service';

@Component({
  selector: 'app-case-member-tab',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './case-member-tab.component.html',
  styleUrls: ['./case-member-tab.component.scss']
})
export class CaseMemberTabComponent implements OnInit {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;

  private readonly memberService = inject(MemberService);
  private readonly router = inject(Router);
  private readonly navContext = inject(NavigationContextService);

  member: MemberDto | null = null;
  loading = false;

  ngOnInit(): void {
    if (this.caseData?.memberId) {
      this.loadMember();
    }
  }

  private loadMember(): void {
    this.loading = true;
    this.memberService.getById(this.caseData.memberId!).subscribe({
      next: (m) => {
        this.member = m;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  openMemberDetail(): void {
    if (this.caseData?.memberId) {
      // Set return context so the member detail "Back" button returns here
      this.navContext.setReturnUrl(`/cases/${this.caseId}`);
      this.router.navigate(['/members', this.caseData.memberId]);
    }
  }
}
