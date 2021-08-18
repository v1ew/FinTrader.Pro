import { Component, OnInit } from '@angular/core';
import {MatBottomSheet, MatBottomSheetRef} from '@angular/material/bottom-sheet';

@Component({
  selector: 'app-disclaimer-sheet',
  templateUrl: './disclaimer-sheet.component.html',
  styleUrls: ['./disclaimer-sheet.component.css']
})
export class DisclaimerSheetComponent implements OnInit {

  constructor(private bottomSheetRef: MatBottomSheetRef<DisclaimerSheetComponent>) { }

  ngOnInit(): void {
  }

  closeDisclaimer(): void {
    this.bottomSheetRef.dismiss();
  }
}
