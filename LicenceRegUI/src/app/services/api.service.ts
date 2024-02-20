import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  // private apiUrl:string = 'https://localhost:44374/api/License/createLicense';
  private apiUrl:string = 'https://localhost:44374/api/';

  constructor(private http : HttpClient) { }


// postMethod(){
//   this.http.post(this.apiUrl, postObj:any)

// }
// return this.http.post(`${this.apiUrl}${url}`,postObj);
// }

InitiatePostRequest(url:string, postObj:any){
  console.log(url, postObj)
  return this.http.post(`${this.apiUrl}${url}`, postObj ,{headers: new HttpHeaders({"Content-Type":"application/json"})});
}

InitiateGetRequest(url:string){
return this.http.get(`${this.apiUrl}${url}`);
}

initiatePutRequest(url:string,postObj:string){
return this.http.put(`${this.apiUrl}${url}`,postObj);
}

initiateDeleteRequest(url:string){
  return this.http.delete(`${this.apiUrl}${url}`);
}



}
