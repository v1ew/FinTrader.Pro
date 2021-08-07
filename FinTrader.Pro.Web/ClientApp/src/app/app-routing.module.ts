import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BondsPickerFormComponent } from './bonds-picker/bonds-picker-form/bonds-picker-form.component';
import { BondsPickerResultComponent } from './bonds-picker/bonds-picker-result/bonds-picker-result.component';
import { FaqPageComponent } from './pages/faq-page/faq-page.component';
import { MainLayoutComponent } from './shared/main-layout/main-layout.component';

const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children: [
      {path: '', redirectTo: 'form', pathMatch: 'full'},
      {path: 'form', component: BondsPickerFormComponent},
      {path: 'result', component: BondsPickerResultComponent},
      {path: 'faq', component: FaqPageComponent}
    ]
  }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
