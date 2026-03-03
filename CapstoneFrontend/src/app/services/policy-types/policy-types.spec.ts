import { TestBed } from '@angular/core/testing';

import { PolicyTypes } from './policy-types';

describe('PolicyTypes', () => {
  let service: PolicyTypes;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PolicyTypes);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
