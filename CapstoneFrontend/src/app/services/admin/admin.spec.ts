import { TestBed } from '@angular/core/testing';
import { Admin } from './admin';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('AdminService', () => {
  let service: Admin;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        Admin,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(Admin);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
