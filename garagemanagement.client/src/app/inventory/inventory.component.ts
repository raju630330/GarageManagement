import { Component } from '@angular/core';

@Component({
  selector: 'app-inventory',
  standalone: false,
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.css'
})
export class InventoryComponent {
  accessories = [
    { label: 'Service booklet', checked: false },
    { label: 'Jack and Handle', checked: false },
    { label: 'Tool kit', checked: false },
    { label: 'Floor mats', checked: false },
    { label: 'Spare Wheel', checked: false },
    { label: 'Wheel caps', checked: false },
    { label: 'Stereo', checked: false },
    { label: 'Amplifier', checked: false },
    { label: 'DVD Player', checked: false },
    { label: 'LED TV', checked: false },
    { label: 'Speakers Qty', checked: false },
    { label: 'Cigarette Lighter', checked: false },
    { label: 'Mud Flaps', checked: false },
    { label: 'Type condition', checked: false },
    { label: 'Battery Make', checked: false },
    { label: 'Battery No.', checked: false }
  ];

  // Divide into two halves for left & right columns
  accessoriesLeft = this.accessories.slice(0, 8);
  accessoriesRight = this.accessories.slice(8, 16);


}
