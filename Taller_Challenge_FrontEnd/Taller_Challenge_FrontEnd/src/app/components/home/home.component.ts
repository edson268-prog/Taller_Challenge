import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  username = localStorage.getItem('username') || 'Administrador';

  quickActions = [
    {
      label: 'New Order',
      path: '/orders/new',
      description: 'Create new service order',
    },
    {
      label: 'See Orders',
      path: '/orders',
      description: 'List all orders',
    },
  ];
}
