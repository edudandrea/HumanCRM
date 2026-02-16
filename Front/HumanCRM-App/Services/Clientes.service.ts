import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ProspeccaoCliente } from './Prospeccao.service';
import { SafeResourceUrl } from '@angular/platform-browser';

export interface CadastroClientes {
  id: number;
  nome: string;
  tipoPessoa: string;
  cpfCnpj: number;
  cep: number | null;
  rua: string;
  numero: number;
  bairro: string;
  cidade: string;
  estado: string;
  complemento: string;
  telefone: string;
  ddd: number;
  rg: number;
  celular: number;
  email: string;
  redeSocial: string;
  origemContato: string;
  observacoes: string;
  responsavelContato: string;
  IE: number;
  IM: number;
  dataContato: string;
  dataCadastro: string;
  orgaoExpedidor: string;
  sexo: number;
  estadoCivil: number;
  razaoSocial: string;
  necessidade: string;
  dataProximoContato: string;
  dataNascimento: string | null;
  nomeArquivo: string;
  contentType: string;
  tamanho: number;
  criadoEm: string;

  downloadUrl: string;
  previewUrl?: string; // para PNG ou fallback
  safeViewerUrl?: SafeResourceUrl;

  prospeccoes: ProspeccaoCliente[];
}

@Injectable({
  providedIn: 'root',
})
export class ClientesService {
  private apiUrl = `${environment.apiUrl}/Clientes`;

  constructor(private http: HttpClient) {}

  getClients(): Observable<any[]> {
    return this.http.get<CadastroClientes[]>(this.apiUrl);
  }

  pesquisarClientes(filtro: any): Observable<CadastroClientes[]> {
    const params: any = {};

    if (filtro.id) params.id = filtro.id;
    if (filtro.nome) params.nome = filtro.nome;
    if (filtro.cpfCnpj) params.cpfCnpj = filtro.cpfCnpj;
    if (filtro.telefone) params.telefone = filtro.telefone;

    return this.http.get<CadastroClientes[]>(this.apiUrl, { params });
  }

  novoCliente(
    cliente: Partial<CadastroClientes>,
  ): Observable<CadastroClientes> {
    return this.http.post<CadastroClientes>(this.apiUrl, cliente);
  }

  atualizarCliente(id: number, cliente: Partial<CadastroClientes>): Observable<CadastroClientes> {
  return this.http.put<CadastroClientes>(`${this.apiUrl}/${id}`, cliente);
}

  uploadDocumento(clienteId: number, file: File) {
    const form = new FormData();
    form.append('file', file);

    return this.http.post<CadastroClientes>(
      `${this.apiUrl}/${clienteId}/documentos`,
      form,
    );
  }

  listarDocumentos(clienteId: number) {
    return this.http.get<CadastroClientes[]>(
      `${this.apiUrl}/${clienteId}/documentos`,
    );
  }

  excluirDocumento(clienteId: number, docId: number) {
    return this.http.delete<void>(
      `${this.apiUrl}/${clienteId}/documentos/${docId}`,
    );
  }
}
