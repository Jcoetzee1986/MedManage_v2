import { Component, Input, Output, EventEmitter, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MemberService } from '../../services/member.service';
import { MemberDto, CreateMemberRequest } from '../../models/member.models';

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
    MatCheckboxModule
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

  saving = false;

  form = this.fb.group({
    memberNumber: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dateOfBirth: [null as Date | null],
    idNumber: [''],
    contactNumber: [''],
    email: ['', Validators.email],
    address: [''],
    allowServices: [true]
  });

  ngOnInit(): void {
    if (this.memberData) {
      this.form.patchValue({
        memberNumber: this.memberData.memberNumber,
        firstName: this.memberData.firstName,
        lastName: this.memberData.lastName,
        dateOfBirth: this.memberData.dateOfBirth ? new Date(this.memberData.dateOfBirth) : null,
        idNumber: this.memberData.idNumber || '',
        contactNumber: this.memberData.contactNumber || '',
        email: this.memberData.email || '',
        address: this.memberData.address || '',
        allowServices: this.memberData.allowServices
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const formValue = this.form.value;
    const request: CreateMemberRequest = {
      memberNumber: formValue.memberNumber!,
      firstName: formValue.firstName!,
      lastName: formValue.lastName!,
      dateOfBirth: formValue.dateOfBirth ? formValue.dateOfBirth.toISOString() : null,
      idNumber: formValue.idNumber || null,
      contactNumber: formValue.contactNumber || null,
      email: formValue.email || null,
      address: formValue.address || null,
      allowServices: formValue.allowServices ?? true
    };

    const obs = this.isNew
      ? this.memberService.create(request)
      : this.memberService.update(this.memberData!.id, request);

    obs.subscribe({
      next: (result) => {
        this.saving = false;
        this.saved.emit(result);
      },
      error: () => {
        this.saving = false;
      }
    });
  }
}
