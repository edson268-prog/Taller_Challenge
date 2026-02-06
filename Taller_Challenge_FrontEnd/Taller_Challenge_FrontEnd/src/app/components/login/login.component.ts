import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Credentials } from '../../models/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loading = false;
  credentials: Credentials = {
    username: '',
    password: '',
  };

  constructor(private router: Router) {}

  onSubmit() {
    this.loading = true;

    setTimeout(() => {
      this.loading = false;
      localStorage.setItem('isLoggedIn', 'true');
      localStorage.setItem('username', this.credentials.username || 'Admin');
      this.router.navigate(['/home']);
    }, 800);
  }
}
