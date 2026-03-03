import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AgentDashboard } from './agent-dashboard';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

describe('AgentDashboard', () => {
  let component: AgentDashboard;
  let fixture: ComponentFixture<AgentDashboard>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AgentDashboard],
      providers: [provideRouter([]), provideHttpClient()]
    })
      .compileComponents();

    fixture = TestBed.createComponent(AgentDashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
