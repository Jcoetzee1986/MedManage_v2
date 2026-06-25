import { Component, Input, Output, EventEmitter, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { ReferenceDataDropdownComponent } from '../../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { MemberService } from '../../services/member.service';
import { MemberDto, CreateMemberRequest } from '../../models/member.models';
import { MedicalAidService } from '../../../medical-aids/services/medical-aid.service';

@Component({
  selector: 'app-member-basic-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCheckboxModule,
    MatSelectModule,
    MatSnackBarModule,
    MatDividerModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './member-basic-tab.component.html',
  styleUrls: ['./member-basic-tab.component.scss']
})
export class MemberBasicTabComponent implements OnInit {
  @Input() memberData: MemberDto | null = null;
  @Input() isNew = false;
  @Output() saved = new EventEmitter<MemberDto>();

  private readonly fb = inject(FormBuilder);
  private readonly memberService = inject(MemberService);
  private readonly medicalAidService = inject(MedicalAidService);
  private readonly snackBar = inject(MatSnackBar);

  saving = false;
  medicalAids: { id: number; name: string }[] = [];

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

    // Other (Next of Kin & Employer)
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

  ngOnInit(): void {
    this.loadMedicalAids();

    if (this.memberData) {
      this.patchFormFromData();
      // Disable memberNumber on edit
      this.form.controls.memberNumber.disable();
    }

    // Setup conditional field watchers
    this.setupConditionalFields();
  }

  private loadMedicalAids(): void {
    this.medicalAidService.getActive().subscribe({
      next: (aids) => this.medicalAids = aids.map(a => ({ id: a.medicalAidId, name: a.medicalAidName || '' })),
      error: () => {}
    });
  }

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

  private setupConditionalFields(): void {
    // Suspended → enable/disable date + reason
    this.form.controls.isSuspended.valueChanges.subscribe(val => {
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

    // Med Aid Exhausted → enable/disable date
    this.form.controls.isMedAidExhausted.valueChanges.subscribe(val => {
      if (val) {
        this.form.controls.dateExhausted.enable();
      } else {
        this.form.controls.dateExhausted.disable();
        this.form.controls.dateExhausted.reset();
      }
    });

    // Reinstated → enable/disable date
    this.form.controls.isReinstated.valueChanges.subscribe(val => {
      if (val) {
        this.form.controls.dateReinstated.enable();
      } else {
        this.form.controls.dateReinstated.disable();
        this.form.controls.dateReinstated.reset();
      }
    });

    // Deceased → enable/disable date
    this.form.controls.isDeceased.valueChanges.subscribe(val => {
      if (val) {
        this.form.controls.dateDeceased.enable();
      } else {
        this.form.controls.dateDeceased.disable();
        this.form.controls.dateDeceased.reset();
      }
    });

    // Set initial disabled states based on current values
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

  onSave(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      this.snackBar.open('Please fill in all required fields (Name, Surname, Member Number)', 'Close', { duration: 4000 });
      return;
    }

    this.saving = true;
    const f = this.form.getRawValue(); // getRawValue to include disabled fields

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
      medicalAidProductId: null, // managed via products tab
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
      : this.memberService.update(this.memberData!.id, request);

    obs.subscribe({
      next: (result) => {
        this.saving = false;
        this.saved.emit(result);
      },
      error: (err) => {
        this.saving = false;
        const message = err?.error?.message || 'Failed to save member';
        this.snackBar.open(message, 'Close', { duration: 4000 });
      }
    });
  }

  onCancel(): void {
    // Navigate back handled by parent
    window.history.back();
  }
}
