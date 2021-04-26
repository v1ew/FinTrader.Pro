export class CouponSelected {
  constructor(
    public position?: number,
    public shortName?: string,
    public isin?: string,
    public date?: Date,
    public value?: number,
    public comment?: string
  ) {}
}
