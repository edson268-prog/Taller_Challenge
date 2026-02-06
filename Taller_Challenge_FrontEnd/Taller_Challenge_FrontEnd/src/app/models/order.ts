export interface OrderItem {
  id?: string;
  description: string;
  quantity: number;
  unitPrice: number;
  totalPrice?: number;
}

export interface Order {
  id?: string;
  customerName: string;
  vehiclePlate: string;
  status: OrderStatus;
  items: OrderItem[];
  createdAt?: Date;
  subtotal?: number;
  taxAmount?: number;
  discountAmount?: number;
  totalAmount?: number;
}

export enum OrderStatus {
  Pending = 1,
  Approved = 2,
  Completed = 3,
}

export interface OrderStatusUpdate {
  status: OrderStatus;
}

export interface OrderParams {
  status?: OrderStatus | string | null;
  page?: number;
  pageSize?: number;
  sortOrder?: string;
}
