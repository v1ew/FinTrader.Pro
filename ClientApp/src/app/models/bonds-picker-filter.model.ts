export class BondsPickerFilter {
  constructor(
    public isIncludedFederal?: boolean,
    public isIncludedCorporate?: boolean,

    public bondsClass?: string,
    public repaymentDate?: Date,
    public strictlyUpToDate?: boolean,
    public calculationMethod?: string,
    public amount?: number,

    public isWithoutOffer?: boolean,
    public isOneBondByIssuer?: boolean,
    public isTwoPortfolios?: boolean
  ) { }
}
