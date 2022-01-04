import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  NgbActiveModal,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { first } from 'rxjs/operators';
import { MeasureUnit } from 'src/app/core/models/measure-unit.model';
import { IngredientsService } from 'src/app/core/services/ingredients.service';

@Component({
  selector: 'app-upsert-ingredient',
  templateUrl: './upsert-ingredient.component.html',
  styleUrls: ['./upsert-ingredient.component.css'],
})
export class UpsertIngredientComponent implements OnInit {
  @Input() ingredientDetails;
  ingredientForm: FormGroup;
  measureUnits = MeasureUnit;
  enumUnitKeys = [];
  closeResult: string;
  modalOptions: NgbModalOptions;

  constructor(
    private ingredientService: IngredientsService,
    private toastr: ToastrService,
    private modalService: NgbModal,
    public activeModal: NgbActiveModal
  ) {
    this.enumUnitKeys = Object.keys(this.measureUnits).filter(
      (f) => !isNaN(Number(f))
    );
  }

  ngOnInit() {
    this.initializeForm();

    if (this.ingredientDetails != undefined && this.ingredientDetails != null) {
      this.ingredientDetails.measureUnit =
        MeasureUnit[this.ingredientDetails.measureUnit].toString();
      this.ingredientForm.patchValue(this.ingredientDetails);
    }
  }

  initializeForm() {
    this.ingredientForm = new FormGroup({
      name: new FormControl('', Validators.required),
      unitPrice: new FormControl('', Validators.required),
      unitQuantity: new FormControl('', Validators.required),
      measureUnit: new FormControl('', Validators.required),
    });
  }

  get form() {
    return this.ingredientForm.controls;
  }

  closeModal(sendData) {
    this.activeModal.close(sendData);
  }

  onSubmit() {
    if (this.ingredientForm.invalid) {
      return;
    }

    if (this.ingredientDetails != undefined && this.ingredientDetails != null) {
      this.updateIngredient();
    } else {
      this.addNewIngredient();
    }
  }

  addNewIngredient() {
    this.ingredientService
      .save(this.ingredientForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.closeModal(data);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }

  updateIngredient() {
    this.ingredientService
      .update(this.ingredientDetails.id, this.ingredientForm.value)
      .pipe(first())
      .subscribe(
        (data) => {
          this.toastr.success('Data is successfully saved!', 'Success!');
          this.closeModal(data);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
  }
}
