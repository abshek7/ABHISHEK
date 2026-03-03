import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ClaimsDashboard } from './claims-dashboard';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

describe('ClaimsDashboard', () => {
  let component: ClaimsDashboard;
  let fixture: ComponentFixture<ClaimsDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClaimsDashboard],
      providers: [provideRouter([]), provideHttpClient()]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ClaimsDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
