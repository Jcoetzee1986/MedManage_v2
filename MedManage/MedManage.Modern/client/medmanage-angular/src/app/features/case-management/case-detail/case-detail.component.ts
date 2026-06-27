import { Component, inject, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { Subject, takeUntil, interval, debounceTime, distinctUntilChanged, switchMap, of } from 'rxjs';
import { CaseService } from '../services/case.service';
import { CaseDto, CreateCaseRequest, UpdateCaseRequest } from '../models/case.models';
import { MemberService } from '../../members/services/member.service';
import { MemberDto } from '../../members/models/member.models';
import { ProviderService } from '../../providers/services/provider.service';
import { ProviderAutocompleteResult, ProviderDto } from '../../providers/models/provider.models';
import { ReportService } from '../../reports/services/report.service';
import { ReferenceDataService } from '../../../core/services/reference-data.service';
import { ReferenceDataItem } from '../../../core/models/reference-data.models';
import { MemberLookupDialogComponent } from '../../../shared/components/member-lookup-dialog/member-lookup-dialog.component';
import { ProviderLookupDialogComponent } from '../../../shared/components/provider-lookup-dialog/provider-lookup-dialog.component';
import { HasRoleDirective } from '../../../shared/directives/has-role.directive';
import { CaseMemberTabComponent } from '../tabs/case-member-tab/case-member-tab.component';
import { CaseProviderTabComponent } from '../tabs/case-provider-tab/case-provider-tab.component';
import { CaseDatesTabComponent } from '../tabs/case-dates-tab/case-dates-tab.component';
import { CaseCptTabComponent } from '../tabs/case-cpt-tab/case-cpt-tab.component';
import { CaseIcdTabComponent } from '../tabs/case-icd-tab/case-icd-tab.component';
import { CaseTariffsTabComponent } from '../tabs/case-tariffs-tab/case-tariffs-tab.component';
//import { CaseFacilityTypesTabComponent } from '../tabs/case-facility-types-tab/case-facility-types-tab.component';
import { CaseExclusionsTabComponent } from '../tabs/case-exclusions-tab/case-exclusions-tab.component';
import { CaseNappiTabComponent } from '../tabs/case-nappi-tab/case-nappi-tab.component';
import { CaseNotesTabComponent } from '../tabs/case-notes-tab/case-notes-tab.component';
import { CaseCommentsTabComponent } from '../tabs/case-comments-tab/case-comments-tab.component';
import { CaseChecklistTabComponent } from '../tabs/case-checklist-tab/case-checklist-tab.component';
import { CaseDocumentsTabComponent } from '../tabs/case-documents-tab/case-documents-tab.component';
import { CaseLinkedCasesTabComponent } from '../tabs/case-linked-cases-tab/case-linked-cases-tab.component';
import { CaseCopyDialogComponent } from '../case-copy-dialog/case-copy-dialog.component';

@Component({
  selector: 'app-case-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatAutocompleteModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    HasRoleDirective,
    CaseMemberTabComponent,
    CaseProviderTabComponent,
    CaseDatesTabComponent,
    CaseCptTabComponent,
    CaseIcdTabComponent,
    CaseTariffsTabComponent,
    //CaseFacilityTypesTabComponent,
    CaseExclusionsTabComponent,
    CaseNappiTabComponent,
    CaseNotesTabComponent,
    CaseCommentsTabComponent,
    CaseChecklistTabComponent,
    CaseDocumentsTabComponent,
    CaseLinkedCasesTabComponent
  ],
  templateUrl: './case-detail.component.html',
  styleUrls: ['./case-detail.component.scss']
})
export class CaseDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly memberService = inject(MemberService);
  private readonly providerService = inject(ProviderService);
  private readonly reportService = inject(ReportService);
  private readonly referenceDataService = inject(ReferenceDataService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  caseData: CaseDto | null = null;
  caseId!: number;
  isNewCase = false;
  loading = true;
  saving = false;
  activeTab = 'detail';

  tabs = [
    { id: 'detail', label: 'Case Detail' },
    { id: 'member', label: 'Member' },
    { id: 'provider', label: 'Provider' },
    { id: 'dates', label: 'Dates' },
    { id: 'cpt', label: 'CPT Codes' },
    { id: 'icd', label: 'ICD Codes' },
    { id: 'tariffs', label: 'Tariffs' },
    //{ id: 'facility', label: 'Facility Types' },
    { id: 'exclusions', label: 'Exclusions' },
    { id: 'nappi', label: 'NAPPI' },
    { id: 'notes', label: 'Notes' },
    { id: 'comments', label: 'Comments' },
    { id: 'checklist', label: 'Checklist' },
    { id: 'documents', label: 'Documents' },
    { id: 'linked', label: 'Linked Cases' }
  ];

  // Member lookup
  selectedMember: MemberDto | null = null;
  memberSearchResults: MemberDto[] = [];
  memberSearching = false;

  // Reference data for dropdowns
  caseStatuses: ReferenceDataItem[] = [];
  caseTypes: ReferenceDataItem[] = [];
  caseCategories: ReferenceDataItem[] = [];

  // Provider lookup (Refer To)
  referToResults: ProviderAutocompleteResult[] = [];
  selectedReferTo: ProviderDto | null = null;

  // Provider lookup (Refer From)
  referFromResults: ProviderAutocompleteResult[] = [];
  selectedReferFrom: ProviderDto | null = null;

  /** Heartbeat interval: send lock refresh every 5 minutes to prevent expiry */
  private readonly HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;
  private lockAcquired = false;

  // ─── Primary Form ──────────────────────────────────────────────

  form = this.fb.group({
    // Case Info
    authNumber: [''],
    accountNr: [''],
    caseStatusId: [null as number | null],
    caseTypeId: [null as number | null, Validators.required],
    caseCategoryId: [null as number | null, Validators.required],
    interimAmount: [null as number | null],
    finalInvoiceAmount: [null as number | null],
    penaltyPercentage: [null as number | null],
    finalInvoiceWithPenalty: [{ value: null as number | null, disabled: true }],
    wasBooking: [false],
    wcaIod: [false],
    // Member (ID stored separately)
    memberId: [null as number | null, Validators.required],
    memberSearch: [''],
    // Provider - Refer To
    referToId: [null as number | null, Validators.required],
    referToSearch: [''],
    // Provider - Refer From
    referFromId: [null as number | null],
    referFromSearch: [''],
    // Description
    description: ['']
  });

  get isFormValid(): boolean {
    return this.form.valid;
  }

  get totalLos(): number | null {
    if (this.caseData?.totalLos != null) return this.caseData.totalLos;
    if (this.caseData?.totalLengthOfStay != null) return this.caseData.totalLengthOfStay;
    return null;
  }

  /** Release lock when user closes/refreshes the browser tab */
  @HostListener('window:beforeunload')
  onBeforeUnload(): void {
    if (this.caseId && this.lockAcquired) {
      const url = `${(this.caseService as any).baseUrl}/${this.caseId}/lock`;
      fetch(url, {
        method: 'DELETE',
        keepalive: true,
        headers: { 'Authorization': `Bearer ${localStorage.getItem('access_token') || ''}` }
      }).catch(() => {});
    }
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === 'new' || !idParam) {
      this.isNewCase = true;
      this.loading = false;
      this.caseData = null;
    } else {
      this.caseId = +idParam;
      this.isNewCase = false;
      this.loadCase();
      this.startHeartbeat();
    }
    this.setupAutoCalculations();
    this.loadReferenceData();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.caseId && this.lockAcquired) {
      this.caseService.unlockCase(this.caseId).subscribe();
    }
  }

  // ─── Auto-Calculations ─────────────────────────────────────────

  private setupAutoCalculations(): void {
    // Recalculate Final Invoice With Penalty whenever finalInvoiceAmount or penaltyPercentage changes
    this.form.get('finalInvoiceAmount')!.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.recalculatePenalty());

    this.form.get('penaltyPercentage')!.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.recalculatePenalty());
  }

  private recalculatePenalty(): void {
    const finalInvoice = this.form.get('finalInvoiceAmount')?.value || 0;
    const penalty = this.form.get('penaltyPercentage')?.value || 0;
    const result = (1 - penalty / 100) * finalInvoice;
    this.form.get('finalInvoiceWithPenalty')?.setValue(
      Math.round(result * 100) / 100,
      { emitEvent: false }
    );
  }

  /** Called from Notes tab when interim amounts change (EventEmitter) */
  onInterimAmountsChanged(totalInterim: number): void {
    this.form.get('interimAmount')?.setValue(totalInterim);
  }

  // ─── Member Search ─────────────────────────────────────────────

  openMemberLookup(): void {
    const dialogRef = this.dialog.open(MemberLookupDialogComponent, {
      width: '700px'
    });
    dialogRef.afterClosed().subscribe((member: MemberDto | null) => {
      if (member) {
        this.onMemberSelected(member);
      }
    });
  }

  onMemberSelected(member: MemberDto): void {
    this.selectedMember = member;
    this.form.patchValue({
      memberId: member.id,
      memberSearch: `${member.lastName}, ${member.firstName} (${member.memberNumber})`
    });
    this.memberSearchResults = [];
  }

  clearMember(): void {
    this.selectedMember = null;
    this.form.patchValue({ memberId: null, memberSearch: '' });
  }

  // ─── Provider Search (Refer To / Refer From) ──────────────────

  openReferToLookup(): void {
    const dialogRef = this.dialog.open(ProviderLookupDialogComponent, {
      width: '700px'
    });
    dialogRef.afterClosed().subscribe((provider: ProviderDto | null) => {
      if (provider) {
        this.onReferToSelected(provider);
      }
    });
  }

  openReferFromLookup(): void {
    const dialogRef = this.dialog.open(ProviderLookupDialogComponent, {
      width: '700px'
    });
    dialogRef.afterClosed().subscribe((provider: ProviderDto | null) => {
      if (provider) {
        this.onReferFromSelected(provider);
      }
    });
  }

  onReferToSelected(provider: ProviderDto): void {
    this.selectedReferTo = provider;
    this.form.patchValue({
      referToId: provider.id,
      referToSearch: `${provider.practiceName} - ${provider.lastName}, ${provider.firstName}`
    });
    this.referToResults = [];
  }

  clearReferTo(): void {
    this.selectedReferTo = null;
    this.form.patchValue({ referToId: null, referToSearch: '' });
  }

  onReferFromSelected(provider: ProviderDto): void {
    this.selectedReferFrom = provider;
    this.form.patchValue({
      referFromId: provider.id,
      referFromSearch: `${provider.practiceName} - ${provider.lastName}, ${provider.firstName}`
    });
    this.referFromResults = [];
  }

  clearReferFrom(): void {
    this.selectedReferFrom = null;
    this.form.patchValue({ referFromId: null, referFromSearch: '' });
  }

  // ─── Data Loading ──────────────────────────────────────────────

  /** Send periodic heartbeats to keep the lock alive while the user is on the page */
  private startHeartbeat(): void {
    interval(this.HEARTBEAT_INTERVAL_MS)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        if (this.caseId && this.lockAcquired) {
          this.caseService.refreshLock(this.caseId).subscribe({
            error: () => {
              this.lockAcquired = false;
              this.snackBar.open(
                'Your lock on this case has expired. Another user may now edit it.',
                'Dismiss',
                { duration: 10000 }
              );
            }
          });
        }
      });
  }

  private loadReferenceData(): void {
    this.referenceDataService.getAll('case-status').subscribe(items => this.caseStatuses = items);
    this.referenceDataService.getAll('case-type').subscribe(items => this.caseTypes = items);
    this.referenceDataService.getAll('case-category').subscribe(items => this.caseCategories = items);
  }

  private loadCase(): void {
    this.loading = true;
    this.caseService.getById(this.caseId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.caseData = data;
          this.loading = false;
          this.populateForm(data);
          // Acquire lock (non-blocking — allow viewing even if lock fails)
          this.caseService.lockCase(this.caseId).subscribe({
            next: () => { this.lockAcquired = true; },
            error: (err) => {
              // Only show the warning if it's a genuine conflict (409), not auth issues
              if (err.status === 409) {
                this.snackBar.open(
                  'This case is currently being edited by another user. Your changes may not be saved.',
                  'Close',
                  { duration: 5000 }
                );
              }
            }
          });
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load case', 'Close', { duration: 3000 });
        }
      });
  }

  private populateForm(data: CaseDto): void {
    this.form.patchValue({
      authNumber: data.authNumber || '',
      accountNr: data.accountNr || '',
      caseStatusId: data.caseStatusId ?? (data as any).statusId ?? null,
      caseTypeId: data.caseTypeId ?? (data as any).authTypeId ?? null,
      caseCategoryId: data.caseCategoryId || null,
      interimAmount: data.interimAmount ?? null,
      finalInvoiceAmount: data.finalInvoiceAmount ?? null,
      penaltyPercentage: data.penaltyPercentage ?? null,
      wasBooking: data.wasBooking ?? false,
      wcaIod: data.wcaIod ?? false,
      memberId: data.memberId || null,
      memberSearch: data.memberSurname
        ? `${data.memberSurname}, ${data.memberName || ''} (${data.memberNumber || ''})`
        : (data.memberName || ''),
      referToId: data.referToId || null,
      referToSearch: data.referToPracticeName
        ? `${data.referToPracticeName} - ${data.referToPersonSurname || ''}, ${data.referToPersonName || ''}`
        : '',
      referFromId: data.referFromId || null,
      referFromSearch: data.referFromPracticeName
        ? `${data.referFromPracticeName} - ${data.referFromPersonSurname || ''}, ${data.referFromPersonName || ''}`
        : '',
      description: data.description || data.caseDescription || ''
    });
    this.recalculatePenalty();
  }

  // ─── Save / Validation ─────────────────────────────────────────

  onSave(): void {
    // Run validation
    const errors = this.validateCase();
    if (errors.length > 0) {
      this.snackBar.open(errors.join(' | '), 'Close', { duration: 6000 });
      return;
    }

    this.saving = true;

    if (this.isNewCase) {
      this.saveNewCase();
    } else {
      this.saveExistingCase();
    }
  }

  private validateCase(): string[] {
    const errors: string[] = [];
    const val = this.form.getRawValue();

    if (!val.memberId) {
      errors.push('Member must be selected');
    }
    if (!val.referToId) {
      errors.push('Refer To practice must be selected');
    }
    if (!val.caseTypeId) {
      errors.push('Case Type is required');
    }
    if (!val.caseCategoryId) {
      errors.push('Case Category is required');
    }
    if (val.interimAmount != null && isNaN(Number(val.interimAmount))) {
      errors.push('Interim Amount must be a valid number');
    }
    if (val.finalInvoiceAmount != null && isNaN(Number(val.finalInvoiceAmount))) {
      errors.push('Final Invoice Amount must be a valid number');
    }
    return errors;
  }

  private buildRequest(): CreateCaseRequest {
    const val = this.form.getRawValue();
    return {
      authNumber: val.authNumber || undefined,
      accountNr: val.accountNr || undefined,
      caseStatusId: val.caseStatusId || undefined,
      caseTypeId: val.caseTypeId || undefined,
      caseCategoryId: val.caseCategoryId || undefined,
      memberId: val.memberId || undefined,
      referToId: val.referToId || undefined,
      referFromId: val.referFromId || undefined,
      interimAmount: val.interimAmount ?? undefined,
      finalInvoiceAmount: val.finalInvoiceAmount ?? undefined,
      penaltyPercentage: val.penaltyPercentage ?? undefined,
      wasBooking: val.wasBooking || undefined,
      wcaIod: val.wcaIod || undefined,
      dateAdmitted: this.caseData?.dateAdmitted || this.caseData?.admissionDate || undefined,
      admissionTime: this.caseData?.admissionTime || undefined,
      dateDischarged: this.caseData?.dateDischarged || this.caseData?.dischargeDate || undefined,
      dischargeTime: this.caseData?.dischargeTime || undefined,
      description: val.description || undefined,
      isBooking: false
    };
  }

  private saveNewCase(): void {
    const request = this.buildRequest();

    // Check for duplicates first
    this.caseService.checkDuplicate(request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          if (result.isDuplicate) {
            const proceed = confirm(
              `${result.message || 'A similar case already exists'} (Case #${result.existingCaseNumber}). Do you still want to create this case?`
            );
            if (!proceed) {
              this.saving = false;
              return;
            }
          }
          this.doCreate(request);
        },
        error: () => {
          // If duplicate check fails, proceed with creation anyway
          this.doCreate(request);
        }
      });
  }

  private doCreate(request: CreateCaseRequest): void {
    this.caseService.create(request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (newCase) => {
          this.saving = false;
          this.isNewCase = false;
          this.caseId = newCase.id;
          this.caseData = newCase;
          this.populateForm(newCase);
          this.snackBar.open(`Case #${newCase.caseNumber} created successfully`, 'Close', { duration: 3000 });
          // Navigate to the new case URL
          this.router.navigate(['/cases', newCase.id], { replaceUrl: true });
        },
        error: () => {
          this.saving = false;
          this.snackBar.open('Failed to create case', 'Close', { duration: 3000 });
        }
      });
  }

  private saveExistingCase(): void {
    const request: UpdateCaseRequest = {
      id: this.caseId,
      ...this.buildRequest()
    };

    this.caseService.update(this.caseId, request)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (updated) => {
          this.saving = false;
          this.caseData = updated;
          this.populateForm(updated);
          this.snackBar.open('Case saved successfully', 'Close', { duration: 3000 });
        },
        error: () => {
          this.saving = false;
          this.snackBar.open('Failed to save case', 'Close', { duration: 3000 });
        }
      });
  }

  // ─── Toolbar Actions ───────────────────────────────────────────

  onClose(): void {
    this.router.navigate(['/cases']);
  }

  onPrintLetter(): void {
    if (!this.caseId) return;
    this.reportService.generateCaseLetter({ caseId: this.caseId }).subscribe({
      next: (blob) => {
        this.reportService.downloadBlob(blob, `case-letter-${this.caseData?.caseNumber || this.caseId}.pdf`);
        this.snackBar.open('Case letter generated', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to generate case letter. Ensure ICD codes are assigned.', 'Close', { duration: 5000 })
    });
  }

  onCreateBooking(): void {
    this.router.navigate(['/bookings/new'], { queryParams: { caseId: this.caseId } });
  }

  onCopyCase(): void {
    const dialogRef = this.dialog.open(CaseCopyDialogComponent, {
      width: '500px',
      data: {
        caseId: this.caseId,
        caseNumber: this.caseData?.authNumber || this.caseData?.caseNumber,
        admissionDate: this.caseData?.admissionDate
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.caseService.copyCase(this.caseId, result)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (newCase) => {
              this.snackBar.open(`Case copied successfully. New case #${newCase.caseNumber}`, 'View', { duration: 5000 })
                .onAction().subscribe(() => {
                  this.router.navigate(['/cases', newCase.id]);
                });
            },
            error: () => {
              this.snackBar.open('Failed to copy case', 'Close', { duration: 3000 });
            }
          });
      }
    });
  }

  // ─── Tab Access ─────────────────────────────────────────────────

  get tabsEnabled(): boolean {
    return !this.isNewCase;
  }
}
