import { Component, OnInit } from '@angular/core';

import { AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import {
  Chart,
  PieController,
  ArcElement,
  Tooltip,
  Legend
} from 'chart.js';

Chart.register(PieController, ArcElement, Tooltip, Legend);


@Component({
  selector: 'app-Dashboard',
  templateUrl: './Dashboard.component.html',
  styleUrls: ['./Dashboard.component.scss'],  
  standalone: false,
})
export class DashboardComponent implements OnInit {
  hoje: Date | string = new Date();
  kpis: {
    agendadosHoje: number;
    prospeccoesAlta: number;
    contratosAtivos: number;
    contratosAVencer: number;
    aguardandoRetorno: number;
  } = {
    agendadosHoje: 0,
    prospeccoesAlta: 0,
    contratosAtivos: 0,
    contratosAVencer: 0,
    aguardandoRetorno: 0,
  };
  agendadosHoje: any[] = [];
  prospeccoesAlta: any[] = [];
  contratosAtivos: any[] = [];
  contratosAVencer: any[] = [];
  aguardandoRetorno: any[] = [];

  @ViewChild('pieCanvas', { static: true }) pieCanvas!: ElementRef<HTMLCanvasElement>;
  private pieChart?: Chart<'pie'>;
  

  constructor() {}

  ngOnInit() {
    this.criarGrafico();
    this.getValoresPizza();
  }

  private criarGrafico(): void {
    const ctx = this.pieCanvas.nativeElement.getContext('2d');
    if (!ctx) return;

    this.pieChart = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: [
          'Agendados hoje',
          'Prospecções > 60%',
          'Contratos ativos',
          'Aguardando retorno'
        ],
        datasets: [{
          data: this.getValoresPizza(),
          // cores opcionais (se quiser, posso deixar iguais seus KPI cards)
          backgroundColor: ['#4f46e5', '#16a34a', '#0ea5e9', '#f59e0b'],
          borderWidth: 0,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom' },
          tooltip: {
            callbacks: {
              label: (item) => {
                const label = item.label ?? '';
                const value = Number(item.raw ?? 0);
                return `${label}: ${value}`;
              }
            }
          }
        }
      }
    });
  }

  private getValoresPizza(): number[] {
    return [
      Number(this.kpis?.agendadosHoje || 10),
      Number(this.kpis?.prospeccoesAlta || 10),
      Number(this.kpis?.contratosAtivos || 10),
      Number(this.kpis?.aguardandoRetorno || 10),
    ];
  }

  atualizarGrafico(): void {
    if (!this.pieChart) return;

    this.pieChart.data.datasets[0].data = this.getValoresPizza();
    this.pieChart.update();
  }

  atualizarDashboard(): void {
    // ... sua rotina que busca dados e seta this.kpis
    // quando terminar:
    this.atualizarGrafico();
  }

  irParaHoje() {
    this.hoje = new Date();
  }
  abrirAgendaHoje() {}

  abrirAgendamento(a: any) {}

  abrirProspAlta() {}

  abrirProspeccao(p: any) {}

  abrirContratosAtivos() {}

  abrirContrato(c: any) {}

  abrirContratosAVencer() {}

  iniciarRenovacao(v: any) {}

  abrirAguardandoRetorno() {}

  abrirCliente(id: any) {}

  registrarRetorno(r: any) {}

  novoAgendamentoParaCliente(r: any) {}
}
