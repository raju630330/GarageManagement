import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RepairOrderService } from '../services/repair-order.service';
import { InventoryService } from '../services/inventory.service';
import { Subscription } from 'rxjs';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-inventory',
  standalone: false,
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})
export class InventoryComponent implements OnInit, OnDestroy {

  constructor(
    private fb: FormBuilder,
    private repairOrderService: RepairOrderService,
    private inventoryService: InventoryService,
    private alert: AlertService
  ) { }

  private sub!: Subscription;

  inventoryForm!: FormGroup;
  showSuccess = false;

  repairOrderId: number | null = null;
  inventoryId = 0;

  accessoryMaster = [
    'Service booklet',
    'Jack and Handle',
    'Tool kit',
    'Floor mats',
    'Spare Wheel',
    'Wheel caps',
    'Stereo',
    'Amplifier',
    'DVD Player',
    'LED TV',
    'Speakers Qty',
    'Cigarette Lighter',
    'Mud Flaps',
    'Type condition',
    'Battery Make',
    'Battery No.'
  ];

  ngOnInit(): void {
    this.createForm();

    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;

      this.repairOrderId = id;
      this.loadInventory(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  // 🔹 Create Reactive Form
  createForm() {
    this.inventoryForm = this.fb.group({
      accessories: this.fb.array([])
    });

    this.accessoryMaster.forEach(label => {
      this.accessories.push(
        this.fb.group({
          label: [label],
          checked: [false]
        })
      );
    });
  }

  // 🔹 FormArray getter
  get accessories(): FormArray {
    return this.inventoryForm.get('accessories') as FormArray;
  }

  // 🔹 Left / Right columns
  get accessoriesLeft(): FormGroup[] {
    return this.accessories.controls.slice(0, 8) as FormGroup[];
  }

  get accessoriesRight(): FormGroup[] {
    return this.accessories.controls.slice(8, 16) as FormGroup[];
  }

  // 🔹 Load inventory
  loadInventory(repairOrderId: number) {
    this.inventoryService.getInventoryByRepairOrderId(repairOrderId)
      .subscribe({
        next: res => {
          if (!res?.accessories) return;
          this.inventoryId = res.id;
          this.accessories.controls.forEach(ctrl => {
            const saved = res.accessories.find(
              x => x.label === ctrl.value.label
            );
            if (saved) {             
              ctrl.patchValue({ checked: saved.checked });
            }
          });
        },
        error: () => {
          console.info('Inventory not created yet');
        }
      });
  }

  // 🔹 Save inventory
  saveInventory() {
    if (!this.repairOrderId) {
      alert('⚠️ Please save Repair Order first');
      return;
    }

    const hasChecked = this.accessories.controls.some(
      ctrl => ctrl.get('checked')?.value === true
    );

    if (!hasChecked) {
      alert('⚠️ Please select at least one accessory');
      return;
    }

    const payload = {
      id: this.inventoryId,
      repairOrderId: this.repairOrderId,
      accessories: this.inventoryForm.value.accessories
    };

    this.inventoryService.saveInventory(payload).subscribe({
      next: (res : any) => {
        this.showSuccess = true;
        this.inventoryId = res.id;
        this.alert.showSuccess("Saved Suceesfully");
      },
      error: () => alert('❌ Error saving inventory')
    });
  }

}
