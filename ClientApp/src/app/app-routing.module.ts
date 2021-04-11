import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BondsPickerFormComponent } from './bonds-picker/bonds-picker-form/bonds-picker-form.component';

const routes: Routes = [
  {path: 'form', component: BondsPickerFormComponent}
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
