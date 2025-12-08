//-- Modulos --//
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';

//-- Componentes --//

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CadastroClienteComponent } from 'CadastroCliente/CadastroCliente.component';
import { SidebarComponent } from 'Sidebar/Sidebar.component';
import { DashboardComponent } from 'Dashboard/Dashboard.component';
import { UsersComponentComponent } from 'Users/Users.component';
import { AgendaComponent } from 'Agenda/Agenda.component';
import { LoginComponent } from 'Login/Login.component';
import { LayoutComponent } from 'Layout/Layout.component';


@NgModule({ declarations: [	
        AppComponent,
        CadastroClienteComponent,
        SidebarComponent,
        DashboardComponent,
        UsersComponentComponent,
        AgendaComponent,
        LoginComponent,
        LayoutComponent,
        
   ],
    bootstrap: [AppComponent], imports: [BrowserModule,
        AppRoutingModule,
        FormsModule,
        NgxSpinnerModule,
        ToastrModule.forRoot({ timeOut: 5000,
            positionClass: 'toast-bottom-right',
            preventDuplicates: true,
            progressBar: true,
            closeButton: true,
        })], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
