import { Component } from '@angular/core';

/**
 * Placeholder wizard component - actual wizard implementation is directly 
 * in the CaseWizardComponent using mat-stepper.
 */
@Component({
  selector: 'app-wizard',
  standalone: true,
  template: '<ng-content></ng-content>'
})
export class WizardComponent {}
