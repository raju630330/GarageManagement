import { TestBed } from '@angular/core/testing';

import { AdditionalJobObserveDetailsService } from './additional-job-observe-details.service';

describe('AdditionalJobObserveDetailsService', () => {
  let service: AdditionalJobObserveDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdditionalJobObserveDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
