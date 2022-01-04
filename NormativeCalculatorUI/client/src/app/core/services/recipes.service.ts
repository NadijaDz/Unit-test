import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RecipesService {
  endpoint: string = 'Recipes';

  constructor(private http: HttpClient) {}

  get(request: any) {
    return this.http.get<any[]>(`${environment.apiUrl}` + this.endpoint, {
      params: new HttpParams()
        .set('skip', request.skip)
        .set('categoryId', request.categoryId)
        .set('searchQuery', request.searchQuery),
      withCredentials: true, //cuva cookie
    });
  }

  save(order: any) {
    return this.http.post(`${environment.apiUrl}${this.endpoint}`, order, {
      withCredentials: true,
    });
  }

  getById(id: string) {
    return this.http.get(`${environment.apiUrl}${this.endpoint}/${id}`, {
      withCredentials: true,
    });
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
