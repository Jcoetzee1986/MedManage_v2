import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { CaseDto } from '../../models/case.models';

@Component({
  selector: 'app-case-provider-tab',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './case-provider-tab.component.html',
  styleUrls: ['./case-provider-tab.component.scss']
})
export class CaseProviderTabComponent {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;
}
