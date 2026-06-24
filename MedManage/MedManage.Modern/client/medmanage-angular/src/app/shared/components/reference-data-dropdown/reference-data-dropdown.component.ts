import { Component, Input, OnInit, OnChanges, SimpleChanges, inject, forwardRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor, FormControl } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Observable, startWith, map } from 'rxjs';
import { ReferenceDataService } from '../../../core/services/reference-data.service';
import { ReferenceDataItem, ReferenceDataResource } from '../../../core/models/reference-data.models';

/**
 * Reusable dropdown component that loads reference data from the API.
 * Supports two modes:
 * - 'select': standard Material select dropdown
 * - 'autocomplete': Material autocomplete with type-ahead filtering
 *
 * Implements ControlValueAccessor for use with reactive and template-driven forms.
 *
 * Usage:
 *   <app-reference-data-dropdown
 *     resource="gender"
 *     label="Gender"
 *     [mode]="'select'"
 *     formControlName="genderId">
 *   </app-reference-data-dropdown>
 */
@Component({
  selector: 'app-reference-data-dropdown',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatAutocompleteModule,
    MatInputModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './reference-data-dropdown.component.html',
  styleUrls: ['./reference-data-dropdown.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ReferenceDataDropdownComponent),
      multi: true
    }
  ]
})
export class ReferenceDataDropdownComponent implements OnInit, OnChanges, ControlValueAccessor {
  private readonly referenceDataService = inject(ReferenceDataService);

  /** The reference data resource to load (e.g., 'gender', 'country') */
  @Input({ required: true }) resource!: ReferenceDataResource;

  /** Label displayed on the form field */
  @Input() label = 'Select';

  /** Placeholder text */
  @Input() placeholder = '';

  /** Display mode: 'select' for dropdown or 'autocomplete' for type-ahead */
  @Input() mode: 'select' | 'autocomplete' = 'select';

  /** Whether the field is required */
  @Input() required = false;

  /** Whether to show the loading spinner */
  loading = false;

  /** All items loaded from the API */
  items: ReferenceDataItem[] = [];

  /** Filtered items for autocomplete mode */
  filteredItems$!: Observable<ReferenceDataItem[]>;

  /** Form control for autocomplete input */
  searchControl = new FormControl<string | ReferenceDataItem>('');

  /** Current selected value (the ID) */
  selectedValue: number | null = null;

  // ControlValueAccessor callbacks
  private onChange: (value: number | null) => void = () => {};
  private onTouched: () => void = () => {};
  disabled = false;

  ngOnInit(): void {
    this.loadItems();
    this.setupAutocompleteFilter();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['resource'] && !changes['resource'].firstChange) {
      this.loadItems();
    }
  }

  /** Load reference data items from the service */
  private loadItems(): void {
    if (!this.resource) return;

    this.loading = true;
    this.referenceDataService.getAll(this.resource).subscribe({
      next: (items) => {
        this.items = items;
        this.loading = false;

        // If in autocomplete mode and we have a value, set the display
        if (this.mode === 'autocomplete' && this.selectedValue != null) {
          const selected = this.items.find(i => i.id === this.selectedValue);
          if (selected) {
            this.searchControl.setValue(selected, { emitEvent: false });
          }
        }
      },
      error: () => {
        this.items = [];
        this.loading = false;
      }
    });
  }

  /** Setup the autocomplete filter observable */
  private setupAutocompleteFilter(): void {
    this.filteredItems$ = this.searchControl.valueChanges.pipe(
      startWith(''),
      map(value => {
        const filterText = typeof value === 'string' ? value : value?.name ?? '';
        return this.filterItems(filterText);
      })
    );
  }

  /** Filter items by name (case-insensitive) */
  private filterItems(value: string): ReferenceDataItem[] {
    const filterValue = value.toLowerCase();
    return this.items.filter(item =>
      item.name.toLowerCase().includes(filterValue)
    );
  }

  /** Display function for autocomplete */
  displayFn = (item: ReferenceDataItem): string => {
    return item?.name ?? '';
  };

  /** Handle select change (select mode) */
  onSelectChange(value: number): void {
    this.selectedValue = value;
    this.onChange(value);
    this.onTouched();
  }

  /** Handle autocomplete selection */
  onAutocompleteSelected(item: ReferenceDataItem): void {
    this.selectedValue = item.id;
    this.onChange(item.id);
    this.onTouched();
  }

  /** Handle autocomplete input clear */
  onAutocompleteClear(): void {
    if (typeof this.searchControl.value === 'string' && this.searchControl.value === '') {
      this.selectedValue = null;
      this.onChange(null);
    }
  }

  // ControlValueAccessor implementation

  writeValue(value: number | null): void {
    this.selectedValue = value;
    if (this.mode === 'autocomplete' && this.items.length > 0) {
      const selected = this.items.find(i => i.id === value);
      if (selected) {
        this.searchControl.setValue(selected, { emitEvent: false });
      } else {
        this.searchControl.setValue('', { emitEvent: false });
      }
    }
  }

  registerOnChange(fn: (value: number | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
    if (isDisabled) {
      this.searchControl.disable({ emitEvent: false });
    } else {
      this.searchControl.enable({ emitEvent: false });
    }
  }
}
