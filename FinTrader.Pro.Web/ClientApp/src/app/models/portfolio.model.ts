import { BondSet } from "./bond-set.model";

export class Portfolio {
  constructor(
    public includes?: string,
    public sum?: number,
    public pay?: number,
    public yields?: number,
    public matDate?: Date,
    public bondSets?: BondSet[],
    public isError?: boolean,
    public errorMessage?: string
  ) {
  }
}
