import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ClaimsQueue } from './claims-queue';
import { provideHttpClient } from '@angular/common/http';

describe('ClaimsQueue', () => {
    let component: ClaimsQueue;
    let fixture: ComponentFixture<ClaimsQueue>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ClaimsQueue],
            providers: [provideHttpClient()]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ClaimsQueue);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
