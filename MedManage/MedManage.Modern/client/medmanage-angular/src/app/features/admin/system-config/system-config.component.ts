import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatTabsModule } from '@angular/material/tabs';
import { AdminService } from '../../../core/services/admin.service';
import { SystemData, SystemDataRequest } from '../../../core/models/admin.models';

@Component({
  selector: 'app-system-config',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatTabsModule
  ],
  templateUrl: './system-config.component.html',
  styleUrls: ['./system-config.component.scss']
})
export class SystemConfigComponent implements OnInit {
  readonly adminService = inject(AdminService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  form!: FormGroup;
  loading = false;
  saving = false;
  systemData: SystemData | null = null;

  ngOnInit(): void {
    this.initForm();
    this.loadSystemData();
  }

  private initForm(): void {
    this.form = this.fb.group({
      systemEmailAddress: [''],
      smtpServer: [''],
      ssl: [false],
      username: [''],
      password: [''],
      address1: [''],
      address2: [''],
      address3: [''],
      address4: [''],
      addressCode: [''],
      email: [''],
      fax: [''],
      telephone: [''],
      website: [''],
      specialIcu: [null],
      icu: [null],
      highCare: [null],
      neuroWard: [null],
      inIsolation: [null],
      generalWard: [null],
      paediatric: [null],
      maternity: [null],
      dayCase: [null],
      stepDown: [null],
      psychiatric: [null],
      defaultProviderId: [null],
      systemCountryId: [null]
    });
  }

  private loadSystemData(): void {
    this.loading = true;
    this.adminService.getSystemData().subscribe({
      next: (data) => {
        this.systemData = data;
        if (data) {
          this.form.patchValue({
            systemEmailAddress: data.systemEmailAddress,
            smtpServer: data.smtpServer,
            ssl: data.ssl,
            username: data.username,
            address1: data.address1,
            address2: data.address2,
            address3: data.address3,
            address4: data.address4,
            addressCode: data.addressCode,
            email: data.email,
            fax: data.fax,
            telephone: data.telephone,
            website: data.website,
            specialIcu: data.specialIcu,
            icu: data.icu,
            highCare: data.highCare,
            neuroWard: data.neuroWard,
            inIsolation: data.inIsolation,
            generalWard: data.generalWard,
            paediatric: data.paediatric,
            maternity: data.maternity,
            dayCase: data.dayCase,
            stepDown: data.stepDown,
            psychiatric: data.psychiatric,
            defaultProviderId: data.defaultProviderId,
            systemCountryId: data.systemCountryId
          });
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load system configuration', 'Dismiss', { duration: 3000 });
      }
    });
  }

  save(): void {
    if (this.form.invalid) return;

    this.saving = true;
    const request: SystemDataRequest = this.form.value;

    const operation = this.systemData
      ? this.adminService.updateSystemData(this.systemData.systemDataId, request)
      : this.adminService.createSystemData(request);

    operation.subscribe({
      next: (data) => {
        this.systemData = data;
        this.saving = false;
        this.snackBar.open('System configuration saved successfully', 'OK', { duration: 3000 });
      },
      error: () => {
        this.saving = false;
        this.snackBar.open('Failed to save system configuration', 'Dismiss', { duration: 3000 });
      }
    });
  }

  onLogoSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files?.length || !this.systemData) return;

    const file = input.files[0];
    this.adminService.uploadLogo(this.systemData.systemDataId, file).subscribe({
      next: () => {
        this.snackBar.open('Logo uploaded successfully', 'OK', { duration: 3000 });
      },
      error: () => {
        this.snackBar.open('Failed to upload logo', 'Dismiss', { duration: 3000 });
      }
    });
  }
}
