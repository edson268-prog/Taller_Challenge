import { CommonModule } from '@angular/common';
import { Component, computed, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Order, OrderParams, OrderStatus } from '../../models/order';
import { OrderService } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css',
})
export class OrderListComponent implements OnInit {
  private authService = inject(AuthService);

  isAdmin = computed(() => this.authService.currentUser()?.role === 'Admin');

  orders: Order[] = [];
  filteredOrders: Order[] = [];
  loading = false;
  error = '';
  searchTerm = '';

  currentPage = 1;
  pageSize = 5;
  currentSort = 'desc';
  selectedStatus: OrderStatus | string | null = '';

  statusOptions = [
    { value: null, label: 'All' },
    { value: OrderStatus.Pending, label: 'Pending' },
    { value: OrderStatus.Approved, label: 'Approved' },
    { value: OrderStatus.Completed, label: 'Completed' },
  ];

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;

    const params: OrderParams = {
      status: this.selectedStatus,
      page: this.currentPage,
      pageSize: this.pageSize,
      sortOrder: this.currentSort,
    };

    this.orderService.getOrders(params).subscribe({
      next: (orders) => {
        this.orders = orders;
        this.filteredOrders = orders;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error loading orders.';
        this.loading = false;
      },
    });
  }

  changePage(newPage: number) {
    this.currentPage = newPage;
    this.loadOrders();
  }

  changeSort(order: string) {
    this.currentSort = order;
    this.currentPage = 1;
    this.loadOrders();
  }

  filterByStatus(status: OrderStatus | string | null) {
    this.selectedStatus = status;
    this.currentPage = 1;
    this.loadOrders();
  }

  getStatusBadgeClass(status: OrderStatus | number): string {
    switch (status) {
      case OrderStatus.Pending:
        return 'status-pending';
      case OrderStatus.Approved:
        return 'status-approved';
      case OrderStatus.Completed:
        return 'status-completed';
      default:
        return 'bg-secondary';
    }
  }
}
