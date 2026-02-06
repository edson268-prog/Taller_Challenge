import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Credentials } from '../../models/auth';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  credentials: Credentials = { username: '', password: '' };

  loading = false;

  onSubmit() {
    if (this.credentials.username && this.credentials.password) {
      this.loading = true;

      this.authService.login(this.credentials).subscribe({
        next: (response) => {
          this.loading = false;
          this.router.navigate(['/home']);
        },
        error: (err) => {
          this.loading = false;
          alert('Wrong credentials, please try again.');
        },
      });
    }
  }
}
