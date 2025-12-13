import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CadastroClientes, ClientesService } from 'Services/Clientes.service';

@Component({
  selector: 'app-CadastroCliente',
  templateUrl: './CadastroCliente.component.html',
  styleUrls: ['./CadastroCliente.component.scss'],
  standalone: false,
})
export class CadastroClienteComponent implements OnInit {
  bsModalRef?: BsModalRef;
  activeTab: number = 1;
  cliente: CadastroClientes[] = [];
  clientesFiltrados: CadastroClientes[] = [];
  selectedCliente?: CadastroClientes;
  subTabActive: number = 1;
  isCNPJ: boolean = false;
  id: number = 1;
  tipoPessoa: string = 'Física';
  nome: string = '';
  cpfCnpj: number = 1;
  cep: number = 1;
  rua: string = '';
  numero: number = 1;
  bairro: string = '';
  cidade: string = '';
  estado: string = '';
  complemento: string = '';
  telefone: string = '';
  celular: number = 1;
  email: string = '';
  redeSocial: string = '';
  responsavelContato: string = '';
  origemContato: string = '';
  obs: string = '';
  idPesquisa: number | null = null;
  idCliente: number | null = null;

  modalRef?: BsModalRef;

  filtroClientes = {
    id: '',
    nome: '',
    cpfCnpj: '',
    telefone: '',
  };

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
  

  constructor(
    private toastr: ToastrService,
    private clienteService: ClientesService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
    
  ) {}

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

  pesquisarClientes(filtro: any): void {
    this.spinner.show();
    this.clienteService.pesquisarClientes(filtro).subscribe({
      next: (res) => {
        this.spinner.hide();

        if (!res || res.length === 0) {
          this.toastr.info('Nenhum cliente encontrado.', 'Pesquisa');
          return;
        }

        this.cliente = res;
        // por exemplo seleciona o primeiro resultado e preenche o formulário
        this.selectedCliente = res[0];

        // popular campos do form com o cliente encontrado
        this.popularCampos(this.selectedCliente);

        // muda para aba 2 (onde está o formulário) — isso também ativa a animação
        this.activeTab = 2;
      },
      error: (err) => {
        this.spinner.hide();
        console.error(err);
        this.toastr.error('Erro ao buscar clientes.', 'Erro');
      },
    });
  }
  onPesquisarClick(): void {

  const filtro: any = {};

  if (this.filtroClientes.id)
    filtro.id = Number(this.filtroClientes.id);

  if (this.filtroClientes.nome)
    filtro.nome = this.filtroClientes.nome;

  if (this.filtroClientes.cpfCnpj)
    filtro.cpfCnpj = this.filtroClientes.cpfCnpj;

  if (this.filtroClientes.telefone)
    filtro.telefone = this.filtroClientes.telefone;

  console.log("Filtro enviado corretamente:", filtro);

  this.pesquisarClientes(filtro);
}

  popularCampos(c: CadastroClientes) {
    if (!c) return;
    this.idCliente = c.id;
    this.nome = c.nome ?? '';
    this.cpfCnpj = c.cpfCnpj ?? '';
    this.telefone = c.telefone ?? '';
    this.celular = c.celular;
    this.rua = c.rua ?? '';
    this.numero = c.numero;
    this.bairro = c.bairro ?? '';
    this.email = c.email ?? '';

    // preencha outros campos do formulário conforme seu model...
  }

  salvarProspeccao() {}

  limparProspeccao() {}

  selecionarClienteProspeccao(cliente: any) {}

  pesquisarClientesProspeccao() {}

  openNovoClienteModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {
      class: 'modal-lg modal-dialog-centered', // modal grande, centralizado
    });
  }

  fecharNovoClienteModal() {
    this.modalRef?.hide();
  }

  salvarNovoCliente() {
    this.spinner.show();
    const cliente = {
      nome: this.nome,
      cpfCnpj: this.cpfCnpj,
      telefone: this.telefone,
      email: this.email,
      tipoPessoa: this.tipoPessoa,
    };

    this.clienteService.novoCliente(cliente).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Cliente cadastrado!', 'Sucesso');
        this.fecharNovoClienteModal();
      },
      error: (err) => {
        this.spinner.hide();
        this.toastr.error('Erro ao salvar cliente', 'Erro');
        this.fecharNovoClienteModal();
      },
    });
  }
}
