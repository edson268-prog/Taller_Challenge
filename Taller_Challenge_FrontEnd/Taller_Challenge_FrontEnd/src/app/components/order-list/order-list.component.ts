import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Order, OrderStatus } from '../../models/order';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css',
})
export class OrderListComponent implements OnInit {
  orders: Order[] = [];
  filteredOrders: Order[] = [];
  loading = false;
  error = '';
  searchTerm = '';
  selectedStatus: OrderStatus | string | null = '';

  statusOptions = [
    { value: null, label: 'All' },
    { value: OrderStatus.Pending, label: 'Pending' },
    { value: OrderStatus.Approved, label: 'Approved' },
    { value: OrderStatus.Completed, label: 'Completed' },
  ];

  constructor(private orderService: OrderService) {}

  ngOnInit() {
    this.loadOrders();
  }

  loadOrders() {
    this.loading = true;
    this.orderService.getOrders(this.selectedStatus?.toString()).subscribe({
      next: (orders) => {
        console.log('Orders loaded:', orders);
        this.orders = orders;
        this.filteredOrders = orders;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error loading orders. Please try again.';
        this.loading = false;
        console.error('Error loading orders:', err);
      },
    });
  }

  filterByStatus(status: OrderStatus | string | null) {
    this.selectedStatus = status;
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
