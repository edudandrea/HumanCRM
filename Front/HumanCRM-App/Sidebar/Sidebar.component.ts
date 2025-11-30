import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-Sidebar',
    templateUrl: './Sidebar.component.html',
    styleUrls: ['./Sidebar.component.scss'],
    standalone: false
})
export class SidebarComponent implements OnInit {

  title = 'HumanCRM';
  isClientesMenuOpen = false;
  role: string | null = null;
  sidebarOpen = true;

  constructor() { }

  ngOnInit() {
  }

  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  toggleClientesMenu() {
    this.isClientesMenuOpen = !this.isClientesMenuOpen;
  }

}
