import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-CadastroCliente',
  templateUrl: './CadastroCliente.component.html',
  styleUrls: ['./CadastroCliente.component.scss'],
  standalone: false,
})
export class CadastroClienteComponent implements OnInit {
  activeTab: number = 1;
  cliente: any = {};
  subTabActive: number = 1;
  isCNPJ: boolean = false;
  tipoPessoa: string = 'Física';

  filtroProspeccao = {
    nome: '',
    documento: '',
    cidade: '',
  };

  clientesProspeccao: any[] = [];

  prospeccao = {
    clienteId: null as number | null,
    nomeCliente: '',
    etapa: '',
    probabilidade: null as number | null,
    origemContato: '',
    interessePrincipal: '',
    necessidade: '',
    dataProximoContato: '',
    canal: '',
    responsavel: '',
    observacoes: '',
  };

  constructor() {}

  ngOnInit() {}

  setTipoPessoa(tipo: string): void {
    this.tipoPessoa = tipo;
  }

  setActiveTab(tabNumber: number) {
    this.activeTab = tabNumber;
  }

  setActiveSubTab(subTabNumber: number) {
    this.subTabActive = subTabNumber;
  }

  salvarProspeccao(){

  }

  limparProspeccao(){

  }

  selecionarClienteProspeccao(cliente: any){

  }

  pesquisarClientesProspeccao(){

  }
}
