import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CadastroClientes, ClientesService } from 'Services/Clientes.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { ElementRef, ViewChild } from '@angular/core';
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

  clientesFiltrados: CadastroClientes[] = [];
  mostrarListaClientes = false;

  idCliente: number | any;
  nome = '';
  cpfCnpj: number | any = '';
  rg: number | any = '';
  ddd: number | any = '';
  telefone: string | any = '';
  celular?: number;
  email = '';
  tipoPessoa = 'Física';

  rua = '';
  complemento = '';
  numero: number | any = '';
  cep: number | null = null;
  bairro = '';
  cidade = '';
  estado = '';

  responsavelContato = '';
  origemContato = '';
  redeSocial = '';
  observacoes = '';
  razaoSocial = '';
  IE: number | any = '';
  IM: number | any = '';
  dataFundacao = '';
  dataProximoContato = '';
  dataNascimento = '';

  orgaoExpedidor = '';
  sexo: number | any = '';
  estadoCivil: number | any = '';
  dataContato = '';

  // ---------- Prospecção ----------
  prospeccoes: ProspeccaoCliente[] = [];

  prospeccaoSelecionada: ProspeccaoCliente | null = null;

  prospeccao: Partial<ProspeccaoCliente> = this.criarProspeccaoVazia();

  // ---------- Filtros ----------
  filtroClientes = {
    id: '',
    nome: '',
    cpfCnpj: '',
    telefone: '',
  };

  // -------------- Documentos --------------
  documentosSelecionados: Array<{
    nome: string;
    tipo: 'pdf' | 'png';
    url: SafeResourceUrl;
    rawUrl: string;
    tabId: number;
  }> = [];

  documentos: CadastroClientes[] = [];
  documentoAtual: CadastroClientes | null = null;

  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  constructor(
    private toastr: ToastrService,
    private clienteService: ClientesService,
    private prospeccaoService: ProspeccaoService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService,
    private sanitizer: DomSanitizer,
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
    p: ProspeccaoCliente,
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
            this.prospeccao.dataProximoContato + 'T00:00:00',
          ).toISOString()
        : null,
    };
  }

  // =====================================================
  // 🔹 CLIENTES
  // =====================================================

  pesquisarClientesAutocomplete(nome: string): void {
    const termo = String(nome ?? '').trim();

    if (termo.length < 2) {
      this.clientesFiltrados = [];
      this.mostrarListaClientes = false;
      return;
    }

    const filtro: any = { nome: termo };

    this.clienteService.pesquisarClientes(filtro).subscribe({
      next: (res) => {
        this.clientesFiltrados = res ?? [];
        this.mostrarListaClientes = this.clientesFiltrados.length > 0;
      },
      error: () => {
        this.clientesFiltrados = [];
        this.mostrarListaClientes = false;
      },
    });
  }

  selecionarClienteDaLista(cliente: CadastroClientes) {
    this.filtroClientes.nome = cliente.nome; // preenche o input (opcional)
    this.mostrarListaClientes = false;
    this.clientesFiltrados = [];

    this.setClienteAtual(cliente); // ✅ reaproveita seu método
  }

  fecharListaClientesComDelay() {
    setTimeout(() => {
      this.mostrarListaClientes = false;
    }, 150);
  }

  salvarNovoCliente() {
    this.spinner.show();

    const cliente = {
      nome: this.nome,
      cpfCnpj: this.cpfCnpj,
      email: this.email,
      tipoPessoa: this.tipoPessoa,
      ddd: Number(this.ddd),
      celular: this.celular ? Number(this.celular) : undefined,
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
      nome: String(this.nome ?? '').trim(),
      cpfCnpj: this.cpfCnpj,
      rg: this.rg,

      telefone: this.telefone,
      celular: this.celular,       

      email: this.email,
      tipoPessoa: String(this.tipoPessoa ?? '').trim(),

      rua: this.rua,
      numero: this.numero ? Number(this.numero) : 0,
      complemento: this.complemento,
      cep: this.cep ? Number(this.cep) : undefined,

      bairro: this.bairro,
      cidade: this.cidade,
      estado: this.estado,

      ddd: this.ddd ? Number(this.ddd) : 0,

      responsavelContato: this.responsavelContato,
      origemContato: this.origemContato,
      redeSocial: this.redeSocial,
      observacoes: this.observacoes,
      razaoSocial: this.razaoSocial,

      sexo: this.sexo ?? null,
      estadoCivil: this.estadoCivil ?? null,
      orgaoExpedidor: this.orgaoExpedidor,
      dataNascimento: this.dataNascimento ?? null,
    };

    console.group('📤 ATUALIZAR CLIENTE');
    console.log(JSON.stringify(payload, null, 2));
    console.groupEnd();

    this.spinner.show();

    this.clienteService.atualizarCliente(payload).subscribe({
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

    this.carregarDocumentosCliente(this.idCliente);
  }

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
    this.sexo = c.sexo ?? '';
    this.estadoCivil = c.estadoCivil ?? '';
    this.dataNascimento = c.dataNascimento ?? '';
    this.orgaoExpedidor = c.orgaoExpedidor;

    // endereço
    this.rua = c.rua ?? '';
    this.complemento = c.complemento ?? '';
    this.numero = c.numero ?? '';
    this.cep = c.cep ?? null;
    this.bairro = c.bairro ?? '';
    this.cidade = c.cidade ?? '';
    this.estado = c.estado ?? '';

    // outros
    this.responsavelContato = c.responsavelContato ?? '';
    this.origemContato = c.origemContato ?? '';
    this.redeSocial = c.redeSocial ?? '';
    this.observacoes = c.observacoes ?? '';
    this.razaoSocial = c.razaoSocial ?? '';
    this.IE = c.IE ?? '';
    this.IM = c.IM ?? '';
    this.orgaoExpedidor = c.orgaoExpedidor ?? '';

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
    this.prospeccaoSelecionada = p;
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

  // =====================================================
  // 🔹 DOCUMENTOS
  // =====================================================

  abrirSeletorDocumento(): void {
    if (!this.idCliente) {
      this.toastr.warning(
        'Selecione um cliente antes de adicionar documentos.',
      );
      return;
    }
    this.fileInput.nativeElement.value = '';
    this.fileInput.nativeElement.click();
  }

  abrirPreview(doc: CadastroClientes, template: any): void {
    this.documentoAtual = doc;
    this.modalRef = this.modalService.show(template, { class: 'modal-xl' });
  }

  fecharPreview(): void {
    this.modalRef?.hide();
    this.modalRef = undefined;
    this.documentoAtual = null;
  }

  visualizarDocumento(doc: any): void {
    this.documentoAtual = doc;
    // se você quiser usar subTabActive:
    this.subTabActive = doc.tabId;
  }

  onDocumentoSelecionado(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];
    if (!file) return;

    const isPdf = file.type === 'application/pdf';
    const isPng = file.type === 'image/png';

    if (!isPdf && !isPng) {
      this.toastr.warning('Selecione um arquivo PDF ou PNG.');
      return;
    }

    // ✅ sobe pro backend e salva vinculado ao cliente
    this.uploadDocumentoCliente(this.idCliente, file);
  }

  private uploadDocumentoCliente(clienteId: number, file: File): void {
    this.spinner.show();

    this.clienteService.uploadDocumento(clienteId, file).subscribe({
      next: (doc: CadastroClientes) => {
        this.spinner.hide();

        // prepara viewer
        this.prepararDocumentoParaUI(doc);

        this.documentos = [doc, ...this.documentos];
        this.toastr.success('Documento adicionado com sucesso!');
      },
      error: (err) => {
        this.spinner.hide();
        console.error(err);
        this.toastr.error('Erro ao salvar documento.');
      },
    });
  }

  private prepararDocumentoParaUI(doc: CadastroClientes): void {
    // ✅ PNG: previewUrl pode ser o próprio downloadUrl (se o endpoint devolver o arquivo direto)
    if (doc.contentType === 'image/png') {
      doc.previewUrl = doc.downloadUrl;
    }

    // ✅ PDF: iframe precisa de SafeResourceUrl
    if (doc.contentType === 'application/pdf') {
      doc.safeViewerUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
        doc.downloadUrl,
      );
    }
  }

  downloadDocumento(doc: any): void {
    const a = document.createElement('a');
    a.href = doc.rawUrl;
    a.download = doc.nome;
    a.target = '_blank';
    a.click();
    a.remove();
  }

  excluirDocumento(doc: CadastroClientes): void {
    if (!doc?.id) return;

    // opcional: confirmação
    const ok = confirm(`Excluir o documento "${doc.nomeArquivo}"?`);
    if (!ok) return;

    this.spinner.show();
    this.clienteService.excluirDocumento(this.idCliente, doc.id).subscribe({
      next: () => {
        this.spinner.hide();
        this.documentos = this.documentos.filter((d) => d.id !== doc.id);
        if (this.documentoAtual?.id === doc.id) this.fecharPreview();
        this.toastr.success('Documento excluído!');
      },
      error: (err) => {
        this.spinner.hide();
        console.error(err);
        this.toastr.error('Erro ao excluir documento.');
      },
    });
  }

  carregarDocumentosCliente(clienteId: number) {
    this.documentos = []; // limpa antes

    this.clienteService.listarDocumentos(clienteId).subscribe({
      next: (docs) => {
        this.documentos = docs ?? [];
        // se você usa previewUrl/safeViewerUrl, dá pra montar aqui também
      },
      error: () => {
        this.documentos = [];
        this.toastr.error('Erro ao carregar documentos do cliente');
      },
    });
  }
}
