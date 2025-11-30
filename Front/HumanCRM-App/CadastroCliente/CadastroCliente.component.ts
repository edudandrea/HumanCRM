import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-CadastroCliente',
    templateUrl: './CadastroCliente.component.html',
    styleUrls: ['./CadastroCliente.component.scss'],
    standalone: false
})
export class CadastroClienteComponent implements OnInit {

  activeTab: number = 1;
  cliente: any = {};
  subTabActive: number = 1;
  isCNPJ: boolean = false;
  tipoPessoa: string = 'Física';


  constructor() { }

  ngOnInit() {
  }

  setTipoPessoa(tipo: string): void {
  this.tipoPessoa = tipo;
}

  setActiveTab(tabNumber: number) {
    this.activeTab = tabNumber;
  }

  setActiveSubTab(subTabNumber: number) {
    this.subTabActive = subTabNumber;
  }

}
