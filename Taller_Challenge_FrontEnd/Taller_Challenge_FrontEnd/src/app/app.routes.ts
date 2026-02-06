import { Routes } from '@angular/router';
import { OrderListComponent } from './components/order-list/order-list.component';
import { OrderDetailComponent } from './components/order-detail/order-detail.component';
import { LoginComponent } from './components/login/login.component';
import { LayoutComponent } from './components/layout/layout.component';
import { HomeComponent } from './components/home/home.component';
import { OrderCreateComponent } from './components/order-create/order-create.component';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'orders', component: OrderListComponent },
      { path: 'orders/new', component: OrderCreateComponent },
      { path: 'orders/:id', component: OrderDetailComponent },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
    ],
  },
  { path: '**', redirectTo: '' },
];
