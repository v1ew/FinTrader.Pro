import { Component, OnInit } from '@angular/core';

import { Repository } from "../../models/repository";
import {Portfolio} from "../../models/portfolio.model";

@Component({
  selector: 'app-bonds-picker-result',
  templateUrl: './bonds-picker-result.component.html',
  styleUrls: ['./bonds-picker-result.component.css']
})
export class BondsPickerResultComponent implements OnInit {

  constructor(private readonly repo: Repository) { }

  ngOnInit(): void {
  }

  get portfolios(): Portfolio[] {
    return this.repo.portfolios;
  }

}
