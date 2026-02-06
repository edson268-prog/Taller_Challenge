import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order';

@Component({
  selector: 'app-order-create',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  templateUrl: './order-create.component.html',
  styleUrl: './order-create.component.css',
})
export class OrderCreateComponent implements OnInit {
  orderForm: FormGroup;
  submitting = false;
  error = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private orderService: OrderService,
    private router: Router,
  ) {
    this.orderForm = this.createForm();
  }

  ngOnInit() {
    if (this.items.length === 0) {
      this.addItem();
    }
  }

  createForm(): FormGroup {
    return this.fb.group({
      customerName: ['', [Validators.required, Validators.minLength(2)]],
      vehiclePlate: ['', [Validators.required, Validators.minLength(3)]],
      status: ['Pending'],
      items: this.fb.array([]),
    });
  }

  get customerName() {
    return this.orderForm.get('customerName')!;
  }

  get vehiclePlate() {
    return this.orderForm.get('vehiclePlate')!;
  }

  get items(): FormArray {
    return this.orderForm.get('items') as FormArray;
  }

  createItemFormGroup(): FormGroup {
    return this.fb.group({
      description: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0.01)]],
    });
  }

  addItem(): void {
    this.items.push(this.createItemFormGroup());
  }

  removeItem(index: number): void {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }

  calculateItemTotal(index: number): number {
    const item = this.items.at(index);
    const quantity = item.get('quantity')?.value || 0;
    const unitPrice = item.get('unitPrice')?.value || 0;
    return quantity * unitPrice;
  }

  updateItemTotal(index: number): void {
    const item = this.items.at(index);
    item.markAsTouched();
  }

  calculateSubtotal(): number {
    return this.items.controls.reduce((total, item, index) => {
      return total + this.calculateItemTotal(index);
    }, 0);
  }

  onSubmit(): void {
    if (this.orderForm.invalid) {
      this.markAllAsTouched();
      return;
    }

    this.submitting = true;
    this.error = '';
    this.successMessage = '';

    const orderData: Order = {
      ...this.orderForm.value,
      items: this.orderForm.value.items.map((item: any) => ({
        description: item.description,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
      })),
    };

    this.orderService.createOrder(orderData).subscribe({
      next: (createdOrder) => {
        this.successMessage = `Order created successfully! Order ID: ${createdOrder.id?.substring(0, 8)}`;

        this.orderForm.reset({
          customerName: '',
          vehiclePlate: '',
          status: 'Pending',
        });

        while (this.items.length !== 0) {
          this.items.removeAt(0);
        }
        this.addItem();

        this.submitting = false;

        setTimeout(() => {
          this.router.navigate(['/orders', createdOrder.id]);
        }, 2000);
      },
      error: (err) => {
        console.error('Error creating order:', err);

        let errorMessage = 'Error creating order. Please try again.';
        if (err.status === 400) {
          errorMessage = 'Invalid data. Please check all fields.';
        } else if (err.status === 500) {
          errorMessage = 'Server error. Please try again later.';
        }

        this.error = errorMessage;
        this.submitting = false;
      },
    });
  }

  private markAllAsTouched(): void {
    this.orderForm.markAllAsTouched();

    this.items.controls.forEach((item) => {
      item.markAllAsTouched();
    });
  }
}
