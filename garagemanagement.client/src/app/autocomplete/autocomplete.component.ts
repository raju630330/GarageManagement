import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl } from '@angular/forms';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-autocomplete',
  templateUrl: './autocomplete.component.html',
  styleUrls: ['./autocomplete.component.css'],
  standalone:false
})
export class AutocompleteComponent implements OnInit {

  @Input() placeholder: string = 'Search...';
  @Input() searchFn!: (query: string) => Observable<any[]>;
  @Input() displayKey: string = 'name';
  @Input() idKey: string = 'id';

  @Output() selected = new EventEmitter<any>();

  control = new FormControl('');
  results: any[] = [];
  selectedItem: any = null;
  showNoResults: boolean = false;

  ngOnInit() {
    this.control.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(value => {
          const term = (value || '').trim();
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
          this.showNoResults = this.results.length === 0 && (this.control.value?.trim().length ?? 0) >= 3;
        },
        error: () => {
          this.results = [];
          this.showNoResults = true;
        }
      });
  }

  choose(item: any) {
    this.selectedItem = item;
    console.log('choose called', item);
    this.control.setValue(item[this.displayKey], { emitEvent: false });
    this.results = [];
    this.showNoResults = false;

    // Emit selected item once, only on choose
    this.selected.emit(item);
  }

  onKeyDown(event: KeyboardEvent) {
    if ((event.key === 'Backspace' || event.key === 'Delete') && this.selectedItem) {
      this.selected.emit({ [this.idKey]: null });
      this.selectedItem = null;
      this.results = [];
      this.showNoResults = false;
    }
  }

  highlight(text: string): string {
    const term = (this.control.value || '').trim();
    if (!term) return text;
    const regex = new RegExp(`(${term})`, 'gi');
    return text.replace(regex, `<mark>$1</mark>`);
  }
}
