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

import { SidebarComponent } from 'Sidebar/Sidebar.component';
import { DashboardComponent } from 'Dashboard/Dashboard.component';
import { UsersComponentComponent } from 'Users/Users.component';
import { AgendaComponent } from 'Agenda/Agenda.component';
import { LoginComponent } from 'Login/Login.component';
import { LayoutComponent } from 'Layout/Layout.component';
import { provideAnimations } from '@angular/platform-browser/animations';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CadastroClienteComponent } from 'CadastroCliente/CadastroCliente.component';



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
    bootstrap: [AppComponent], imports: [
        BrowserModule,
        AppRoutingModule,
        FormsModule,
        NgxSpinnerModule,
        ModalModule.forRoot(),
        ToastrModule.forRoot({ timeOut: 1000,
            positionClass: 'toast-bottom-right',
            preventDuplicates: true,
            progressBar: true,
            closeButton: true,
        })], providers: [
            provideAnimations(),
            provideHttpClient(withInterceptorsFromDi())
        
        ] })
export class AppModule { }
