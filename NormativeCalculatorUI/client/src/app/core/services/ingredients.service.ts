import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { DataTablesResponse } from '../models/data-tables-response.model';

@Injectable({
  providedIn: 'root',
})
export class IngredientsService {
  endpoint: string = 'Ingredients';

  constructor(private http: HttpClient) {}

  get() {
    return this.http.get<any[]>(`${environment.apiUrl}` + this.endpoint, {
      withCredentials: true, //cuva cookie
    });
  }
  
  save(order: any) {
    return this.http.post(`${environment.apiUrl}${this.endpoint}`, order, {
      withCredentials: true, 
    });
  }

  getIngredientsForTable(tableQuery: Promise<DataTablesResponse>) {
    return this.http.post(`${environment.apiUrl}${this.endpoint + '/GetIngredientsForTable'}`, tableQuery, {
      withCredentials: true,
    }).pipe(retry(1)).toPromise();
  }

  delete(id) {
    return this.http.delete(`${environment.apiUrl}${this.endpoint}/${id}`, {
      withCredentials: true,
    });
  }
  
  update(id, order: any) {
    return this.http.put(`${environment.apiUrl}${this.endpoint}/${id}`, order, {
      withCredentials: true,
    });
  }
}
