import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MatBottomSheetModule } from '@angular/material/bottom-sheet';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatNativeDateModule, MatRippleModule } from '@angular/material/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';

import { Repository } from '../models/repository';
import { BondsPickerFormComponent } from './bonds-picker-form/bonds-picker-form.component';
import { BondsPickerResultComponent } from './bonds-picker-result/bonds-picker-result.component';
import { PortfolioTabsComponent } from './portfolio-tabs/portfolio-tabs.component';
import { BondsPickerLayoutComponent } from './bonds-picker-layout/bonds-picker-layout.component';

@NgModule({
  declarations: [
    BondsPickerFormComponent,
    BondsPickerResultComponent,
    PortfolioTabsComponent,
    BondsPickerLayoutComponent,
  ],
  providers: [
    Repository
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatIconModule,
    MatInputModule,
    MatNativeDateModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatSliderModule,
    MatTableModule,
    MatTabsModule,
  ],
  exports: [
    BondsPickerLayoutComponent  ]
})
export class BondsPickerModule { }
