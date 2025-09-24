import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms'; 
import { TotalRepairCostComponent } from './total-repair-cost.component';

describe('TotalRepairCostComponent', () => {
  let component: TotalRepairCostComponent;
  let fixture: ComponentFixture<TotalRepairCostComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TotalRepairCostComponent],
       imports: [FormsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TotalRepairCostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should calculate grand total correctly', () => {
    component.repairItems = [
      { sno: '1', description: 'Test 1', amount: 100 },
      { sno: '2', description: 'Test 2', amount: 200 }
    ];
    component.calculateTotal();
    expect(component.grandTotal).toBe(300);
  });

});
