import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CadastroClientes, ClientesService } from 'Services/Clientes.service';
import {
  ProspeccaoCliente,
  ProspeccaoService,
} from 'Services/Prospeccao.service';

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
  ddd: number = 1;
  rg: number = 1;
  razaoSocial: string = '';
  IE: number = 1;
  IM: number = 1;
  dataFuncacao: string = '';
  dataContato: string = '';
  orgaoExpedidor: string = '';
  sexo: number = 1;
  estadoCivil: number = 1;

  modalRef?: BsModalRef;

  filtroClientes = {
    id: '',
    nome: '',
    cpfCnpj: '',
    telefone: '',
  };

  filtroProspeccao = {
    id: '',
    nome: '',
    documento: '',
    cidade: '',
  };

  clientesProspeccao: any[] = [];
  prospeccoes: ProspeccaoCliente[] = [];
  mostrarHistorico = false;

  prospeccao: Partial<ProspeccaoCliente> = {
    id: 1,
    etapa: '',
    probabilidade: 1,
    origemContato: '',
    interessePrincipal: '',
    necessidade: '',
    dataProximoContato: '',
    canal: '',
    responsavel: '',
    observacoes: '',
  };

  podeSalvar: boolean = false;

  constructor(
    private toastr: ToastrService,
    private clienteService: ClientesService,
    private prospeccaoService: ProspeccaoService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {}

  ngOnInit() {}

  setTipoPessoa(tipo: string): void {
    this.tipoPessoa = tipo;
  }

  setActiveTab(tabNumber: number) {
    this.activeTab = tabNumber;
    if (tabNumber === 3 && this.selectedCliente) {
      this.definirClienteAtual(this.selectedCliente);
    }
  }

  setActiveSubTab(subTabNumber: number) {
    this.subTabActive = subTabNumber;
  }
  //--------- CLIENTES -----------//
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
        this.definirClienteAtual(res[0]);
        this.prospeccoes = res[0].prospeccoes ?? [];
        this.prospeccao = this.prospeccoes.length
          ? { ...this.prospeccoes[0] }
          : this.novaProspeccao();

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

    if (this.filtroClientes.id) filtro.id = Number(this.filtroClientes.id);

    if (this.filtroClientes.nome) filtro.nome = this.filtroClientes.nome;

    if (this.filtroClientes.cpfCnpj)
      filtro.cpfCnpj = this.filtroClientes.cpfCnpj;

    if (this.filtroClientes.telefone)
      filtro.telefone = this.filtroClientes.telefone;

    console.log('Filtro enviado corretamente:', filtro);

    this.pesquisarClientes(filtro);
  }

  popularCampos(c: CadastroClientes) {
    if (!c) return;
    this.idCliente = c.id;
    this.nome = c.nome ?? '';
    this.cpfCnpj = c.cpfCnpj ?? '';
    this.rg = c.rg;
    this.ddd = c.ddd;
    this.telefone = c.telefone ?? '';
    this.celular = c.celular;
    this.rua = c.rua ?? '';
    this.cidade = c.cidade ?? '';
    this.estado = c.estado ?? '';
    this.numero = c.numero;
    this.bairro = c.bairro ?? '';
    this.email = c.email ?? '';
    this.redeSocial = c.redeSocial ?? '';
    this.complemento = c.complemento ?? '';
    this.responsavelContato = c.responsavelContato ?? '';
    this.origemContato = c.origemContato ?? '';
    this.obs = c.obs ?? '';
    this.IE = c.IE;
    this.IM = c.IM;
    this.dataContato = c.dataContato ?? '';
    this.razaoSocial = c.razaoSocial ?? '';
    this.orgaoExpedidor = c.orgaoExpedidor ?? '';
    this.sexo = c.sexo;
    this.estadoCivil = c.estadoCivil;

    // preencha outros campos do formulário conforme seu model...
  }

  openNovoClienteModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {
      class: 'modal-lg modal-dialog-centered', // modal grande, centralizado
    });
  }

  openNovoProspModal(templateProsp: TemplateRef<any>) {
    this.modalRef = this.modalService.show(templateProsp, {
      class: 'modal-lg modal-dialog-centered', // modal grande, centralizado
    });
  }

  fecharModal() {
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
      ddd: this.ddd,
    };

    this.clienteService.novoCliente(cliente).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Cliente cadastrado!', 'Sucesso');
        this.fecharModal();
      },
      error: (err) => {
        this.spinner.hide();
        this.toastr.error('Erro ao salvar cliente', 'Erro');
        this.fecharModal();
      },
    });
  }

  salvarCliente() {
    if (!this.podeSalvar) {
      return;
    }
    this.spinner.show();
    const cliente = {
      nome: this.nome,
      cpfCnpj: this.cpfCnpj,
      rg: this.rg,
      telefone: this.telefone,
      email: this.email,
      tipoPessoa: this.tipoPessoa,
      rua: this.rua,
      complemento: this.complemento,
      cep: this.cep,
      bairro: this.bairro,
      cidade: this.cidade,
      estado: this.estado,
      ddd: this.ddd,
      responsavelContato: this.responsavelContato,
      origemContato: this.origemContato,
      redeSocial: this.redeSocial,
      obs: this.obs,
      razaoSocial: this.razaoSocial,
      sexo:this.sexo,
      estadoCivil:this.estadoCivil,
      orgaoExpedidor:this.orgaoExpedidor,
    };
    console.log('Filtro enviado corretamente:', cliente);

    this.clienteService.salvarCliente(cliente).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Cliente cadastrado!', 'Sucesso');
        this.podeSalvar = false;
      },
      error: (err) => {
        this.spinner.hide();
        this.toastr.error('Erro ao salvar cliente', 'Erro');
        this.podeSalvar = false;
      },
    });
  }

  private definirClienteAtual(c: CadastroClientes) {
    this.selectedCliente = c;

    this.idCliente = c.id;
    this.nome = c.nome;
    this.cpfCnpj = c.cpfCnpj;

    this.prospeccoes = c.prospeccoes ?? [];
    this.mostrarHistorico = false;

    this.prospeccao = this.prospeccoes.length
      ? { ...this.prospeccoes[0] }
      : this.novaProspeccao();
  }
  //-------------- PROSPECÇÃO -----------------//
  novaProspeccao(): ProspeccaoCliente {
    return {
      id: 0,
      clienteId: this.selectedCliente!.id,
      etapa: '',
      probabilidade: 0,
      canal: '',
      responsavel: '',
      origemContato: '',
      interessePrincipal: '',
      necessidade: '',
      dataCriacao: new Date().toISOString(),
    };
  }

  selecionarProspeccao(p: ProspeccaoCliente) {
    this.prospeccao = { ...p }; // clone para não editar direto
    this.mostrarHistorico = false;
  }

  addNovaProspeccao() {
    if (!this.idCliente) return;

    this.spinner.show();

    this.prospeccaoService
      .addProspeccao(this.idCliente, this.prospeccao)
      .subscribe({
        next: (res) => {
          this.spinner.hide();
          this.toastr.success('Prospecção criada!', 'Sucesso');

          // adiciona no topo do histórico
          this.prospeccoes.unshift(res);

          // seleciona a nova prospecção
          this.prospeccao = { ...res };
          this.mostrarHistorico = false;
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Erro ao criar prospecção', 'Erro');
        },
      });
  }

  atualizarProspeccao() {
    if (!this.idCliente || !this.prospeccao.id) return;

    const payload = {
      etapa: this.prospeccao.etapa,
      probabilidade: this.prospeccao.probabilidade,
      origemContato: this.prospeccao.origemContato,
      interessePrincipal: this.prospeccao.interessePrincipal,
      necessidade: this.prospeccao.necessidade,
      dataProximoContato: this.prospeccao.dataProximoContato,
      canal: this.prospeccao.canal,
      responsavel: this.prospeccao.responsavel,
      observacoes: this.prospeccao.observacoes,
    };

    console.group('📤 PAYLOAD ENVIADO PARA O BACKEND');
    console.log('clienteId:', this.idCliente);
    console.log('prospeccaoId:', this.prospeccao.id);
    console.log('payload:', JSON.stringify(payload, null, 2));
    console.groupEnd();

    this.spinner.show();

    this.prospeccaoService
      .atualizarProspeccao(this.idCliente, this.prospeccao.id, payload)
      .subscribe({
        next: (res) => {
          this.spinner.hide();
          this.toastr.success('Prospecção atualizada!', 'Sucesso');
          this.carregarProspeccoes();
        },
        error: (err) => {
          this.spinner.hide();
          console.error('❌ ERRO BACKEND:', err);
          this.toastr.error('Erro ao atualizar prospecção', 'Erro');
        },
      });
  }

  carregarProspeccoes() {
    this.prospeccaoService
      .getProspeccao(this.id)
      .subscribe((res) => (this.prospeccoes = res));
  }

  limparProspeccao() {}

  selecionarClienteProspeccao(c: CadastroClientes) {
    this.idCliente = c.id;
    this.nome = c.nome;

    this.prospeccoes = c.prospeccoes ?? [];
    this.mostrarHistorico = false;

    // carrega a prospecção mais recente no formulário
    if (this.prospeccoes.length > 0) {
      this.prospeccao = { ...this.prospeccoes[0] };
    } else {
      this.prospeccao = this.novaProspeccao();
    }
  }

  toggleHistorico() {
    // se vai abrir o histórico, recarrega do backend
    if (!this.mostrarHistorico) {
      this.recarregarProspecoes();
    }

    this.mostrarHistorico = !this.mostrarHistorico;
  }

  recarregarProspecoes() {
    if (!this.idCliente) return;

    this.spinner.show();

    this.clienteService.pesquisarClientes({ id: this.idCliente }).subscribe({
      next: (res) => {
        this.spinner.hide();

        if (!res || !res.length) return;

        const cliente = res[0];

        // 🔥 substitui o array inteiro (remove cache)
        this.prospeccoes = [...(cliente.prospeccoes ?? [])];

        // opcional: ordena por data (mais recente primeiro)
        this.prospeccoes.sort((a, b) => {
          if (!a.dataCriacao || !b.dataCriacao) return 0;

          return (
            new Date(b.dataCriacao).getTime() -
            new Date(a.dataCriacao).getTime()
          );
        });
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar histórico de prospecções', 'Erro');
      },
    });
  }

  pesquisarClientesProspeccao() {}

  //---------- BOTÕES ------------//

  alterar() {
    this.podeSalvar = true;
  }

  cancelar() {
    this.podeSalvar = false;
  }
}
