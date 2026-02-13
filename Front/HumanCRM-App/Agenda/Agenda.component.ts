import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Agenda',
  templateUrl: './Agenda.component.html',
  styleUrls: ['./Agenda.component.scss'],
  standalone: false,
})
export class AgendaComponent implements OnInit {
  agendaSemana: any[] = [];
  semanaInicio!: Date;
  semanaFim!: Date;
  modoAgenda: 'diario' | 'semanal' = 'diario';
  dataSelecionada: string = '';
  dataInicioSemana: string = '';
  dataFimSemana: string = '';
  textoBusca: string = '';
  statusFiltro: string = '';
  ordemFiltro: string = '';
  loading: boolean = false;
  ordem: string = 'asc';
  agendamentosFiltrados: any[] = [];
  totais: { confirmados: number; pendentes: number; cancelados: number } = {
    confirmados: 0,
    pendentes: 0,
    cancelados: 0,
  };

  constructor() {}

  ngOnInit() {}

  carregarAgendamentos() {}

  selecionarDia(data: string) {}

  setModoAgenda(modo: string) {}

  atualizarSemanaECarregar() {}

  filtrarAgendamentos() {}

  irParaHoje() {}

  novoAgendamento() {}

  abrirDetalhes(a: string) {}

  editarAgendamento(a: any) {}

  cancelarAgendamento(a: any) {}
}

