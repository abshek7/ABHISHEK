import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CustomerCreatePolicy } from './customer-create-policy';

describe('CustomerCreatePolicy', () => {
  let component: CustomerCreatePolicy;
  let fixture: ComponentFixture<CustomerCreatePolicy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerCreatePolicy],
    }).compileComponents();

    fixture = TestBed.createComponent(CustomerCreatePolicy);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

