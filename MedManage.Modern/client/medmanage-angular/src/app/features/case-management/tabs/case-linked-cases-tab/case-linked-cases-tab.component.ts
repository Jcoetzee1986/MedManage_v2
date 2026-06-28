import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseLinkDto, CreateCaseLinkRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-linked-cases-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './case-linked-cases-tab.component.html',
  styleUrls: ['./case-linked-cases-tab.component.scss']
})
export class CaseLinkedCasesTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly router = inject(Router);

  items: CaseLinkDto[] = [];
  displayedColumns = ['authNumber', 'admissionDate', 'caseStatus', 'caseType', 'caseCategory', 'referToPractice', 'memberName', 'relationship', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    childCaseId: [null as number | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getLinks(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load linked cases', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const childCaseId = this.addForm.value.childCaseId;
    if (!childCaseId) return;

    const request: CreateCaseLinkRequest = { childCase: childCaseId };

    this.caseService.createLink(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('Case linked', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to link case', 'Close', { duration: 3000 })
    });
  }

  onNavigate(item: CaseLinkDto): void {
    this.router.navigate(['/cases', item.caseId]);
  }

  onDelete(item: CaseLinkDto): void {
    if (confirm('Remove this link?')) {
      this.caseService.deleteLink(this.caseId, item.caseId).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Link removed', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to remove link', 'Close', { duration: 3000 })
      });
    }
  }
}
