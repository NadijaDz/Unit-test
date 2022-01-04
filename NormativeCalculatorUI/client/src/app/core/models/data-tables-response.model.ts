import { Filter } from "./filter.model";

export class DataTablesResponse {
    data: any[];
    draw: number;
    recordsFiltered: number;
    recordsTotal: number;
    filter: Filter[];
    
    constructor(
        data: any,
        draw: number,
        recordsFiltered: number,
        recordsTotal: number,
        filter: Filter[]
      ) {
        this.data = data;
        this.draw = draw;
        this.recordsFiltered = recordsFiltered;
        this.recordsTotal = recordsTotal;
        this.filter = filter;
      }
}
