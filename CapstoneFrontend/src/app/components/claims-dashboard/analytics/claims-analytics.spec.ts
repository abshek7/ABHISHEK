import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ClaimsAnalytics } from './claims-analytics';
import { provideHttpClient } from '@angular/common/http';

describe('ClaimsAnalytics', () => {
    let component: ClaimsAnalytics;
    let fixture: ComponentFixture<ClaimsAnalytics>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ClaimsAnalytics],
            providers: [provideHttpClient()]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ClaimsAnalytics);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
