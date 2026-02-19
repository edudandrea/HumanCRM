/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ClienteDocumentoService } from './ClienteDocumento.service';

describe('Service: ClienteDocumento', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ClienteDocumentoService]
    });
  });

  it('should ...', inject([ClienteDocumentoService], (service: ClienteDocumentoService) => {
    expect(service).toBeTruthy();
  }));
});
