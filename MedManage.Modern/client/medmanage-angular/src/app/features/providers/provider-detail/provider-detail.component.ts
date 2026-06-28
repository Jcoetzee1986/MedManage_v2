import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NavigationContextService } from '../../../core/services/navigation-context.service';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject, takeUntil } from 'rxjs';
import { ProviderService } from '../services/provider.service';
import { ProviderDto } from '../models/provider.models';
import { ProviderBasicTabComponent } from '../tabs/provider-basic-tab/provider-basic-tab.component';
import { ProviderTariffsTabComponent } from '../tabs/provider-tariffs-tab/provider-tariffs-tab.component';
import { ProviderCustomTariffsTabComponent } from '../tabs/provider-custom-tariffs-tab/provider-custom-tariffs-tab.component';
import { ProviderDiscountsTabComponent } from '../tabs/provider-discounts-tab/provider-discounts-tab.component';

@Component({
  selector: 'app-provider-detail',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatSnackBarModule,
    ProviderBasicTabComponent,
    ProviderTariffsTabComponent,
    ProviderCustomTariffsTabComponent,
    ProviderDiscountsTabComponent
  ],
  templateUrl: './provider-detail.component.html',
  styleUrls: ['./provider-detail.component.scss']
})
export class ProviderDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly navContext = inject(NavigationContextService);
  private readonly providerService = inject(ProviderService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  providerData: ProviderDto | null = null;
  providerId!: number;
  loading = true;
  isNew = false;

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam === 'new') {
      this.isNew = true;
      this.loading = false;
    } else if (idParam) {
      this.providerId = +idParam;
      this.loadProvider();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private loadProvider(): void {
    this.loading = true;
    this.providerService.getById(this.providerId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.providerData = data;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load provider', 'Close', { duration: 3000 });
        }
      });
  }

  onBackToList(): void {
    const returnUrl = this.navContext.getReturnUrl();
    this.router.navigateByUrl(returnUrl || '/providers');
  }

  onDeleteProvider(): void {
    if (confirm('Are you sure you want to delete this provider?')) {
      this.providerService.delete(this.providerId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Provider deleted', 'Close', { duration: 3000 });
            this.router.navigate(['/providers']);
          },
          error: () => {
            this.snackBar.open('Failed to delete provider', 'Close', { duration: 3000 });
          }
        });
    }
  }

  onProviderSaved(provider: ProviderDto): void {
    this.providerData = provider;
    if (this.isNew) {
      this.isNew = false;
      this.providerId = provider.id;
      this.router.navigate(['/providers', provider.id], { replaceUrl: true });
    }
    this.snackBar.open('Provider saved successfully', 'Close', { duration: 3000 });
  }
}
