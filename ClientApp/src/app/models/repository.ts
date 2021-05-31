import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BondsPickerFilter } from './bonds-picker-filter.model';
import { BondSet } from './bond-set.model';
import { Portfolio } from './portfolio.model';

@Injectable()
export class Repository {
  //filter: BondsPickerFilter;
  private portfolio: Portfolio = null;
  private portfolioRequested: boolean = false;

  constructor(private http: HttpClient) {
    this.portfolioRequested = false;
  }

  getBondSet(filter: BondsPickerFilter) {
    this.http.post<Portfolio>("/api/bondsets", filter)
      .subscribe(p => this.portfolio = p);
    this.portfolioRequested = true;
  }

  get bondSet(): BondSet {
    return this.portfolio?.bondSets[0];
  }

  get formSent(): boolean {
    return this.portfolioRequested;
  }
}
