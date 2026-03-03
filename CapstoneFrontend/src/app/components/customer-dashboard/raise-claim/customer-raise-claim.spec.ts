import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerRaiseClaim } from './customer-raise-claim';

describe('CustomerRaiseClaim', () => {
  let component: CustomerRaiseClaim;
  let fixture: ComponentFixture<CustomerRaiseClaim>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerRaiseClaim],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerRaiseClaim);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

