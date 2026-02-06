import { CommonModule } from '@angular/common';
import { Component, computed, inject } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css',
})
export class LayoutComponent {
  private authService = inject(AuthService);

  isAdmin = computed(() => this.authService.currentUser()?.role === 'Admin');

  username = localStorage.getItem('username') || 'Admin';
  pageTitle = 'Dashboard';

  private allMenuItems = [
    {
      path: '/home',
      icon: 'fas fa-home',
      label: 'Dashboard',
      adminOnly: false,
    },
    {
      path: '/orders',
      icon: 'fas fa-list-alt',
      label: 'Orders',
      adminOnly: false,
    },
    {
      path: '/orders/new',
      icon: 'fas fa-plus-circle',
      label: 'New Order',
      adminOnly: true,
    },
  ];

  menuItems = computed(() => {
    const role = this.authService.currentUser()?.role;
    return this.allMenuItems.filter(
      (item) => !item.adminOnly || role === 'Admin',
    );
  });

  logout() {
    this.authService.logout();
    window.location.href = '/';
  }
}
