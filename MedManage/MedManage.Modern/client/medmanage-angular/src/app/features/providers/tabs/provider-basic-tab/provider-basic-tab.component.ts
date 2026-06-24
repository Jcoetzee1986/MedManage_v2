import { Component, Input, Output, EventEmitter, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { ProviderService } from '../../services/provider.service';
import { ProviderDto, CreateProviderRequest } from '../../models/provider.models';

@Component({
  selector: 'app-provider-basic-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule
  ],
  templateUrl: './provider-basic-tab.component.html',
  styleUrls: ['./provider-basic-tab.component.scss']
})
export class ProviderBasicTabComponent implements OnInit {
  @Input() providerData: ProviderDto | null = null;
  @Input() isNew = false;
  @Output() saved = new EventEmitter<ProviderDto>();

  private readonly fb = inject(FormBuilder);
  private readonly providerService = inject(ProviderService);

  saving = false;

  form = this.fb.group({
    providerNumber: ['', Validators.required],
    practiceName: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    contactNumber: [''],
    email: ['', Validators.email],
    address: [''],
    bHFNumber: [''],
    hpcsaNumber: [''],
    isActive: [true]
  });

  ngOnInit(): void {
    if (this.providerData) {
      this.form.patchValue({
        providerNumber: this.providerData.providerNumber,
        practiceName: this.providerData.practiceName,
        firstName: this.providerData.firstName,
        lastName: this.providerData.lastName,
        contactNumber: this.providerData.contactNumber || '',
        email: this.providerData.email || '',
        address: this.providerData.address || '',
        bHFNumber: this.providerData.bHFNumber || '',
        hpcsaNumber: this.providerData.hpcsaNumber || '',
        isActive: this.providerData.isActive
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const formValue = this.form.value;
    const request: CreateProviderRequest = {
      providerNumber: formValue.providerNumber!,
      practiceName: formValue.practiceName!,
      firstName: formValue.firstName!,
      lastName: formValue.lastName!,
      contactNumber: formValue.contactNumber || null,
      email: formValue.email || null,
      address: formValue.address || null,
      bHFNumber: formValue.bHFNumber || null,
      hpcsaNumber: formValue.hpcsaNumber || null,
      isActive: formValue.isActive ?? true
    };

    const obs = this.isNew
      ? this.providerService.create(request)
      : this.providerService.update(this.providerData!.id, request);

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
