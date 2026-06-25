import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MedicalAidService } from '../services/medical-aid.service';
import {
  MedicalAidDto,
  MedicalAidProductDto,
  MedicalAidExclusionDto,
  MedicalAidTariffDto
} from '../models/medical-aid.models';

@Component({
  selector: 'app-medical-aid-admin',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatTooltipModule
  ],
  templateUrl: './medical-aid-admin.component.html',
  styleUrls: ['./medical-aid-admin.component.scss']
})
export class MedicalAidAdminComponent implements OnInit {
  private readonly medicalAidService = inject(MedicalAidService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  // ─── Medical Aids ────────────────────────────────────────────
  medicalAids: MedicalAidDto[] = [];
  medicalAidColumns = ['name', 'casePrefix', 'initiationDate', 'terminatedDate', 'actions'];
  showMedicalAidForm = false;
  editingMedicalAidId: number | null = null;

  medicalAidForm = this.fb.group({
    medicalAidName: ['', Validators.required],
    casePrefix: [''],
    reportTemplate: [''],
    mainClientId: [null as number | null],
    medicalAidInitiationDate: [''],
    medicalAidReinstatedDate: [''],
    medicalAidTerminatedDate: ['']
  });

  // ─── Products ────────────────────────────────────────────────
  selectedMedicalAidId: number | null = null;
  selectedMedicalAidName: string | null = null;
  products: MedicalAidProductDto[] = [];
  productColumns = ['name', 'allowServices', 'actions'];
  showProductForm = false;
  editingProductId: number | null = null;

  productForm = this.fb.group({
    medAidProductName: ['', Validators.required],
    allowServices: [true]
  });

  // ─── Exclusions ──────────────────────────────────────────────
  exclusions: MedicalAidExclusionDto[] = [];
  exclusionColumns = ['baseTariffId', 'description', 'actions'];
  showExclusionForm = false;

  exclusionForm = this.fb.group({
    baseTariffId: ['', Validators.required]
  });

  // ─── Tariffs ─────────────────────────────────────────────────
  tariffs: MedicalAidTariffDto[] = [];
  tariffColumns = ['tariffNameId', 'tariffName', 'actions'];
  showTariffForm = false;

  tariffForm = this.fb.group({
    tariffNameId: [null as number | null, Validators.required]
  });

  ngOnInit(): void {
    this.loadMedicalAids();
  }

  // ─── Medical Aid Methods ─────────────────────────────────────

  loadMedicalAids(): void {
    this.medicalAidService.getAll().subscribe({
      next: (data) => this.medicalAids = data,
      error: () => this.snackBar.open('Failed to load medical aids', 'Close', { duration: 3000 })
    });
  }

  onAddMedicalAid(): void {
    this.showMedicalAidForm = true;
    this.editingMedicalAidId = null;
    this.medicalAidForm.reset();
  }

  onEditMedicalAid(item: MedicalAidDto): void {
    this.showMedicalAidForm = true;
    this.editingMedicalAidId = item.medicalAidId;
    this.medicalAidForm.patchValue({
      medicalAidName: item.medicalAidName || '',
      casePrefix: item.casePrefix || '',
      reportTemplate: item.reportTemplate || '',
      mainClientId: item.mainClientId || null,
      medicalAidInitiationDate: item.medicalAidInitiationDate || '',
      medicalAidReinstatedDate: item.medicalAidReinstatedDate || '',
      medicalAidTerminatedDate: item.medicalAidTerminatedDate || ''
    });
  }

  onCancelMedicalAid(): void {
    this.showMedicalAidForm = false;
    this.editingMedicalAidId = null;
    this.medicalAidForm.reset();
  }

  onSaveMedicalAid(): void {
    if (this.medicalAidForm.invalid) return;

    const formValue = this.medicalAidForm.value;

    if (this.editingMedicalAidId) {
      const request = {
        medicalAidId: this.editingMedicalAidId,
        mainClientId: formValue.mainClientId || null,
        medicalAidName: formValue.medicalAidName || null,
        casePrefix: formValue.casePrefix || null,
        reportTemplate: formValue.reportTemplate || null,
        medicalAidInitiationDate: formValue.medicalAidInitiationDate || null,
        medicalAidReinstatedDate: formValue.medicalAidReinstatedDate || null,
        medicalAidTerminatedDate: formValue.medicalAidTerminatedDate || null
      };
      this.medicalAidService.update(this.editingMedicalAidId, request).subscribe({
        next: () => {
          this.onCancelMedicalAid();
          this.loadMedicalAids();
          this.snackBar.open('Medical aid updated', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to update medical aid', 'Close', { duration: 3000 })
      });
    } else {
      const request = {
        mainClientId: formValue.mainClientId || null,
        medicalAidName: formValue.medicalAidName || null,
        casePrefix: formValue.casePrefix || null,
        reportTemplate: formValue.reportTemplate || null,
        medicalAidInitiationDate: formValue.medicalAidInitiationDate || null,
        medicalAidReinstatedDate: formValue.medicalAidReinstatedDate || null,
        medicalAidTerminatedDate: formValue.medicalAidTerminatedDate || null
      };
      this.medicalAidService.create(request).subscribe({
        next: () => {
          this.onCancelMedicalAid();
          this.loadMedicalAids();
          this.snackBar.open('Medical aid created', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to create medical aid', 'Close', { duration: 3000 })
      });
    }
  }

  onDeleteMedicalAid(item: MedicalAidDto): void {
    if (confirm(`Are you sure you want to delete "${item.medicalAidName}"?`)) {
      this.medicalAidService.delete(item.medicalAidId).subscribe({
        next: () => {
          this.loadMedicalAids();
          this.snackBar.open('Medical aid deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete medical aid', 'Close', { duration: 3000 })
      });
    }
  }

  onSelectMedicalAid(item: MedicalAidDto): void {
    this.selectedMedicalAidId = item.medicalAidId;
    this.selectedMedicalAidName = item.medicalAidName;
    this.loadProducts();
    this.loadExclusions();
    this.loadTariffs();
  }

  // ─── Product Methods ─────────────────────────────────────────

  loadProducts(): void {
    if (!this.selectedMedicalAidId) return;
    this.medicalAidService.getProducts(this.selectedMedicalAidId).subscribe({
      next: (data) => this.products = data,
      error: () => this.snackBar.open('Failed to load products', 'Close', { duration: 3000 })
    });
  }

  onAddProduct(): void {
    this.showProductForm = true;
    this.editingProductId = null;
    this.productForm.reset({ allowServices: true });
  }

  onEditProduct(item: MedicalAidProductDto): void {
    this.showProductForm = true;
    this.editingProductId = item.medAidProductId;
    this.productForm.patchValue({
      medAidProductName: item.medAidProductName || '',
      allowServices: item.allowServices ?? true
    });
  }

  onCancelProduct(): void {
    this.showProductForm = false;
    this.editingProductId = null;
    this.productForm.reset({ allowServices: true });
  }

  onSaveProduct(): void {
    if (this.productForm.invalid || !this.selectedMedicalAidId) return;

    const formValue = this.productForm.value;

    if (this.editingProductId) {
      const request = {
        medAidProductId: this.editingProductId,
        medAidProductName: formValue.medAidProductName || null,
        allowServices: formValue.allowServices ?? true
      };
      this.medicalAidService.updateProduct(this.selectedMedicalAidId, this.editingProductId, request).subscribe({
        next: () => {
          this.onCancelProduct();
          this.loadProducts();
          this.snackBar.open('Product updated', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to update product', 'Close', { duration: 3000 })
      });
    } else {
      const request = {
        medAidProductName: formValue.medAidProductName || null,
        allowServices: formValue.allowServices ?? true
      };
      this.medicalAidService.createProduct(this.selectedMedicalAidId, request).subscribe({
        next: () => {
          this.onCancelProduct();
          this.loadProducts();
          this.snackBar.open('Product created', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to create product', 'Close', { duration: 3000 })
      });
    }
  }

  onDeleteProduct(item: MedicalAidProductDto): void {
    if (!this.selectedMedicalAidId) return;
    if (confirm(`Are you sure you want to delete product "${item.medAidProductName}"?`)) {
      this.medicalAidService.deleteProduct(this.selectedMedicalAidId, item.medAidProductId).subscribe({
        next: () => {
          this.loadProducts();
          this.snackBar.open('Product deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete product', 'Close', { duration: 3000 })
      });
    }
  }

  // ─── Exclusion Methods ───────────────────────────────────────

  loadExclusions(): void {
    if (!this.selectedMedicalAidId) return;
    this.medicalAidService.getExclusions(this.selectedMedicalAidId).subscribe({
      next: (data) => this.exclusions = data,
      error: () => this.snackBar.open('Failed to load exclusions', 'Close', { duration: 3000 })
    });
  }

  onAddExclusion(): void {
    this.showExclusionForm = true;
    this.exclusionForm.reset();
  }

  onCancelExclusion(): void {
    this.showExclusionForm = false;
    this.exclusionForm.reset();
  }

  onSaveExclusion(): void {
    if (this.exclusionForm.invalid || !this.selectedMedicalAidId) return;

    const request = {
      baseTariffId: this.exclusionForm.value.baseTariffId!
    };
    this.medicalAidService.addExclusion(this.selectedMedicalAidId, request).subscribe({
      next: () => {
        this.onCancelExclusion();
        this.loadExclusions();
        this.snackBar.open('Exclusion added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add exclusion', 'Close', { duration: 3000 })
    });
  }

  onRemoveExclusion(item: MedicalAidExclusionDto): void {
    if (!this.selectedMedicalAidId) return;
    if (confirm(`Remove exclusion for tariff "${item.baseTariffId}"?`)) {
      this.medicalAidService.removeExclusion(this.selectedMedicalAidId, item.baseTariffId).subscribe({
        next: () => {
          this.loadExclusions();
          this.snackBar.open('Exclusion removed', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to remove exclusion', 'Close', { duration: 3000 })
      });
    }
  }

  // ─── Tariff Methods ──────────────────────────────────────────

  loadTariffs(): void {
    if (!this.selectedMedicalAidId) return;
    this.medicalAidService.getTariffs(this.selectedMedicalAidId).subscribe({
      next: (data) => this.tariffs = data,
      error: () => this.snackBar.open('Failed to load tariffs', 'Close', { duration: 3000 })
    });
  }

  onAddTariff(): void {
    this.showTariffForm = true;
    this.tariffForm.reset();
  }

  onCancelTariff(): void {
    this.showTariffForm = false;
    this.tariffForm.reset();
  }

  onSaveTariff(): void {
    if (this.tariffForm.invalid || !this.selectedMedicalAidId) return;

    const request = {
      tariffNameId: this.tariffForm.value.tariffNameId!
    };
    this.medicalAidService.addTariff(this.selectedMedicalAidId, request).subscribe({
      next: () => {
        this.onCancelTariff();
        this.loadTariffs();
        this.snackBar.open('Tariff added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add tariff', 'Close', { duration: 3000 })
    });
  }

  onRemoveTariff(item: MedicalAidTariffDto): void {
    if (!this.selectedMedicalAidId) return;
    if (confirm(`Remove tariff "${item.tariffName || item.tariffNameId}"?`)) {
      this.medicalAidService.removeTariff(this.selectedMedicalAidId, item.tariffNameId).subscribe({
        next: () => {
          this.loadTariffs();
          this.snackBar.open('Tariff removed', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to remove tariff', 'Close', { duration: 3000 })
      });
    }
  }
}
