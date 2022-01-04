import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-recipe-detail',
  templateUrl: './recipe-detail.component.html',
  styleUrls: ['./recipe-detail.component.css'],
})
export class RecipeDetailComponent implements OnInit {
  @Input() recipeDetails;
  constructor(public activeModal: NgbActiveModal) {}

  ngOnInit(): void {}

  closeModal(sendData) {
    this.activeModal.close(sendData);
  }
}
