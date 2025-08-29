import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { WorkshopProfileService } from './workshop-profile.service';

describe('WorkshopProfileService', () => {
  let service: WorkshopProfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [WorkshopProfileService]
    });
    service = TestBed.inject(WorkshopProfileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
