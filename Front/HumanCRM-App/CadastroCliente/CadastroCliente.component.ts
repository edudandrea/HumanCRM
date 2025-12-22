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
  // ---------- UI ----------
  activeTab = 1;
  subTabActive = 1;
  mostrarHistorico = false;
  podeSalvar = false;

  modalRef?: BsModalRef;

  // ---------- Cliente ----------
  cliente: CadastroClientes[] = [];
  selectedCliente?: CadastroClientes;

  idCliente: number | null = null;
  nome = '';
  cpfCnpj: number | any = '';
  rg: number | any = '';
  ddd: number | any = '';
  telefone = '';
  celular: number | any = '';
  email = '';
  tipoPessoa = 'Física';

  rua = '';
  complemento = '';
  numero: number | any = '';
  cep: number | any = '';
  bairro = '';
  cidade = '';
  estado = '';

  responsavelContato = '';
  origemContato = '';
  redeSocial = '';
  obs = '';
  razaoSocial = '';
  IE: number | any = '';
  IM: number | any = '';
  dataFundacao = '';
  dataProximoContato = '';

  orgaoExpedidor = '';
  sexo: number | any = '';
  estadoCivil: number | any = '';
  dataContato = '';

  // ---------- Prospecção ----------
  prospeccoes: ProspeccaoCliente[] = [];

  prospeccao: Partial<ProspeccaoCliente> = this.criarProspeccaoVazia();

  // ---------- Filtros ----------
  filtroClientes = {
    id: '',
    nome: '',
    cpfCnpj: '',
    telefone: '',
  };

  constructor(
    private toastr: ToastrService,
    private clienteService: ClientesService,
    private prospeccaoService: ProspeccaoService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {}

  ngOnInit(): void {}

  // =====================================================
  // 🔹 HELPERS (ESSENCIAIS)
  // =====================================================

  /** Cria uma prospecção vazia (formulário) */
  private criarProspeccaoVazia(): Partial<ProspeccaoCliente> {
    return {
      etapa: '',
      probabilidade: 0,
      origemContato: '',
      interessePrincipal: '',
      necessidade: '',
      dataProximoContato: null,
      canal: '',
      responsavel: '',
      observacoes: '',
    };
  }

  /** Normaliza data ISO → yyyy-MM-dd (para input type="date") */
  private normalizarProspeccao(
    p: ProspeccaoCliente
  ): Partial<ProspeccaoCliente> {
    return {
      ...p,
      dataProximoContato: p.dataProximoContato
        ? new Date(p.dataProximoContato).toISOString().substring(0, 10)
        : null,
    };
  }

  /** Prepara payload para backend (yyyy-MM-dd → ISO) */
  private prepararPayloadProspeccao() {
    return {
      ...this.prospeccao,
      dataProximoContato: this.prospeccao.dataProximoContato
        ? new Date(
            this.prospeccao.dataProximoContato + 'T00:00:00'
          ).toISOString()
        : null,
    };
  }

  // =====================================================
  // 🔹 CLIENTES
  // =====================================================

  pesquisarClientes(filtro: any): void {
    this.spinner.show();

    this.clienteService.pesquisarClientes(filtro).subscribe({
      next: (res) => {
        this.spinner.hide();

        if (!res?.length) {
          this.toastr.info('Nenhum cliente encontrado.');
          return;
        }

        this.carregarCliente(res[0]);
        this.activeTab = 2;
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error('Erro ao buscar clientes');
      },
    });
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

    console.group('📤 NOVO CLIENTE');
    console.log(JSON.stringify(cliente, null, 2));
    console.groupEnd();

    this.clienteService.novoCliente(cliente).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Cliente cadastrado!', 'Sucesso');

        // 🔥 já carrega o cliente recém-criado
        this.carregarCliente(res);

        this.activeTab = 2;
        this.fecharModal();
      },
      error: (err) => {
        this.spinner.hide();
        console.error(err);
        this.toastr.error('Erro ao salvar cliente', 'Erro');
      },
    });
  }

  atualizarCliente() {
    if (!this.idCliente) {
      this.toastr.warning('Cliente não selecionado.', 'Atenção');
      return;
    }

    if (!this.podeSalvar) return;

    const payload = {
      id: this.idCliente,
      nome: this.nome,
      cpfCnpj: this.cpfCnpj,
      rg: this.rg,
      telefone: this.telefone,
      celular: this.celular,
      email: this.email,
      tipoPessoa: this.tipoPessoa,
      rua: this.rua,
      numero: this.numero,
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
      sexo: this.sexo,
      estadoCivil: this.estadoCivil,
      orgaoExpedidor: this.orgaoExpedidor,
    };

    console.group('📤 ATUALIZAR CLIENTE');
    console.log(JSON.stringify(payload, null, 2));
    console.groupEnd();

    this.spinner.show();

    this.clienteService.salvarCliente(payload).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Cliente atualizado com sucesso!', 'Sucesso');

        // 🔥 mantém estado atualizado
        this.carregarCliente(res);
        this.podeSalvar = false;
      },
      error: (err) => {
        this.spinner.hide();
        console.error(err);
        this.toastr.error('Erro ao atualizar cliente', 'Erro');
      },
    });
  }

  setClienteAtual(cliente: CadastroClientes) {
    if (!cliente) return;

    this.selectedCliente = cliente;

    // dados principais
    this.idCliente = cliente.id;
    this.nome = cliente.nome;
    this.cpfCnpj = cliente.cpfCnpj;

    // histórico de prospecções
    this.prospeccoes = cliente.prospeccoes ?? [];
    this.mostrarHistorico = false;

    // carrega a prospecção mais recente no formulário
    if (this.prospeccoes.length > 0) {
      this.prospeccao = this.normalizarProspeccao(this.prospeccoes[0]);
    } else {
      this.iniciarProspeccao();
    }
  }

  onPesquisarClick(): void {
    const filtro: any = {};
    if (this.filtroClientes.id) filtro.id = Number(this.filtroClientes.id);
    if (this.filtroClientes.nome) filtro.nome = this.filtroClientes.nome;
    if (this.filtroClientes.cpfCnpj)
      filtro.cpfCnpj = this.filtroClientes.cpfCnpj;
    if (this.filtroClientes.telefone)
      filtro.telefone = this.filtroClientes.telefone;

    this.pesquisarClientes(filtro);
  }

  private carregarCliente(c: CadastroClientes) {
    this.selectedCliente = c;
    this.idCliente = c.id;

    // dados básicos
    this.nome = c.nome ?? '';
    this.cpfCnpj = c.cpfCnpj ?? '';
    this.rg = c.rg ?? '';
    this.ddd = c.ddd ?? '';
    this.telefone = c.telefone ?? '';
    this.celular = c.celular ?? '';
    this.email = c.email ?? '';
    this.tipoPessoa = c.tipoPessoa ?? 'Física';

    // endereço
    this.rua = c.rua ?? '';
    this.complemento = c.complemento ?? '';
    this.cep = c.cep ?? '';
    this.bairro = c.bairro ?? '';
    this.cidade = c.cidade ?? '';
    this.estado = c.estado ?? '';

    // outros
    this.responsavelContato = c.responsavelContato ?? '';
    this.origemContato = c.origemContato ?? '';
    this.redeSocial = c.redeSocial ?? '';
    this.obs = c.obs ?? '';
    this.razaoSocial = c.razaoSocial ?? '';
    this.IE = c.IE ?? '';
    this.IM = c.IM ?? '';

    // prospecções
    this.prospeccoes = c.prospeccoes ?? [];
    this.mostrarHistorico = false;

    this.prospeccao = this.prospeccoes.length
      ? this.normalizarProspeccao(this.prospeccoes[0])
      : this.criarProspeccaoVazia();
  }

  // =====================================================
  // 🔹 PROSPECÇÕES
  // =====================================================

  iniciarProspeccao() {
    this.prospeccao = this.criarProspeccaoVazia();
  }

  selecionarProspeccao(p: ProspeccaoCliente) {    
    this.prospeccao = this.normalizarProspeccao(p);
    this.mostrarHistorico = false;
    console.log('📅 DATA NO FORM:', this.prospeccao.dataProximoContato);
  }

  

  addNovaProspeccao() {
    if (!this.idCliente) return;

    const payload = this.prepararPayloadProspeccao();

    this.spinner.show();
    this.prospeccaoService.addProspeccao(this.idCliente, payload).subscribe({
      next: (res) => {
        this.spinner.hide();
        this.toastr.success('Prospecção criada!');

        this.prospeccoes.unshift(res);
        this.prospeccao = this.normalizarProspeccao(res);
        this.mostrarHistorico = false;

        this.fecharModal();
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error('Erro ao criar prospecção');
      },
    });
  }

  atualizarProspeccao() {
    if (!this.idCliente || !this.prospeccao.id) return;

    const payload = this.prepararPayloadProspeccao();

    this.spinner.show();
    this.prospeccaoService
      .atualizarProspeccao(this.idCliente, this.prospeccao.id, payload)
      .subscribe({
        next: () => {
          this.spinner.hide();
          this.toastr.success('Prospecção atualizada!');
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Erro ao atualizar prospecção');
        },
      });
  }

  toggleHistorico() {
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
        if (!res?.length) return;

        this.prospeccoes = [...(res[0].prospeccoes ?? [])];
      },
      error: () => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar histórico');
      },
    });
  }

  // =====================================================
  // 🔹 MODAL
  // =====================================================

  openNovoClienteModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {
      class: 'modal-lg modal-dialog-centered',
    });
  }

  openNovoProspModal(template: TemplateRef<any>) {
  this.prospeccao = this.criarProspeccaoVazia();
  this.modalRef = this.modalService.show(template, {
    class: 'modal-lg modal-dialog-centered',
  });
}


  fecharModal() {
    this.modalRef?.hide();
  }

  // =====================================================
  // 🔹 BOTÕES
  // =====================================================

  alterar() {
    this.podeSalvar = true;
  }

  cancelar() {
    this.podeSalvar = false;
  }

  // =====================================================
  // 🔹 SUBTABS
  // =====================================================

  setActiveTab(tabNumber: number) {
    this.activeTab = tabNumber;
    if (tabNumber === 3 && this.selectedCliente) {
      this.setClienteAtual(this.selectedCliente);
    }
  }
  setActiveSubTab(subTabNumber: number) {
    this.subTabActive = subTabNumber;
  }
}
