import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AgentWorkload } from './agent-workload';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('AgentWorkload', () => {
    let component: AgentWorkload;
    let fixture: ComponentFixture<AgentWorkload>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [AgentWorkload],
            providers: [provideHttpClient(), provideAnimations()]
        })
            .compileComponents();

        fixture = TestBed.createComponent(AgentWorkload);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
