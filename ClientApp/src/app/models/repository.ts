import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BondsPickerFilter } from './bonds-picker-filter.model';
import { BondSet } from './bond-set.model';

@Injectable()
export class Repository {
  //filter: BondsPickerFilter;
  private bondSetData: BondSet;
  private bondsRequested: boolean = false;

  constructor(private http: HttpClient) {
    this.bondSetData = {
      bonds: [],
      coupons: []
    };
    this.bondsRequested = false;
  }

  getBondSet(filter: BondsPickerFilter) {
    this.http.post<BondSet>("/api/bondsets", filter)
      .subscribe(b => this.bondSetData = b);
    this.bondsRequested = true;
  }

  get bondSet(): BondSet {
    return this.bondSetData;
  }

  get formSent(): boolean {
    return this.bondsRequested;
  }
}
