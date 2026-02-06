import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Order, OrderStatusUpdate } from '../../models/order';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, CurrencyPipe, DatePipe],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.css',
})
export class OrderDetailComponent implements OnInit {
  orderId!: string;
  order: Order | null = null;
  loading = false;
  error = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private orderService: OrderService,
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params) => {
      this.orderId = params['id'];
      this.loadOrder();
    });
  }

  loadOrder() {
    this.loading = true;
    this.orderService.getOrderById(this.orderId).subscribe({
      next: (order) => {
        this.order = order;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error loading order details.';
        this.loading = false;
      },
    });
  }

  updateStatus() {
    if (!this.order) return;

    const statusUpdate: OrderStatusUpdate = {
      status: this.order.status,
    };

    this.orderService.updateOrderStatus(this.orderId, statusUpdate).subscribe({
      next: () => {
        console.log('Status updated successfully.');
      },
      error: (err) => {
        alert('Error updating status.');
        this.loadOrder();
      },
    });
  }

  calculatePrice() {
    if (!this.order) return;

    this.loading = true;
    this.orderService.calculatePrice(this.orderId).subscribe({
      next: (updatedOrder) => {
        this.loading = false;
        this.loadOrder();
      },
      error: (err) => {
        this.loading = false;
        this.loadOrder();
        this.error = 'Error calculating price.';
      },
    });
  }
}
