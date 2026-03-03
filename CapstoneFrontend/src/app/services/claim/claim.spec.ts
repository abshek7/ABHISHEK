import { TestBed } from '@angular/core/testing';
import { Claim } from './claim';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('ClaimService', () => {
  let service: Claim;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        Claim,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(Claim);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
