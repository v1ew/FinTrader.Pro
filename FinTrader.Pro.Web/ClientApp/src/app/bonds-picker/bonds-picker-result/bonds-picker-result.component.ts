import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';

import { Repository } from "../../models/repository";
import {Portfolio} from "../../models/portfolio.model";

@Component({
  selector: 'app-bonds-picker-result',
  templateUrl: './bonds-picker-result.component.html',
  styleUrls: ['./bonds-picker-result.component.css']
})
export class BondsPickerResultComponent implements OnInit {

  constructor(private readonly router: Router, private readonly repo: Repository) {
  }

  // Если форму еще не отправляли, то переадресуем на форму
  ngOnInit(): void {
    if (!this.repo.formSent) {
      this.router.navigate(["form"]);
    }
  }

  get portfolios(): Portfolio[] {
    return this.repo.portfolios;
  }

}
