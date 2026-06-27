import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, MatTabsModule],
  template: `
    <div class="admin-container">
      <nav mat-tab-nav-bar [tabPanel]="tabPanel">
        <a mat-tab-link routerLink="users" routerLinkActive #rla1="routerLinkActive" [active]="rla1.isActive">
          User Management
        </a>
        <a mat-tab-link routerLink="locks" routerLinkActive #rla2="routerLinkActive" [active]="rla2.isActive">
          Case Locks
        </a>
        <a mat-tab-link routerLink="reference-data" routerLinkActive #rla3="routerLinkActive" [active]="rla3.isActive">
          Reference Data
        </a>
        <a mat-tab-link routerLink="system-config" routerLinkActive #rla4="routerLinkActive" [active]="rla4.isActive">
          System Config
        </a>
        <a mat-tab-link routerLink="imports" routerLinkActive #rla5="routerLinkActive" [active]="rla5.isActive">
          Imports
        </a>
      </nav>
      <mat-tab-nav-panel #tabPanel>
        <router-outlet></router-outlet>
      </mat-tab-nav-panel>
    </div>
  `,
  styles: [`
    .admin-container { padding: 16px; }
  `]
})
export class AdminLayoutComponent {}
