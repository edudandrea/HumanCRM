import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-UsersComponent',
    templateUrl: './Users.component.html',
    styleUrls: ['./Users.component.scss'],
    standalone: false
})
export class UsersComponentComponent implements OnInit {
  bsModalRef?: BsModalRef
  userId: number = 0;
  login: string = '';
  userName: string = '';
  password: string = '';
  function: string = '';
  selectedUser: any = [];
  selectedFunction: string = '';
  users: any[] = [];
  filteredUsers: any[] = [];
  searchTerm: string = '';
  editedUser: any = [];
  showPassword = false;

  constructor(private modal: BsModalService) { }

  ngOnInit() {
  }

  searchUsers(){

  }

  openModalEdit(templateEdit: any, userId: any){

  }

  deleteUser(userId: any, login: any){
  }
  openModal(template: any){

  }

  togglePasswordVisibility(){

  }

  addUser(){

  }

  editUser(){

  }
}
