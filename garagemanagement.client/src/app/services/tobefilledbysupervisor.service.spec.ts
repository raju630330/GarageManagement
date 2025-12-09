import { TestBed } from '@angular/core/testing';

import { TobefilledbysupervisorService } from './tobefilledbysupervisor.service';

describe('TobefilledbysupervisorService', () => {
  let service: TobefilledbysupervisorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TobefilledbysupervisorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
