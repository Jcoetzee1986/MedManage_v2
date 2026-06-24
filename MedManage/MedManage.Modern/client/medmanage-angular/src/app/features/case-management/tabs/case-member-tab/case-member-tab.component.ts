import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { CaseDto } from '../../models/case.models';

@Component({
  selector: 'app-case-member-tab',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './case-member-tab.component.html',
  styleUrls: ['./case-member-tab.component.scss']
})
export class CaseMemberTabComponent {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;
}
