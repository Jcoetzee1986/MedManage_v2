import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MemberService } from '../../services/member.service';
import { MemberMedicalAidProductDto, CreateMemberMedicalAidProductRequest } from '../../models/member.models';
import { MedicalAidService } from '../../../medical-aids/services/medical-aid.service';
import { MedicalAidProductDto } from '../../../medical-aids/models/medical-aid.models';

@Component({
  selector: 'app-member-products-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './member-products-tab.component.html',
  styleUrls: ['./member-products-tab.component.scss']
})
export class MemberProductsTabComponent implements OnInit {
  @Input() memberId!: number;
  @Input() medicalAidId: number | null = null;

  private readonly memberService = inject(MemberService);
  private readonly medicalAidService = inject(MedicalAidService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: MemberMedicalAidProductDto[] = [];
  availableProducts: MedicalAidProductDto[] = [];
  displayedColumns = ['medicalAidProductName', 'dateFrom', 'dateTo', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    medicalAidProductId: [null as number | null, Validators.required],
    dateFrom: [null as Date | null, Validators.required],
    dateTo: [null as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
    this.loadAvailableProducts();
  }

  loadItems(): void {
    this.memberService.getMedicalAidProducts(this.memberId).subscribe({
      next: (data) => this.items = data,
      error: () => this.snackBar.open('Failed to load medical aid products', 'Close', { duration: 3000 })
    });
  }

  private loadAvailableProducts(): void {
    if (this.medicalAidId) {
      this.medicalAidService.getProducts(this.medicalAidId).subscribe({
        next: (products) => this.availableProducts = products,
        error: () => {}
      });
    }
  }

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.form.reset();
  }

  onEdit(item: MemberMedicalAidProductDto): void {
    this.showForm = true;
    this.editingId = item.id;
    this.form.patchValue({
      medicalAidProductId: item.medicalAidProductId,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null
    });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const formValue = this.form.value;
    const request: CreateMemberMedicalAidProductRequest = {
      medicalAidProductId: formValue.medicalAidProductId!,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : null,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : null
    };

    const obs = this.editingId
      ? this.memberService.updateMedicalAidProduct(this.memberId, this.editingId, request)
      : this.memberService.createMedicalAidProduct(this.memberId, request);

    obs.subscribe({
      next: () => {
        this.showForm = false;
        this.editingId = null;
        this.form.reset();
        this.loadItems();
        this.snackBar.open('Product saved', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save product', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: MemberMedicalAidProductDto): void {
    if (confirm('Are you sure you want to remove this product history entry?')) {
      this.memberService.deleteMedicalAidProduct(this.memberId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Product removed', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete product', 'Close', { duration: 3000 })
      });
    }
  }
}
