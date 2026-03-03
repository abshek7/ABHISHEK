import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminAnalytics } from './admin-analytics';
import { provideHttpClient } from '@angular/common/http';

describe('AdminAnalytics', () => {
    let component: AdminAnalytics;
    let fixture: ComponentFixture<AdminAnalytics>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [AdminAnalytics],
            providers: [provideHttpClient()]
        })
            .compileComponents();

        fixture = TestBed.createComponent(AdminAnalytics);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
