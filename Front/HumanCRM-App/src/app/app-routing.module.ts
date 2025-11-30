import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AgendaComponent } from 'Agenda/Agenda.component';
import { CadastroClienteComponent } from 'CadastroCliente/CadastroCliente.component';
import { DashboardComponent } from 'Dashboard/Dashboard.component';
import { LoginComponent } from 'Login/Login.component';
import { SidebarComponent } from 'Sidebar/Sidebar.component';
import { UsersComponentComponent } from 'Users/Users.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: SidebarComponent,

    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'clientes', component: CadastroClienteComponent },
      { path: 'agendamento', component: AgendaComponent },
      { path: 'users', component: UsersComponentComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
    ],
  },

  { path: '**', redirectTo: 'login' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
