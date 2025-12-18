import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';
import { ProspeccaoCliente } from './Prospeccao.service';

export interface CadastroClientes {
  id: number;
  nome: string;
  tipoPessoa: string;
  cpfCnpj: number;
  cep: number;
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
  obs: string;
  responsavelContato: string;
  IE: number,
  IM: number,
  dataContato: string,
  dataCadastro: string,
  orgaoExpedidor: string;
  sexo: number;
  estadoCivil: number;
  razaoSocial: string;

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
    cliente: Partial<CadastroClientes>
  ): Observable<CadastroClientes> {
    return this.http.post<CadastroClientes>(this.apiUrl, cliente);
  }

  salvarCliente(
    cliente: Partial<CadastroClientes>
  ): Observable<CadastroClientes> {
    return this.http.put<CadastroClientes>(this.apiUrl, cliente);
  }
}
