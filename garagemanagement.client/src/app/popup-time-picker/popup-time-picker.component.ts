import {
  Component,
  Input,
  Output,
  EventEmitter,
  HostListener,
  OnChanges,
  SimpleChanges
} from '@angular/core';

@Component({
  selector: 'app-popup-time-picker',
  standalone: false,
  styleUrls: ['./popup-time-picker.component.css'],
  templateUrl: './popup-time-picker.component.html'
})
export class PopupTimePickerComponent implements OnChanges {

  @Input() selectedDate: string = '';
  @Input() selectedTime: string | null = null;
  @Input() interval: number = 5;

  @Output() timeChange = new EventEmitter<string>();

  showPopup = false;

  selectedDateTime: string = '';

  allSlots: string[] = [];
  filteredSlots: string[] = [];

  constructor() {
    this.allSlots = this.buildAllSlots();
  }

  // 🔥 React properly to date / time changes
  ngOnChanges(changes: SimpleChanges) {

    if (changes['selectedDate']) {
      this.filterSlots(); // only filter slots
    }

    if (changes['selectedTime'] && this.selectedTime) {
      this.selectedDateTime = this.selectedTime; // just display
    }
  }

  togglePopup() {
    this.showPopup = !this.showPopup;
  }

  closePopup() {
    this.showPopup = false;
  }

  buildAllSlots(): string[] {
    const arr: string[] = [];
    for (let h = 0; h < 24; h++) {
      for (let m = 0; m < 60; m += this.interval) {
        arr.push(
          `${h.toString().padStart(2, '0')}:${m
            .toString()
            .padStart(2, '0')}`
        );
      }
    }
    return arr;
  }

  // ✅ ONLY filters time slots — does NOT reset or emit
  filterSlots() {
    const today = new Date().toISOString().slice(0, 10);

    // Future date → allow all slots
    if (this.selectedDate !== today) {
      this.filteredSlots = this.allSlots;
      return;
    }

    // Today → restrict past slots
    const now = new Date();
    const currentTime =
      `${now.getHours().toString().padStart(2, '0')}:` +
      `${now.getMinutes().toString().padStart(2, '0')}`;

    this.filteredSlots = this.allSlots.filter(t =>
      t >= currentTime || t === this.selectedTime
    );
  }


  // ✅ User action only
  selectTime(t: string) {
    this.selectedDateTime = t;
    this.timeChange.emit(t);
    this.closePopup();
  }

  // Close popup when clicking outside
  @HostListener('document:click', ['$event'])
  clickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (
      !target.closest('.time-popup') &&
      !target.closest('.time-input') &&
      !target.closest('.time-input-group')
    ) {
      this.showPopup = false;
    }
  }
}
