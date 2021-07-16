import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-circle-link',
  templateUrl: './circle-link.component.html',
  styleUrls: ['./circle-link.component.css']
})
export class CircleLinkComponent implements OnInit {

  @Input()
  link: string = "";

  @Input()
  title: string = "";

  constructor() { }

  ngOnInit(): void {
  }
}
