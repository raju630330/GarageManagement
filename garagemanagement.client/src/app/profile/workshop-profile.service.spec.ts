import { TestBed } from '@angular/core/testing';

import { WorkshopProfileService } from './workshop-profile.service';

describe('WorkshopProfileService', () => {
  let service: WorkshopProfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkshopProfileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
