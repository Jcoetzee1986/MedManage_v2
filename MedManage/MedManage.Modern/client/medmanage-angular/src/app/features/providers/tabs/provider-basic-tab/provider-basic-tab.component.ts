import { Component, Input, Output, EventEmitter, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
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
    MatCheckboxModule,
    MatIconModule
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
    // General
    providerNumber: ['', Validators.required],
    practiceName: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    specialityName: [''],
    practiceGroupNumber: [''],
    numberOfPartners: [null as number | null],
    serviceArea: [''],
    isHospital: [false],
    // Contact
    contactNumber: [''],
    cellNumber: [''],
    fax: [''],
    email: ['', Validators.email],
    countryName: [''],
    languageName: [''],
    // Physical Address
    address1: [''],
    address2: [''],
    address3: [''],
    address4: [''],
    addressCode: [''],
    // Postal Address
    postalAddress1: [''],
    postalAddress2: [''],
    postalAddress3: [''],
    postalAddress4: [''],
    postalAddressCode: [''],
    // Banking
    bankName: [''],
    branchName: [''],
    branchCode: [''],
    accountType: [''],
    accountNumber: [''],
    // Meta
    isActive: [true]
  });

  ngOnInit(): void {
    if (this.providerData) {
      this.form.patchValue({
        providerNumber: this.providerData.providerNumber,
        practiceName: this.providerData.practiceName,
        firstName: this.providerData.firstName,
        lastName: this.providerData.lastName,
        specialityName: this.providerData.specialityName || '',
        practiceGroupNumber: this.providerData.practiceGroupNumber || '',
        numberOfPartners: this.providerData.numberOfPartners,
        serviceArea: this.providerData.serviceArea || '',
        isHospital: this.providerData.isHospital || false,
        contactNumber: this.providerData.contactNumber || '',
        cellNumber: this.providerData.cellNumber || '',
        fax: this.providerData.fax || '',
        email: this.providerData.email || '',
        countryName: this.providerData.countryName || '',
        languageName: this.providerData.languageName || '',
        address1: this.providerData.address1 || '',
        address2: this.providerData.address2 || '',
        address3: this.providerData.address3 || '',
        address4: this.providerData.address4 || '',
        addressCode: this.providerData.addressCode || '',
        postalAddress1: this.providerData.postalAddress1 || '',
        postalAddress2: this.providerData.postalAddress2 || '',
        postalAddress3: this.providerData.postalAddress3 || '',
        postalAddress4: this.providerData.postalAddress4 || '',
        postalAddressCode: this.providerData.postalAddressCode || '',
        bankName: this.providerData.bankName || '',
        branchName: this.providerData.branchName || '',
        branchCode: this.providerData.branchCode || '',
        accountType: this.providerData.accountType || '',
        accountNumber: this.providerData.accountNumber || '',
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
      practiceGroupNumber: formValue.practiceGroupNumber || null,
      numberOfPartners: formValue.numberOfPartners || null,
      serviceArea: formValue.serviceArea || null,
      isHospital: formValue.isHospital || false,
      contactNumber: formValue.contactNumber || null,
      cellNumber: formValue.cellNumber || null,
      fax: formValue.fax || null,
      email: formValue.email || null,
      address1: formValue.address1 || null,
      address2: formValue.address2 || null,
      address3: formValue.address3 || null,
      address4: formValue.address4 || null,
      addressCode: formValue.addressCode || null,
      postalAddress1: formValue.postalAddress1 || null,
      postalAddress2: formValue.postalAddress2 || null,
      postalAddress3: formValue.postalAddress3 || null,
      postalAddress4: formValue.postalAddress4 || null,
      postalAddressCode: formValue.postalAddressCode || null,
      bankName: formValue.bankName || null,
      branchName: formValue.branchName || null,
      branchCode: formValue.branchCode || null,
      accountType: formValue.accountType || null,
      accountNumber: formValue.accountNumber || null,
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
