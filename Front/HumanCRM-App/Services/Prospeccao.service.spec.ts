/* tslint:disable:no-unused-variable */

import { TestBed, waitForAsync, inject } from '@angular/core/testing';
import { ProspeccaoService } from './Prospeccao.service';

describe('Service: Prospeccao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProspeccaoService]
    });
  });

  it('should ...', inject([ProspeccaoService], (service: ProspeccaoService) => {
    expect(service).toBeTruthy();
  }));
});
