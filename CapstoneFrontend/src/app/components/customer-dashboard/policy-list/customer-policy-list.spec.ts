import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerPolicyList } from './customer-policy-list';

describe('CustomerPolicyList', () => {
  let component: CustomerPolicyList;
  let fixture: ComponentFixture<CustomerPolicyList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerPolicyList],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerPolicyList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

