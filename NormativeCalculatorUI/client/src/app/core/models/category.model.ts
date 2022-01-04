export class Category {
    public id: number;
    public categoryName: string;
    public createdAt: Date;
    public isDeleted: boolean;
  
    constructor(
      id: number,
      categoryName: string,
      createdAt: Date,
      isDeleted: boolean,
  
    ) {
      this.id = id;
      this.categoryName = categoryName;
      this.createdAt = createdAt;
      this.isDeleted = isDeleted;
    }
}
