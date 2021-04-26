import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';

import { Repository } from "../../models/repository";
import { BondSet } from "../../models/bond-set.model";
import { BondSelected } from "../../models/bond-selected.model";
import { CouponSelected } from "../../models/coupon-selected.model";

@Component({
  selector: 'app-bonds-picker-result',
  templateUrl: './bonds-picker-result.component.html',
  styleUrls: ['./bonds-picker-result.component.css']
})
export class BondsPickerResultComponent implements OnInit {
  displayedBondsColumns: string[] = ["bondName", "matDate", "couponValue", "amountToBye", "sum"];
  displayedCouponsColumns: string[] = ["cpnNum", "cpnDate", "cpnBondName", "cpnSum", "cpnComment"];

  constructor(private readonly router: Router, private readonly repo: Repository) {
  }

  ngOnInit(): void {
    if (!this.repo.formSent) {
      this.router.navigate(["form"]);
    }
  }

  get bondsSource(): BondSelected[] {
    return this.repo.bondSet.bonds;
  }

  get couponsSource(): CouponSelected[] {
    return this.repo.bondSet.coupons;
  }
}
