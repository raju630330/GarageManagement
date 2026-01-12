import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  forwardRef
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR
} from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.css'],
  standalone: false,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AutocompleteComponent),
      multi: true
    }
  ]
})
export class AutocompleteComponent
  implements OnInit, ControlValueAccessor {

  @Input() placeholder: string = 'Search...';
  @Input() searchFn!: (query: string) => Observable<any[]>;
  @Input() displayKey: string = 'name';
  @Input() idKey: string = 'id';

  @Output() selected = new EventEmitter<any>();
  @Output() clearForm = new EventEmitter<void>();

  control = new FormControl('');
  results: any[] = [];
  selectedItem: any = null;
  showNoResults: boolean = false;
  disabled = false; 
  // ⭐ CVA callbacks
  private onChange = (_: any) => { };
  private onTouched = () => { };

  ngOnInit() {
    this.control.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(value => {
          const term = (value || '').trim();

          // ⭐ propagate typed value (NO behaviour change)
          this.onChange(term);

          if (term.length < 3) {
            this.results = [];
            this.showNoResults = false;
            return of([]);
          }
          return this.searchFn(term);
        })
      )
      .subscribe({
        next: data => {
          this.results = data || [];
          this.showNoResults =
            this.results.length === 0 &&
            (this.control.value?.trim().length ?? 0) >= 3;
        },
        error: () => {
          this.results = [];
          this.showNoResults = true;
        }
      });
  }

  choose(item: any) {
    this.selectedItem = item;
    this.control.setValue(item[this.displayKey], { emitEvent: false });
    this.results = [];
    this.showNoResults = false;

    // ⭐ update parent form on choose
    this.onChange(item[this.displayKey]);

    // existing behaviour
    this.selected.emit(item);
  }

  onKeyDown(event: KeyboardEvent) {
    if ((event.key === 'Backspace' || event.key === 'Delete') && this.selectedItem) {
      this.selectedItem = null;
      this.results = [];
      this.showNoResults = false;

      // ⭐ clear parent form value
      this.onChange('');

      this.clearForm.emit();
    }
  }

  // ⭐ CVA methods
  writeValue(value: any): void {
    this.control.setValue(value ?? '', { emitEvent: false });
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  highlight(text: string): string {
    const term = (this.control.value || '').trim();
    if (!term) return text;
    const regex = new RegExp(`(${term})`, 'gi');
    return text.replace(regex, `<mark>$1</mark>`);
  }
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;

    if (isDisabled) {
      this.control.disable({ emitEvent: false });
      this.results = [];
      this.showNoResults = false;
    } else {
      this.control.enable({ emitEvent: false });
    }
  }
}
