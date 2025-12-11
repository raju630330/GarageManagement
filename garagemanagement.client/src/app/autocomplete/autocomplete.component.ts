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

  // Function to fetch data from parent (API)
  @Input() searchFn!: (query: string) => Observable<any[]>;

  // Field to display in dropdown
  @Input() displayKey: string = 'name';

  @Output() selected = new EventEmitter<any>();

  control = new FormControl('');
  results: any[] = [];
  selectedItem: any = null; // store the selected object

  ngOnInit() {
    this.control.valueChanges
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(value => {
          if (!value || value.trim().length < 3) {
            this.results = [];
            return of([]);
          }
          return this.searchFn(value);
        })
      )
      .subscribe({
        next: data => this.results = data,
        error: () => this.results = []
      });
  }

  choose(item: any) {
    this.control.setValue(item[this.displayKey], { emitEvent: false });
    this.results = [];
    this.selectedItem = item; // store selected item
    this.selected.emit(item);
  }

  // Optional: handle manual input
  onBlur() {
    if (!this.selectedItem || this.selectedItem[this.displayKey] !== this.control.value) {
      this.selectedItem = null;
      this.results = [];
    }
  }
}
