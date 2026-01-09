import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';

interface StockItem {
  partNo: string;
  partName: string;
  brand: string;
  category: string;
  qtyOnHand: number;
  avgPurchasePrice: number;
  avgSellingPrice: number;
  taxType: string;
  taxPercent: number;
  taxAmount: number;
  rackNo: string;
  ageing: string;
  barcode: string;
}

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css'],
  standalone: false
})
export class StockComponent implements OnInit {

  stockStats = {
    uniquePartNos: 1202,
    totalStockItems: 31640.51,
    stockValue: 3165219.06
  };

  tabs = ['stock', 'order', 'inward', 'issued', 'purchaseReturn', 'stockAlert'];
  activeTab = 'stock';

  searchForm!: FormGroup;

  stockItems: StockItem[] = []; // API or dummy data
  filteredItems: StockItem[] = [];

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.searchForm = this.fb.group({
      status: ['In Stock'],
      search: ['']
    });

    // Dummy data for demonstration
    this.stockItems = [
      {
        partNo: 'FOG/LAMP/Y',
        partName: 'Yellow Fog Lamp',
        brand: 'ABC',
        category: 'Lighting',
        qtyOnHand: 3,
        avgPurchasePrice: 250,
        avgSellingPrice: 400,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 72,
        rackNo: 'R1',
        ageing: '10 days',
        barcode: '123456789012'
      },
      {
        partNo: '3MM WIRE',
        partName: '3mm Copper Wire',
        brand: 'XYZ',
        category: 'Electrical',
        qtyOnHand: 15,
        avgPurchasePrice: 50,
        avgSellingPrice: 80,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 5,
        rackNo: 'R2',
        ageing: '5 days',
        barcode: '987654321098'
      },
      {
        partNo: '3MM WIRE',
        partName: '3mm Copper Wire',
        brand: 'XYZ',
        category: 'Electrical',
        qtyOnHand: 15,
        avgPurchasePrice: 50,
        avgSellingPrice: 80,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 5,
        rackNo: 'R2',
        ageing: '5 days',
        barcode: '987654321098'
      },
      {
        partNo: '3MM WIRE',
        partName: '3mm Copper Wire',
        brand: 'XYZ',
        category: 'Electrical',
        qtyOnHand: 15,
        avgPurchasePrice: 50,
        avgSellingPrice: 80,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 5,
        rackNo: 'R2',
        ageing: '5 days',
        barcode: '987654321098'
      },
      {
        partNo: '3MM WIRE',
        partName: '3mm Copper Wire',
        brand: 'XYZ',
        category: 'Electrical',
        qtyOnHand: 15,
        avgPurchasePrice: 50,
        avgSellingPrice: 80,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 5,
        rackNo: 'R2',
        ageing: '5 days',
        barcode: '987654321098'
      },
      {
        partNo: '3MM WIRE',
        partName: '3mm Copper Wire',
        brand: 'XYZ',
        category: 'Electrical',
        qtyOnHand: 15,
        avgPurchasePrice: 50,
        avgSellingPrice: 80,
        taxType: 'GST',
        taxPercent: 18,
        taxAmount: 5,
        rackNo: 'R2',
        ageing: '5 days',
        barcode: '987654321098'
      }

    ];

    this.filteredItems = [...this.stockItems];

    // React to search form changes
    this.searchForm.valueChanges.subscribe(() => this.applyFilters());
  }

  // Getters for proper typing
  get statusControl(): FormControl {
    return this.searchForm.get('status') as FormControl;
  }

  get searchControl(): FormControl {
    return this.searchForm.get('search') as FormControl;
  }

  applyFilters(): void {
    const { status, search } = this.searchForm.value;
    this.filteredItems = this.stockItems.filter(item => {
      const matchesStatus = status === 'In Stock' ? item.qtyOnHand > 0 : true;
      const matchesSearch = !search ||
        item.partName.toLowerCase().includes(search.toLowerCase()) ||
        item.partNo.toLowerCase().includes(search.toLowerCase()) ||
        item.brand.toLowerCase().includes(search.toLowerCase());
      return matchesStatus && matchesSearch;
    });
  }

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }

  exportStock() {
    alert('Export functionality not implemented yet!');
  }

}
