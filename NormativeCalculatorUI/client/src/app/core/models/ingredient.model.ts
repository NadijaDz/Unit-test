import { MeasureUnit } from "./measure-unit.model";

export class Ingredient {
  public id: number;
  public name: string;
  public unitPrice: number;
  public unitQuantity: number;
  public measureUnit: MeasureUnit;
  public createdAt: Date;
  public isDeleted: boolean;
  public costIngredient: number;

  constructor(
    id: number,
    name: string,
    unitPrice: number,
    unitQuantity: number,
    createdAt: Date,
    measureUnit: MeasureUnit,
    isDeleted: boolean,
    costIngredient: number,
  ) {
    this.id = id;
    this.name = name;
    this.unitPrice = unitPrice;
    this.unitQuantity = unitQuantity;
    this.measureUnit = measureUnit;
    this.createdAt = createdAt;
    this.isDeleted = isDeleted;
    this.costIngredient = costIngredient;
  }
}
