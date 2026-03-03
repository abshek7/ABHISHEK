import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AdminOversight } from './admin-oversight';
import { provideHttpClient } from '@angular/common/http';

describe('AdminOversight', () => {
    let component: AdminOversight;
    let fixture: ComponentFixture<AdminOversight>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [AdminOversight],
            providers: [provideHttpClient()]
        })
            .compileComponents();

        fixture = TestBed.createComponent(AdminOversight);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
