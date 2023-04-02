import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BondsPickerFilter } from './bonds-picker-filter.model';
import { BondSet } from './bond-set.model';
import { Portfolio } from './portfolio.model';

@Injectable()
export class Repository {
  private prtfls: Portfolio[] = null;
  private portfolioRequested: boolean = false;
  private filterSent: BondsPickerFilter = null;

  constructor(private http: HttpClient) {
    this.portfolioRequested = false;
  }

  getBondSet(filter: BondsPickerFilter) {
    this.http.post<Portfolio[]>("https://ofz.fintrader.pro/api/bondsets", filter)
      .subscribe(p => this.prtfls = p);
    this.portfolioRequested = true;
    this.filterSent = filter;
  }

  get portfolios(): Portfolio[] {
    return this.prtfls;
  }

  get formSent(): boolean {
    return this.portfolioRequested;
  }

  resetRequested() {
    this.portfolioRequested = false;
  }

  get savedFilter(): BondsPickerFilter {
    return this.filterSent;
  }
}
