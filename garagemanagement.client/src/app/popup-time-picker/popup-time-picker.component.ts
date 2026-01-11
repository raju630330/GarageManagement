import { Component, Input, Output, EventEmitter, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-popup-time-picker',
  standalone: false,
  styleUrls: ['./popup-time-picker.component.css'],
  templateUrl: './popup-time-picker.component.html'
})
export class PopupTimePickerComponent {

  selectedDateTime: string = '';
  @Input() selectedDate: string = "";
  @Input() selectedTime: string | null = null;
  @Input() interval: number = 5;
  @Output() timeChange = new EventEmitter<string>();

  showPopup = false;

  allSlots: string[] = [];
  filteredSlots: string[] = [];

  constructor() {
    this.allSlots = this.buildAllSlots();
  }

  ngOnChanges() {
    this.filterSlots();
    if (this.selectedTime) {
      this.selectedDateTime = this.selectedTime;
    }
  }

  togglePopup() {
    this.showPopup = !this.showPopup;
  }

  closePopup() {
    this.showPopup = false;
  }

  buildAllSlots() {
    const arr: string[] = [];
    for (let h = 0; h < 24; h++) {
      for (let m = 0; m < 60; m += this.interval) {
        arr.push(`${h.toString().padStart(2, '0')}:${m.toString().padStart(2, '0')}`);
      }
    }
    return arr;
  }

  filterSlots() {

    this.selectedDateTime = "";
    this.timeChange.emit("");

    const today = new Date().toISOString().slice(0, 10);

    if (this.selectedDate === today) {
      const now = new Date();
      const current = `${now.getHours().toString().padStart(2, '0')}:${now.getMinutes().toString().padStart(2, '0')}`;
      this.filteredSlots = this.allSlots.filter(t => t >= current);
    } else {
      this.filteredSlots = this.allSlots;
    }
  }

  selectTime(t: string) {
    this.selectedDateTime = t;  // update UI
    this.timeChange.emit(t);    // send to parent form
    this.closePopup();
  }

  // Close popup if clicked outside
  @HostListener('document:click', ['$event'])
  clickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.time-popup') && !target.closest('.time-input') && !target.closest('.time-input-group')) {
      this.showPopup = false;
    }
  }
}
