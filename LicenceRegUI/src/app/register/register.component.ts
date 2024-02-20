import { Component, inject } from '@angular/core';
import { ApiService } from "../services/api.service";
import { HotToastService } from '@ngneat/hot-toast';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

  constructor(private apiService: ApiService, private toast: HotToastService) {
    
  }

  private toastService = inject(HotToastService);

  headerText: string = "Create an Account";
  Name: string = "";
  Mobile: number = 0;
  Email: string = "";
  Password: any = "";

  formData = {
    Name: '',
    Email: '',
    Mobile: 0,
    Password: '',
  };

  fetchFormData() {
    console.log(this.formData);
    this.apiService.InitiatePostRequest("User/UserRegistration", this.formData).subscribe(res => {
      console.log(res);
      this.toastService.success("User Created Successfully");
       console.error();
       
    } ,(error)=>{
      this.toastService.error("User Created Unsuccessfully");
    });
  }

}




