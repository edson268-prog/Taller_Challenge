import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  private authService = inject(AuthService);

  username = computed(
    () => this.authService.currentUser()?.username || 'Visitor',
  );

  private allActions = [
    {
      label: 'New Order',
      path: '/orders/new',
      description: 'Create new service order',
      onlyAdmin: true,
    },
    {
      label: 'See Orders',
      path: '/orders',
      description: 'List all orders',
      onlyAdmin: false,
    },
  ];

  quickActions = computed(() => {
    const role = this.authService.currentUser()?.role;

    return this.allActions.filter((action) => {
      return !action.onlyAdmin || role === 'Admin';
    });
  });
}
