import { HttpClient, HttpHeaders, HttpParams, HttpResponse, HttpResponseBase, HttpErrorResponse } from '@angular/common/http';

export class VitaUtil {
  public static getApiUri() {

   // return "http://chargeid-api-test.azurewebsites.net";

    if (window.location.href.indexOf("test") > -1) {
      return "https://chargeid-api-test.azurewebsites.net/";
    }

    if (location.hostname === "localhost" || location.hostname === "127.0.0.1")
      return "https://localhost:5001";

    //// prod
    return "https://chargeid-api-test.azurewebsites.net/";
  }

  public static getHeaders() {
    var headers = new HttpHeaders({
      "Content-Type": "application/json",
      "Accept": "application/json",
      "Access-Control-Allow-Origin": "*",
      "Access-Control-Allow-Methods": "GET, POST, OPTIONS, PUT, PATCH, DELETE",
      "Access-Control-Allow-Headers": "Origin,Accept, X-Requested-With, Content-Type, X-CSRF-Token, X-Requested-With, Accept, Accept-Version, Content-Length, Content-MD5, Date, X-Api-Version, X-File-Name",
    });

    return headers;
  }
}