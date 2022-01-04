import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RecipeCategoriesService {
  endpoint: string = 'RecipeCategories';

  constructor(private http: HttpClient) {}

  get(skip: any) {
    return this.http.get<any[]>(`${environment.apiUrl}` + this.endpoint, {
      params: new HttpParams().set('skip', skip),
      withCredentials: true,
    });
  }

  save(order: any) {
    return this.http.post(`${environment.apiUrl}${this.endpoint}`, order, {
      withCredentials: true,
    });
  }

  delete(id) {
    return this.http.delete(`${environment.apiUrl}${this.endpoint}/${id}`, {
      withCredentials: true,
    });
  }

  getById(id) {
    return this.http.get(`${environment.apiUrl}${this.endpoint}/${id}`, {
      withCredentials: true,
    });
  }

  update(id, order: any) {
    return this.http.put(`${environment.apiUrl}${this.endpoint}/${id}`, order, {
      withCredentials: true,
    });
  }
}
