import { Component, inject } from '@angular/core';
import { ApiService } from '../services/api.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { HotToastService } from '@ngneat/hot-toast';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private apiService: ApiService, private http: HttpClient, private router: Router, private toast: HotToastService) {

  }
  private toastService = inject(HotToastService);
  headerText: string = "Enter credential to login";

  formData = {
    Mobile: "",
    Password: "",
    
  }

  // Toastcatch(){
  // this.toastService.success("User Login Successfully");
  // }

  // isLoginSuccess: boolean = false;

  // FetchFormData() {
  //   console.log(this.formData);
  //   this.apiService.InitiatePostRequest("License/login", this.formData).subscribe(res => {
  //     console.log(res);

  //     if (res == true) {
  //       this.router.navigate(["/dashboard"]);
  //     }
  //     else {
  //       this.router.navigate(["login"]);
  //     }
  //   })
  // (`User/DeleteUser/${userid}`)

  // FetchFormData() {
  //   console.log(this.formData);
  //   this.apiService.InitiatePostRequest("User/LoginUser",this.formData).subscribe((res:any) => {
  //       console.log(res);
  //       this.toastService.success("User Login Successfully");
  //       console.error();
  //       if (res.success == true) {
  //         this.router.navigate(['/dashboard'])
  //         this.toastService.success("User Login Successfully");
  //       } else {
  //         this.router.navigate(['/login'])
  //         this.toastService.error("User login error");
  //       }
  //     },
  //     (error) => {
  //       this.toastService.error("User login error");
  //       console.error('API Error:', error);
  //     }
  //   );
  // }

  //   this.http.post("https://localhost:44374/api/License/login", this.formData).subscribe(res =>{
  //     console.log(res);  // Without apiService - directly with httpClient    
  //   })


  LoginUser() {
    // this.formData.Mobile='0567890987';
    // this.formData.Password = '23456dfgh';
    // console.log(this.formData);

    // this.http.post('https://localhost:44374/api/User/LoginUser', this.formData).subscribe((res: any)=>
    // {console.log(res)})
    this.apiService.InitiatePostRequest("User/LoginUser", this.formData).subscribe(
      (res: any) => {
        console.log(res);
        if (res.success == true) {
          this.router.navigate(['/dashboard'])
          this.toastService.success(res.message);
        }
        else {
          this.router.navigate(['/login'])
          this.toastService.error(res.message);
        }

      },
      (error) => {
        (`console.error("Login failed", error);`)

      }
    );
  }

}

