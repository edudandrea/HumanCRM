import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-Agenda',
    templateUrl: './Agenda.component.html',
    styleUrls: ['./Agenda.component.scss'],
    standalone: false
})
export class AgendaComponent implements OnInit {

  agendaSemana: any[] = [];
  semanaInicio!: Date;
  semanaFim!: Date;
  

  constructor() { }
  

  ngOnInit() {
  }

  carregarAgendamentos(){
    
  }

  selecionarDia(data: string){

  }

}
