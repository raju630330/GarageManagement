import { TestBed } from '@angular/core/testing';

import { JobCardService } from './job-card.service';

describe('JobCardService', () => {
  let service: JobCardService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JobCardService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
