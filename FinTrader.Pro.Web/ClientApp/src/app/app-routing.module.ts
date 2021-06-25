import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BondsPickerFormComponent } from './bonds-picker/bonds-picker-form/bonds-picker-form.component';
import { BondsPickerResultComponent } from './bonds-picker/bonds-picker-result/bonds-picker-result.component';

const routes: Routes = [
  {path: 'form', component: BondsPickerFormComponent},
  {path: 'result', component: BondsPickerResultComponent}
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
