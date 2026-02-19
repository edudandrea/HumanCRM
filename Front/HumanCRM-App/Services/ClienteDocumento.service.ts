import { Injectable } from '@angular/core';


export interface ClienteDocumento {
  id: number;
  clienteId: number;
  nomeArquivo: string;
  contentType: string; // 'application/pdf' | 'image/png' etc
  previewUrl?: string;      // blob/objectURL ou URL da API
  safeViewerUrl?: any;      // SafeResourceUrl
}

@Injectable({
  providedIn: 'root'
})
export class ClienteDocumentoService {

constructor() { }

}
