import { Component, OnInit } from "@angular/core";
import { Repository } from "../../models/repository";
import { BondSet } from "../../models/bond-set.model";
import { BondSelected } from "../../models/bond-selected.model";

@Component({
  selector: 'app-bonds-picker-result',
  templateUrl: './bonds-picker-result.component.html',
  styleUrls: ['./bonds-picker-result.component.css']
})
export class BondsPickerResultComponent implements OnInit {
  displayedColumns: string[] = ["shortName", "matDate", "couponValue", "amountToBye", "sum"];

  constructor(private readonly  repo: Repository) {
  }

  ngOnInit(): void {
  }

  get dataSource(): BondSelected[] {
    return this.repo.bondSet.bonds;
  }
}
