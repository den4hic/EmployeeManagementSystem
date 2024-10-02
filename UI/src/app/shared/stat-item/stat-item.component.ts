import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-stat-item',
  templateUrl: './stat-item.component.html',
  styleUrl: './stat-item.component.css'
})
export class StatItemComponent {
  @Input({required: true}) label: string = "";
  @Input({required: true}) value: number = 0;
  @Input({required: true}) icon: string = "";

  constructor() {}
}
