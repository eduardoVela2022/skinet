import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

// Component
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})

// Component props
export class AppComponent {
  title = 'Skinet';
}
