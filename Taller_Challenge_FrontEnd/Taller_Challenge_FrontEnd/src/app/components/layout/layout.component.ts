import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css',
})
export class LayoutComponent {
  username = localStorage.getItem('username') || 'Admin';
  pageTitle = 'Dashboard';

  menuItems = [
    { path: '/home', icon: 'fas fa-home', label: 'Dashboard' },
    { path: '/orders', icon: 'fas fa-list-alt', label: 'Orders' },
    { path: '/orders/new', icon: 'fas fa-plus-circle', label: 'New Order' },
  ];

  logout() {
    localStorage.removeItem('isLoggedIn');
    localStorage.removeItem('username');
    window.location.href = '/';
  }
}
