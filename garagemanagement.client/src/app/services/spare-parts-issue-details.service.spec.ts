import { TestBed } from '@angular/core/testing';

import { SparePartsIssueDetailsService } from './spare-parts-issue-details.service';

describe('SparePartsIssueDetailsService', () => {
  let service: SparePartsIssueDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SparePartsIssueDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
