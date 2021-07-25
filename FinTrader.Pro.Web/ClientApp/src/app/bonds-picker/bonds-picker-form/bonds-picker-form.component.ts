import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BondsPickerFilter } from '../../models/bonds-picker-filter.model';
import { Repository } from '../../models/repository';

interface SelectItem {
  value: string;
  text: string;
}

@Component({
  selector: 'app-bonds-picker-form',
  templateUrl: './bonds-picker-form.component.html',
  styleUrls: ['./bonds-picker-form.component.css']
})
export class BondsPickerFormComponent implements OnInit {
  public bondsPicker: BondsPickerFilter;
  public bondClasses: SelectItem[] = [
    { value: "MostLiquid", text: "Самые ликвидные" },
    { value: "FarthestRepaynment", text: "По самому дальнему погашению" },
    { value: "MostProfitable", text: "Самые доходные" },
    { value: "ByRepaymentDate", text: "По дате погашения" },
  ];

  public calculationMethods: SelectItem[] = [
    { value: "InvestmentAmount", text: "По сумме инвестиций" },
    { value: "MonthlyCoupon", text: "По купону в месяц" },
  ];

  constructor(private router: Router, private repo: Repository) {
    if (this.repo.savedFilter == null) {
      this.bondsPicker = new BondsPickerFilter(false, false, "MostLiquid", null, false, "InvestmentAmount", 10000, false, false, false);
    } else {
      this.bondsPicker = this.repo.savedFilter;
    }
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    if (this.validateForm()) {
      this.repo.getBondSet(this.bondsPicker);
      this.router.navigate(["result"]);
    }
  }

  validateForm(): boolean {
    if (!(this.bondsPicker.isIncludedCorporate || this.bondsPicker.isIncludedFederal)) {
      alert("Нужно выбрать хотя бы один тип облигаций - ОФЗ или корпоративные.");
      return false;
    }

    return true;
  }

  onToggleIsIncludedFederal(): void {
    this.bondsPicker.isOneBondByIssuer = false;
  }

  formatLabel(value: number) {
    if (value >= 1000000) {
      return Math.round(value / 10000) / 100 + 'm';
    }
    else if (value >= 1000) {
      return Math.round(value / 1000) + 'k';
    }

    return value;
  }
}
