import { Component, OnInit, ViewChild } from '@angular/core';
import { DataTablesResponse } from 'src/app/core/models/data-tables-response.model';
import { Ingredient } from 'src/app/core/models/ingredient.model';
import { IngredientsService } from 'src/app/core/services/ingredients.service';
import { DataTableDirective } from "angular-datatables"; 
import { Subject, Subscription } from 'rxjs';
import { Filter } from 'src/app/core/models/filter.model';
import { MeasureUnit } from 'src/app/core/models/measure-unit.model';
import { NgxBootstrapConfirmService } from 'ngx-bootstrap-confirm';
import { ToastrService } from 'ngx-toastr';
import { first } from 'rxjs/operators';
import { UpsertIngredientComponent } from './upsert-ingredients/upsert-ingredient.component';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-ingredients',
  templateUrl: './ingredients.component.html',
  styleUrls: ['./ingredients.component.css'],
})
export class IngredientsComponent implements OnInit {
  ingredients: Ingredient[] = [];
  dtOptions: DataTables.Settings = {};
  nameFilter: string;
  measureFilter: string;
  unitQuantityMin: string;
  unitQuantityMax: string;
  filters: Filter[]=[];
  @ViewChild(DataTableDirective)    
  dtElement: DataTableDirective;
  dtTrigger: Subject<any> = new Subject();   
  timerSubscription: Subscription;   
  measureUnits = MeasureUnit;
  measureEnumKeys = [];

  constructor(
    private ingredientsService: IngredientsService,
    private ngxBootstrapConfirmService: NgxBootstrapConfirmService,
    private toastr: ToastrService,
    private modalService: NgbModal,
  ) {
    this.measureEnumKeys = Object.keys(this.measureUnits).filter(
      (f) => !isNaN(Number(f))
    );
  }

  ngOnInit() {

    let lastPage = 0;
    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      displayStart: lastPage, // Last Selected Page
      serverSide: true,
      searching: false, 
      ajax: (dataTablesParameters: any, callback) => {
        lastPage = dataTablesParameters.start; // Note :  dataTablesParameters.start = page count * table length 
        dataTablesParameters.filter = this.prepareFilters();
        this.ingredientsService
          .getIngredientsForTable(dataTablesParameters)
          .then((resp: DataTablesResponse) => {
            this.ingredients = resp.data;
            callback({
              recordsTotal: resp.recordsTotal,
              recordsFiltered: resp.recordsFiltered,
              data: [],
            });
          });
      },
      autoWidth: false,
      ordering: true,
      lengthMenu: ['10', '15', '25', '50', '100'],
      columns: [
        { data: 'Name', searchable: true },
        { data: 'UnitPrice' },
        { data: 'UnitQuantity' },
        { data: 'MeasureUnit' },
        { data: 'CostIngredient' },
      ],
    };
  }

  prepareFilters(){
    this.filters=[];

    if(this.nameFilter != null && this.nameFilter != ""){
      this.filters.push({property:"Name", value:this.nameFilter});
    }

    if(this.measureFilter != null && this.measureFilter != ""){
      this.filters.push({property:"MeasureUnit", value:this.measureFilter});
    }

    if(this.unitQuantityMin != null && this.unitQuantityMin != ""){
      this.filters.push({property:"UnitQuantityMin", value: this.unitQuantityMin.toString()});
    }

    if(this.unitQuantityMax != null && this.unitQuantityMax != ""){
      this.filters.push({property:"UnitQuantityMax", value: this.unitQuantityMax.toString()});
    }
    
    return this.filters;
  }

  ngAfterViewInit(): void {    
    this.dtTrigger.next();        
  }  

  rerender(): void {       
    this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {    
      dtInstance.destroy();    
      this.dtTrigger.next();    
    });    
  }

  search() {   
    this.rerender();    
  }  

  ngOnDestroy(): void {    
    this.dtTrigger.unsubscribe();    
       
    if (this.timerSubscription) {    
      this.timerSubscription.unsubscribe();    
    }    
  } 

  private refreshData(): void {    
    this.rerender();    
    this.subscribeToData();        
  }   

  private subscribeToData(): void {    
    this.refreshData();    
  } 

  delete(id){
    let options ={
      title: 'Are you sure you want to delete?',
      confirmLabel: 'Delete',
      declineLabel: 'Cancel',
    }
    this.ngxBootstrapConfirmService.confirm(options).then((res: boolean) => {
      if(res){
        this.ingredientsService
        .delete(id)
        .pipe(first())
        .subscribe((response: any) => {
          this.toastr.success('Data is successfully deleted!', 'Success!');
          this.ingredients = this.ingredients.filter(item=> item.id != response.id);
        },
        (error) => {
          this.toastr.error('Something went wrong', 'Error!');
        }
      );
      }
    });
  }

  open(ingredient) {
    const modalRef = this.modalService.open(UpsertIngredientComponent, {
      scrollable: true,
    });
    modalRef.componentInstance.ingredientDetails = ingredient;
    modalRef.result.then(
      (result) => {
        this.rerender();
      },
      (reason) => {}
    );
  }
}
