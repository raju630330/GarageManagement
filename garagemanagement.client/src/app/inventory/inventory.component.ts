import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RepairOrderService } from '../services/repair-order.service';

@Component({
  selector: 'app-inventory',
  standalone: false,
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent {

  constructor(private http: HttpClient, private repairOrderService: RepairOrderService) { }

  formSubmitted = false;
  showSuccess = false;

  repairOrderId: number | null = null;

  ngOnInit(): void {
    // 🔥 Get current repair order id
    this.repairOrderService.repairOrderId$.subscribe(id => {
      this.repairOrderId = id;
    });
  }

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

  accessoriesLeft = this.accessories.slice(0, 8);
  accessoriesRight = this.accessories.slice(8, 16);

  saveInventory() {
    this.formSubmitted = true;

    if (!this.repairOrderId) {
      alert('⚠️ Please save Repair Order first');
      return;
    }

    const allSelected = this.accessories.every(x => x.checked);
    if (!allSelected) {
      return;
    }

    // ✅ Append repairOrderId here
    const payload = {
      repairOrderId: this.repairOrderId,
      accessories: this.accessories
    };

    this.http.post("https://localhost:7086/api/Inventory/save", payload)
      .subscribe({
        next: () => {
          this.showSuccess = true;
          setTimeout(() => this.showSuccess = false, 2000);
        },
        error: () => {
          alert("❌ Error saving inventory");
        }
      });
  }
}
