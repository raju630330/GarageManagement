import { Component } from '@angular/core';

@Component({
  selector: 'app-new-jobcard',
  templateUrl: './new-jobcard.component.html',
  styleUrls: ['./new-jobcard.component.css'],
  standalone: false
})
export class NewJobCardComponent {
  // Track section collapse state
  isSectionOpen: { [key: number]: boolean } = {
    1: false, 2: false, 3: false, 4: false, 5: false
  };

  newConcern: string = '';
  concerns: { text: string, active: boolean }[] = [];

  toggleSection(section: number) {
    this.isSectionOpen[section] = !this.isSectionOpen[section];
  }

  addConcern() {
    if (this.newConcern.trim() !== '') {
      this.concerns.push({ text: this.newConcern, active: true });
      this.newConcern = '';
    }
  }

  toggleConcern(index: number) {
    this.concerns[index].active = !this.concerns[index].active;
  }

  openPopup(type: string) {
    alert(`Open details for: ${type}`);
  }
}
