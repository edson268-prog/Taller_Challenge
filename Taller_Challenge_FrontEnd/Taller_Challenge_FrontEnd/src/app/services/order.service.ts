import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order, OrderParams, OrderStatusUpdate } from '../models/order';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  createOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(this.apiUrl, order);
  }

  getOrders(queryParams: OrderParams): Observable<Order[]> {
    let params = new HttpParams();

    if (queryParams.status) params = params.set('status', queryParams.status);
    if (queryParams.page)
      params = params.set('page', queryParams.page.toString());
    if (queryParams.pageSize)
      params = params.set('pageSize', queryParams.pageSize.toString());
    if (queryParams.sortOrder)
      params = params.set('sortOrder', queryParams.sortOrder);

    return this.http.get<Order[]>(this.apiUrl, { params });
  }

  getOrderById(id: string): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/${id}`);
  }

  updateOrderStatus(
    id: string,
    statusUpdate: OrderStatusUpdate,
  ): Observable<any> {
    return this.http.patch(`${this.apiUrl}/${id}/status`, statusUpdate);
  }

  calculatePrice(id: string): Observable<Order> {
    return this.http.post<Order>(`${this.apiUrl}/${id}/price`, {});
  }
}
