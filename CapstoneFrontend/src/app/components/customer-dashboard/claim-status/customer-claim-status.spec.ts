import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerClaimStatus } from './customer-claim-status';

describe('CustomerClaimStatus', () => {
  let component: CustomerClaimStatus;
  let fixture: ComponentFixture<CustomerClaimStatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerClaimStatus],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerClaimStatus);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

