import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-Login',
    templateUrl: './Login.component.html',
    styleUrls: ['./Login.component.scss'],
    standalone: false
})
export class LoginComponent implements OnInit {

  username: string = '';
  password: string = '';
  resetEmail: string = '';
  showChangePassword: boolean = false;
  login = { email: '', senha: '', lembrar: false };

  constructor() { }

  ngOnInit() {
  }

  onLogin(){

  }

  openChangePassword(){

  }

  closeChangePassword(){

  }

  sendReset(){

  }

}
