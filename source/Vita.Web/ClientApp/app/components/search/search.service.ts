
import { Injectable } from "@angular/core";
//import { HttpClientModule } from '@angular/common/http';

import "rxjs/add/operator/toPromise";
import { Observable } from "rxjs";

import { ChargeClient, SearchResponse } from "../../vita.api";
import { Charge } from "../../vita.api";

//Renamed based on John Papa Heroes

@Injectable()
export class SearchService {

  //constructor(private http: HttpClient) { } //, private api: VitaApi
  constructor(private api: ChargeClient) {}

  //searchNew(keyword: string): Promise<FileResponse> {

  //    const url = `${this.searchUrl}/${keyword}`;
  //    return this.api.search(url);

  //}

  search(keyword: string):  Observable<SearchResponse[] | null> {
    console.info('searching for ' + keyword);
    return this.api.classify(keyword);

  }


  //searchOld(keyword: string): Observable<Array<SearchResponse>> {

  //    const url = `${this.searchUrl}/${keyword}`;
  //    return this.http
  //        .get<Array<SearchResponse>>(url);


  //    // return this.http.get(URL)
  //    //     .toPromise()
  //    //     .then(response => response.json().data as myClass[])
  //    //     .catch(this.handleError);

  //    //return this.http
  //    //    .get(url)
  //    //    .toPromise()
  //    //    .then((response) => {
  //    //        return response.json() as Array<SearchResponse>; //response.json().data
  //    //    })
  //    //    .catch(error => this.handleError(error));
  //}


  private handleError(error: any): Observable<any> {
    console.error("An error occurred", error);
    return Observable.throw(error);
    //return Promise.reject(error.message || error);
  }
}
