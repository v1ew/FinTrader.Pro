import { Component } from '@angular/core';
import { Repository } from "./models/repository";
import {Portfolio} from "./models/portfolio.model";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'FinTraderPro';

  constructor(private readonly repo: Repository) { }

  get formSent(): boolean {
    return this.repo.formSent;
  }

}
