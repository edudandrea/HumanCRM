/* tslint:disable:no-unused-variable */

import { TestBed, waitForAsync, inject } from '@angular/core/testing';
import { ClientesService } from './Clientes.service';

describe('Service: ClientesService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ClientesService]
    });
  });

  it('should ...', inject([ClientesService], (service: ClientesService) => {
    expect(service).toBeTruthy();
  }));
});
