import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatListModule } from '@angular/material/list';
import { MatChipsModule } from '@angular/material/chips';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { EpisodeService } from '../services/episode.service';
import {
  EpisodeDto,
  EpisodeCaseDto,
  CreateEpisodeDto,
  UpdateEpisodeDto,
  LinkCaseToEpisodeDto
} from '../models/episode.models';

@Component({
  selector: 'app-episode-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatSnackBarModule,
    MatDividerModule,
    MatListModule,
    MatChipsModule,
    MatTableModule,
    MatTooltipModule
  ],
  templateUrl: './episode-detail.component.html',
  styleUrls: ['./episode-detail.component.scss']
})
export class EpisodeDetailComponent implements OnInit {
  private readonly episodeService = inject(EpisodeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  episodeId: number | null = null;
  isNew = false;
  loading = false;
  linkedCases: EpisodeCaseDto[] = [];

  /** Table columns for linked cases */
  caseColumns = ['caseId', 'dateCreated', 'dateInserted', 'actions'];

  episodeForm = this.fb.group({
    episodeDescription: ['', Validators.required],
    memberId: [null as number | null],
    dateCreated: ['']
  });

  linkCaseForm = this.fb.group({
    caseId: [null as number | null, Validators.required]
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === 'new') {
      this.isNew = true;
    } else {
      this.episodeId = Number(idParam);
      this.loadEpisode();
      this.loadLinkedCases();
    }
  }

  loadEpisode(): void {
    if (!this.episodeId) return;
    this.loading = true;

    this.episodeService.getById(this.episodeId).subscribe({
      next: (episode) => {
        this.episodeForm.patchValue({
          episodeDescription: episode.episodeDescription || '',
          memberId: episode.memberId,
          dateCreated: episode.dateCreated || ''
        });
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load episode', 'Close', { duration: 3000 });
        this.loading = false;
      }
    });
  }

  loadLinkedCases(): void {
    if (!this.episodeId) return;

    this.episodeService.getCases(this.episodeId).subscribe({
      next: (cases) => {
        this.linkedCases = cases;
      },
      error: () => {
        this.snackBar.open('Failed to load linked cases', 'Close', { duration: 3000 });
      }
    });
  }

  onSave(): void {
    if (this.episodeForm.invalid) {
      this.snackBar.open('Please fill in required fields', 'Close', { duration: 3000 });
      return;
    }

    const formValue = this.episodeForm.value;

    if (this.isNew) {
      const dto: CreateEpisodeDto = {
        episodeDescription: formValue.episodeDescription,
        memberId: formValue.memberId,
        dateCreated: formValue.dateCreated || undefined
      };

      this.episodeService.create(dto).subscribe({
        next: (created) => {
          this.snackBar.open('Episode created', 'Close', { duration: 3000 });
          this.router.navigate(['/episodes', created.episodeId]);
        },
        error: () => {
          this.snackBar.open('Failed to create episode', 'Close', { duration: 3000 });
        }
      });
    } else {
      const dto: UpdateEpisodeDto = {
        episodeId: this.episodeId!,
        episodeDescription: formValue.episodeDescription,
        memberId: formValue.memberId,
        dateCreated: formValue.dateCreated || undefined
      };

      this.episodeService.update(this.episodeId!, dto).subscribe({
        next: () => {
          this.snackBar.open('Episode updated', 'Close', { duration: 3000 });
        },
        error: () => {
          this.snackBar.open('Failed to update episode', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onLinkCase(): void {
    if (this.linkCaseForm.invalid || !this.episodeId) return;

    const caseId = this.linkCaseForm.value.caseId!;
    const dto: LinkCaseToEpisodeDto = { caseId };

    this.episodeService.linkCase(this.episodeId, dto).subscribe({
      next: () => {
        this.snackBar.open(`Case ${caseId} linked to episode`, 'Close', { duration: 3000 });
        this.linkCaseForm.reset();
        this.loadLinkedCases();
      },
      error: (err) => {
        const message = err.error?.message || 'Failed to link case';
        this.snackBar.open(message, 'Close', { duration: 3000 });
      }
    });
  }

  onUnlinkCase(episodeCase: EpisodeCaseDto): void {
    if (!this.episodeId) return;

    if (!confirm(`Unlink Case #${episodeCase.caseId} from this episode?`)) {
      return;
    }

    this.episodeService.unlinkCase(this.episodeId, episodeCase.caseId).subscribe({
      next: () => {
        this.snackBar.open(`Case ${episodeCase.caseId} unlinked`, 'Close', { duration: 3000 });
        this.loadLinkedCases();
      },
      error: () => {
        this.snackBar.open('Failed to unlink case', 'Close', { duration: 3000 });
      }
    });
  }

  onBack(): void {
    this.router.navigate(['/episodes']);
  }
}
