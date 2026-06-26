import { Component, inject, OnInit, OnDestroy, ViewChildren, QueryList, ElementRef, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatDividerModule } from '@angular/material/divider';
import { Subject, takeUntil } from 'rxjs';
import { MemberService } from '../services/member.service';
import { MemberDto, CreateMemberRequest, MemberMedicalAidProductDto, CreateMemberMedicalAidProductRequest, MemberNoteDto, CreateMemberNoteRequest } from '../models/member.models';
import { MedicalAidService } from '../../medical-aids/services/medical-aid.service';
import { MedicalAidProductDto } from '../../medical-aids/models/medical-aid.models';
import { AuthService } from '../../../core/services/auth.service';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';

interface SectionLink {
  id: string;
  label: string;
}

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatDividerModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.scss']
})
export class MemberDetailComponent implements OnInit, OnDestroy, AfterViewInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly memberService = inject(MemberService);
  private readonly medicalAidService = inject(MedicalAidService);
  private readonly authService = inject(AuthService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  @ViewChildren('sectionEl') sectionElements!: QueryList<ElementRef>;

  memberData: MemberDto | null = null;
  memberId!: number;
  loading = true;
  saving = false;
  isNew = false;
  activeSection = 'personal';

  // Reference data for medical aid dropdown
  medicalAids: { id: number; name: string }[] = [];
  availableProducts: MedicalAidProductDto[] = [];

  // Sub-entities
  memberProducts: MemberMedicalAidProductDto[] = [];
  memberNotes: MemberNoteDto[] = [];
  productDisplayedColumns = ['medicalAidProductName', 'dateFrom', 'dateTo', 'actions'];
  noteDisplayedColumns = ['note', 'noteDate', 'createdBy', 'actions'];

  // Sub-entity forms
  showProductForm = false;
  editingProductId: number | null = null;
  showNoteForm = false;
  editingNoteId: number | null = null;

  sections: SectionLink[] = [
    { id: 'personal', label: 'Personal Information' },
    { id: 'medical-aid', label: 'Medical Aid' },
    { id: 'logistics', label: 'Logistics & Address' },
    { id: 'next-of-kin', label: 'Next of Kin & Employment' },
    { id: 'products', label: 'Medical Aid Products' },
    { id: 'suspension', label: 'Suspension Details' },
    { id: 'notes', label: 'Member Notes' }
  ];

  // Main form
  form = this.fb.group({
    // Personal
    genderId: [null as number | null],
    titleId: [null as number | null],
    lastName: ['', Validators.required],
    firstName: ['', Validators.required],
    initials: [''],
    idNumber: [''],
    marritalStatusId: [null as number | null],
    languageId: [null as number | null],
    dateOfBirth: [null as Date | null],
    isPensioner: [false],
    isMbodRma: [false],
    raceId: [null as number | null],

    // Medical Aid
    medicalAidId: [null as number | null],
    memberStatusId: [null as number | null],
    memberNumber: ['', Validators.required],
    dateOfBenefit: [null as Date | null],
    dateJoined: [null as Date | null],
    isMedAidExhausted: [false],
    dateExhausted: [null as Date | null],
    isWaitingPeriod: [false],
    isReinstated: [false],
    dateReinstated: [null as Date | null],
    isDeceased: [false],
    dateDeceased: [null as Date | null],

    // Logistics
    countryId: [null as number | null],
    passportNumber: [''],
    passportExpiryDate: [null as Date | null],
    periodInCountryId: [null as number | null],
    addressLine1: [''],
    addressLine2: [''],
    addressLine3: [''],
    addressCode: [''],
    phoneNumber: [''],
    cellNumber: [''],

    // Next of Kin & Employer
    dependents: [''],
    nextOfKin: [''],
    relationship: [''],
    contactNumber: [''],
    employerCountryId: [null as number | null],
    employerAddress: [''],
    employerAddressCode: [''],
    employerPhoneNumber: [''],

    // Suspension
    isSuspended: [false],
    dateSuspended: [null as Date | null],
    suspendReasonId: [null as number | null],

    // System
    allowServices: [true]
  });

  // Product sub-entity form
  productForm = this.fb.group({
    medicalAidProductId: [null as number | null, Validators.required],
    dateFrom: [null as Date | null, Validators.required],
    dateTo: [null as Date | null]
  });

  // Note sub-entity form
  noteForm = this.fb.group({
    note: ['', Validators.required],
    noteDate: [new Date() as Date | null]
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === 'new') {
      this.isNew = true;
      this.loading = false;
    } else if (idParam) {
      this.memberId = +idParam;
      this.loadMember();
    }

    this.loadMedicalAids();
    this.setupConditionalFields();
  }

  ngAfterViewInit(): void {
    // Observe sections for scroll-based active nav
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ─── Data Loading ────────────────────────────────────────────

  private loadMember(): void {
    this.loading = true;
    this.memberService.getById(this.memberId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.memberData = data;
          this.patchFormFromData();
          this.form.controls.memberNumber.disable();
          this.loading = false;
          this.loadSubEntities();
          this.loadAvailableProducts(data.medicalAidId);
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load member', 'Close', { duration: 3000 });
        }
      });
  }

  private loadMedicalAids(): void {
    const clientId = this.authService.activeClientId;
    this.medicalAidService.getActive(clientId ?? undefined)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (aids) => this.medicalAids = aids.map(a => ({ id: a.medicalAidId, name: a.medicalAidName || '' })),
        error: () => {}
      });
  }

  private loadAvailableProducts(medicalAidId: number | null): void {
    if (medicalAidId) {
      this.medicalAidService.getProducts(medicalAidId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (products) => this.availableProducts = products,
          error: () => {}
        });
    }
  }

  private loadSubEntities(): void {
    if (this.isNew) return;

    this.memberService.getMedicalAidProducts(this.memberId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => this.memberProducts = data,
        error: () => {}
      });

    this.memberService.getNotes(this.memberId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => this.memberNotes = data,
        error: () => {}
      });
  }

  // ─── Form Patching ──────────────────────────────────────────

  private patchFormFromData(): void {
    const m = this.memberData!;
    this.form.patchValue({
      genderId: m.genderId,
      titleId: m.titleId,
      lastName: m.lastName,
      firstName: m.firstName,
      initials: m.initials || '',
      idNumber: m.idNumber || '',
      marritalStatusId: m.marritalStatusId,
      languageId: m.languageId,
      dateOfBirth: m.dateOfBirth ? new Date(m.dateOfBirth) : null,
      isPensioner: m.isPensioner,
      isMbodRma: m.isMbodRma,
      raceId: m.raceId,

      medicalAidId: m.medicalAidId,
      memberStatusId: m.memberStatusId,
      memberNumber: m.memberNumber,
      dateOfBenefit: m.dateOfBenefit ? new Date(m.dateOfBenefit) : null,
      dateJoined: m.dateJoined ? new Date(m.dateJoined) : null,
      isMedAidExhausted: m.isMedAidExhausted,
      dateExhausted: m.dateExhausted ? new Date(m.dateExhausted) : null,
      isWaitingPeriod: m.isWaitingPeriod,
      isReinstated: m.isReinstated,
      dateReinstated: m.dateReinstated ? new Date(m.dateReinstated) : null,
      isDeceased: m.isDeceased,
      dateDeceased: m.dateDeceased ? new Date(m.dateDeceased) : null,

      countryId: m.countryId,
      passportNumber: m.passportNumber || '',
      passportExpiryDate: m.passportExpiryDate ? new Date(m.passportExpiryDate) : null,
      periodInCountryId: m.periodInCountryId,
      addressLine1: m.addressLine1 || '',
      addressLine2: m.addressLine2 || '',
      addressLine3: m.addressLine3 || '',
      addressCode: m.addressCode || '',
      phoneNumber: m.phoneNumber || '',
      cellNumber: m.cellNumber || '',

      dependents: m.dependents || '',
      nextOfKin: m.nextOfKin || '',
      relationship: m.relationship || '',
      contactNumber: m.contactNumber || '',
      employerCountryId: m.employerCountryId,
      employerAddress: m.employerAddress || '',
      employerAddressCode: m.employerAddressCode || '',
      employerPhoneNumber: m.employerPhoneNumber || '',

      isSuspended: m.isSuspended,
      dateSuspended: m.dateSuspended ? new Date(m.dateSuspended) : null,
      suspendReasonId: m.suspendReasonId,

      allowServices: m.allowServices
    });
  }

  // ─── Conditional Fields ─────────────────────────────────────

  private setupConditionalFields(): void {
    this.form.controls.isSuspended.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(val => {
      if (val) {
        this.form.controls.dateSuspended.enable();
        this.form.controls.suspendReasonId.enable();
      } else {
        this.form.controls.dateSuspended.disable();
        this.form.controls.suspendReasonId.disable();
        this.form.controls.dateSuspended.reset();
        this.form.controls.suspendReasonId.reset();
      }
    });

    this.form.controls.isMedAidExhausted.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(val => {
      if (val) {
        this.form.controls.dateExhausted.enable();
      } else {
        this.form.controls.dateExhausted.disable();
        this.form.controls.dateExhausted.reset();
      }
    });

    this.form.controls.isReinstated.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(val => {
      if (val) {
        this.form.controls.dateReinstated.enable();
      } else {
        this.form.controls.dateReinstated.disable();
        this.form.controls.dateReinstated.reset();
      }
    });

    this.form.controls.isDeceased.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(val => {
      if (val) {
        this.form.controls.dateDeceased.enable();
      } else {
        this.form.controls.dateDeceased.disable();
        this.form.controls.dateDeceased.reset();
      }
    });

    // Medical Aid change → reload available products
    this.form.controls.medicalAidId.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(val => {
      this.loadAvailableProducts(val);
    });

    // Set initial disabled states
    if (!this.form.controls.isSuspended.value) {
      this.form.controls.dateSuspended.disable();
      this.form.controls.suspendReasonId.disable();
    }
    if (!this.form.controls.isMedAidExhausted.value) {
      this.form.controls.dateExhausted.disable();
    }
    if (!this.form.controls.isReinstated.value) {
      this.form.controls.dateReinstated.disable();
    }
    if (!this.form.controls.isDeceased.value) {
      this.form.controls.dateDeceased.disable();
    }
  }

  // ─── Navigation ─────────────────────────────────────────────

  scrollToSection(sectionId: string): void {
    this.activeSection = sectionId;
    const el = document.getElementById(sectionId);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  onBackToList(): void {
    this.router.navigate(['/members']);
  }

  // ─── Save / Delete ──────────────────────────────────────────

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.snackBar.open('Please fill in all required fields (Name, Surname, Member Number)', 'Close', { duration: 4000 });
      return;
    }

    this.saving = true;
    const f = this.form.getRawValue();

    const request: CreateMemberRequest = {
      memberNumber: f.memberNumber!,
      firstName: f.firstName!,
      lastName: f.lastName!,
      initials: f.initials || null,
      dateOfBirth: f.dateOfBirth ? f.dateOfBirth.toISOString() : null,
      genderId: f.genderId,
      titleId: f.titleId,
      idNumber: f.idNumber || null,
      marritalStatusId: f.marritalStatusId,
      languageId: f.languageId,
      raceId: f.raceId,
      isPensioner: f.isPensioner ?? false,
      isMbodRma: f.isMbodRma ?? false,

      medicalAidId: f.medicalAidId,
      medicalAidProductId: null,
      memberStatusId: f.memberStatusId,
      dateOfBenefit: f.dateOfBenefit ? f.dateOfBenefit.toISOString() : null,
      dateJoined: f.dateJoined ? f.dateJoined.toISOString() : null,
      isMedAidExhausted: f.isMedAidExhausted ?? false,
      dateExhausted: f.dateExhausted ? f.dateExhausted.toISOString() : null,
      isWaitingPeriod: f.isWaitingPeriod ?? false,
      isReinstated: f.isReinstated ?? false,
      dateReinstated: f.dateReinstated ? f.dateReinstated.toISOString() : null,
      isDeceased: f.isDeceased ?? false,
      dateDeceased: f.dateDeceased ? f.dateDeceased.toISOString() : null,

      countryId: f.countryId,
      passportNumber: f.passportNumber || null,
      passportExpiryDate: f.passportExpiryDate ? f.passportExpiryDate.toISOString() : null,
      periodInCountryId: f.periodInCountryId,
      addressLine1: f.addressLine1 || null,
      addressLine2: f.addressLine2 || null,
      addressLine3: f.addressLine3 || null,
      addressCode: f.addressCode || null,
      phoneNumber: f.phoneNumber || null,
      cellNumber: f.cellNumber || null,

      dependents: f.dependents || null,
      nextOfKin: f.nextOfKin || null,
      relationship: f.relationship || null,
      contactNumber: f.contactNumber || null,
      employerCountryId: f.employerCountryId,
      employerAddress: f.employerAddress || null,
      employerAddressCode: f.employerAddressCode || null,
      employerPhoneNumber: f.employerPhoneNumber || null,

      isSuspended: f.isSuspended ?? false,
      dateSuspended: f.dateSuspended ? f.dateSuspended.toISOString() : null,
      suspendReasonId: f.suspendReasonId,

      allowServices: f.allowServices ?? true
    };

    const obs = this.isNew
      ? this.memberService.create(request)
      : this.memberService.update(this.memberId, request);

    obs.pipe(takeUntil(this.destroy$)).subscribe({
      next: (result) => {
        this.saving = false;
        this.memberData = result;
        if (this.isNew) {
          this.isNew = false;
          this.memberId = result.id;
          this.router.navigate(['/members', result.id], { replaceUrl: true });
          this.form.controls.memberNumber.disable();
          this.loadSubEntities();
        }
        this.snackBar.open('Member saved successfully', 'Close', { duration: 3000 });
      },
      error: (err) => {
        this.saving = false;
        const message = err?.error?.message || 'Failed to save member';
        this.snackBar.open(message, 'Close', { duration: 4000 });
      }
    });
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

  // ─── Medical Aid Products Sub-Entity ─────────────────────────

  onAddProduct(): void {
    this.showProductForm = true;
    this.editingProductId = null;
    this.productForm.reset();
  }

  onEditProduct(item: MemberMedicalAidProductDto): void {
    this.showProductForm = true;
    this.editingProductId = item.id;
    this.productForm.patchValue({
      medicalAidProductId: item.medicalAidProductId,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null
    });
  }

  onCancelProduct(): void {
    this.showProductForm = false;
    this.editingProductId = null;
    this.productForm.reset();
  }

  onSaveProduct(): void {
    if (this.productForm.invalid) return;

    const f = this.productForm.value;
    const request: CreateMemberMedicalAidProductRequest = {
      medicalAidProductId: f.medicalAidProductId!,
      dateFrom: f.dateFrom ? f.dateFrom.toISOString() : null,
      dateTo: f.dateTo ? f.dateTo.toISOString() : null
    };

    const obs = this.editingProductId
      ? this.memberService.updateMedicalAidProduct(this.memberId, this.editingProductId, request)
      : this.memberService.createMedicalAidProduct(this.memberId, request);

    obs.pipe(takeUntil(this.destroy$)).subscribe({
      next: () => {
        this.showProductForm = false;
        this.editingProductId = null;
        this.productForm.reset();
        this.loadSubEntities();
        this.snackBar.open('Product saved', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save product', 'Close', { duration: 3000 })
    });
  }

  onDeleteProduct(item: MemberMedicalAidProductDto): void {
    if (confirm('Remove this product history entry?')) {
      this.memberService.deleteMedicalAidProduct(this.memberId, item.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.loadSubEntities();
            this.snackBar.open('Product removed', 'Close', { duration: 3000 });
          },
          error: () => this.snackBar.open('Failed to delete product', 'Close', { duration: 3000 })
        });
    }
  }

  // ─── Notes Sub-Entity ───────────────────────────────────────

  onAddNote(): void {
    this.showNoteForm = true;
    this.editingNoteId = null;
    this.noteForm.reset({ note: '', noteDate: new Date() });
  }

  onEditNote(item: MemberNoteDto): void {
    this.showNoteForm = true;
    this.editingNoteId = item.id;
    this.noteForm.patchValue({
      note: item.note,
      noteDate: item.noteDate ? new Date(item.noteDate) : new Date()
    });
  }

  onCancelNote(): void {
    this.showNoteForm = false;
    this.editingNoteId = null;
    this.noteForm.reset();
  }

  onSaveNote(): void {
    if (this.noteForm.invalid) return;

    const f = this.noteForm.value;
    const request: CreateMemberNoteRequest = {
      note: f.note!,
      noteDate: f.noteDate ? f.noteDate.toISOString() : null
    };

    const obs = this.editingNoteId
      ? this.memberService.updateNote(this.memberId, this.editingNoteId, request)
      : this.memberService.createNote(this.memberId, request);

    obs.pipe(takeUntil(this.destroy$)).subscribe({
      next: () => {
        this.showNoteForm = false;
        this.editingNoteId = null;
        this.noteForm.reset();
        this.loadSubEntities();
        this.snackBar.open('Note saved', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save note', 'Close', { duration: 3000 })
    });
  }

  onDeleteNote(item: MemberNoteDto): void {
    if (confirm('Are you sure you want to delete this note?')) {
      this.memberService.deleteNote(this.memberId, item.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.loadSubEntities();
            this.snackBar.open('Note deleted', 'Close', { duration: 3000 });
          },
          error: () => this.snackBar.open('Failed to delete note', 'Close', { duration: 3000 })
        });
    }
  }
}
