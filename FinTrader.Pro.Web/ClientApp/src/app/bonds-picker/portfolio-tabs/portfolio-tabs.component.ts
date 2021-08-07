import { Component, OnInit, Input } from '@angular/core';
import { Portfolio } from "../../models/portfolio.model";
import { BondSelected } from "../../models/bond-selected.model";
import { CouponSelected } from "../../models/coupon-selected.model";

@Component({
  selector: 'app-portfolio-tabs',
  templateUrl: './portfolio-tabs.component.html',
  styleUrls: ['./portfolio-tabs.component.css']
})
export class PortfolioTabsComponent implements OnInit {
  displayedBondsColumns: string[] = ["bondName", "matDate", "couponValue", "amountToBye", "sum"];
  displayedCouponsColumns: string[] = ["cpnNum", "cpnDate", "cpnBondName", "cpnSum", "cpnComment"];

  @Input() portfolio: Portfolio;
  @Input() order: number = 0;

  constructor() { }

  ngOnInit(): void {
  }

  get bondsSource(): BondSelected[] {
    return this.portfolio?.bondSets[0].bonds;
  }

  get couponsSource(): CouponSelected[] {
    return this.portfolio?.bondSets[0].coupons;
  }
}
