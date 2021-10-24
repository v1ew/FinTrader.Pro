import { Component, OnInit } from '@angular/core';
import { Repository } from "../../models/repository";
import {Portfolio} from "../../models/portfolio.model";

@Component({
  selector: 'app-bonds-picker-layout',
  templateUrl: './bonds-picker-layout.component.html',
  styleUrls: ['./bonds-picker-layout.component.css']
})
export class BondsPickerLayoutComponent implements OnInit {

  constructor(private repo: Repository) { }

  ngOnInit(): void {
  }

  get formSent(): boolean {
    return this.repo.formSent;
  }

  backToForm() {
    this.repo.resetRequested();
  }
}
