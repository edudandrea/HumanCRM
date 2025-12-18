import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'environments/environment';
import { Observable } from 'rxjs';

export interface ProspeccaoCliente {
  id?: number;
  clienteId: number;
  etapa: string;
  probabilidade: number;
  origemContato: string;
  interessePrincipal: string;
  necessidade: string;
  dataProximoContato?: string;
  canal: string;
  responsavel: string;
  observacoes?: string;
  dataCriacao?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ProspeccaoService {
  private apiUrl = `${environment.apiUrl}/Clientes`;

  constructor(private http: HttpClient) {}

  getProspeccao(clienteId: number): Observable<any[]> {
    return this.http.get<ProspeccaoCliente[]>(
      `${this.apiUrl}/${clienteId}/prospeccoes`
    );
  }  

  addProspeccao(
    clienteId: number,
    prospeccao: Partial<ProspeccaoCliente>
  ): Observable<ProspeccaoCliente> {
    return this.http.post<ProspeccaoCliente>(
      `${this.apiUrl}/${clienteId}/prospeccoes`,
      prospeccao
    );
  }

  atualizarProspeccao(
  clienteId: number,
  prospeccaoId: number,
  prospeccao: Partial<ProspeccaoCliente>
): Observable<ProspeccaoCliente> {
  return this.http.put<ProspeccaoCliente>(
    `${this.apiUrl}/${clienteId}/prospeccoes/${prospeccaoId}`,
    prospeccao
  );
}
}
