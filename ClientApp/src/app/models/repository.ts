import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BondsPickerFilter } from './bonds-picker-filter.model';
import { BondSet } from './bond-set.model';

@Injectable()
export class Repository {
  //filter: BondsPickerFilter;
  bondSetData: BondSet;

  constructor(private http: HttpClient) {
    this.bondSetData = {
      bonds: []
    };
  }

  getBondSet(filter: BondsPickerFilter) {
    this.http.post<BondSet>("/api/bondsets", filter)
      .subscribe(b => this.bondSetData = b);
  }

  get bondSet(): BondSet {
    return this.bondSetData;
  }
}
