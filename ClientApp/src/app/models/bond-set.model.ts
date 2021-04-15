import { BondSelected } from "./bond-selected.model";
import { CouponSelected } from "./coupon-selected.model";

export class BondSet {
  constructor(
    public bonds?: BondSelected[],
    public coupons?: CouponSelected[]
  ) { }
}
