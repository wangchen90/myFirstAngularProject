import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Router } from '@angular/router';
import { HotToastService } from '@ngneat/hot-toast';
import { NgxPaginationModule } from 'ngx-pagination';
import { Pipe, PipeTransform } from '@angular/core';
import Swal from 'sweetalert2';

declare var $: any;


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']



})

export class DashboardComponent {

  // searchText = '';
  // items = [
  //   {
  //     "user_id": 92,
  //     "name": "sdfgh",
  //     "email": "sdfgh@123",
  //     "mobile": "1234565432",
  //     "password": "e8009424f0e9e97be7ef2c79a77a24e0d060837a"
  //   },
  //   {
  //     "user_id": 93,
  //     "name": "asdfg",
  //     "email": "sdfghj2123",
  //     "mobile": "12345678",
  //     "password": "cc268918c7af9aaafc9dcadb82eecbd56486486a"
  //   },
  //   {
  //     "user_id": 94,
  //     "name": "yuio",
  //     "email": "asdfg@12",
  //     "mobile": "0347732123",
  //     "password": "a152e841783914146e4bcd4f39100686"
  //   },
  //   {
  //     "user_id": 95,
  //     "name": "test",
  //     "email": "cghvgh",
  //     "mobile": "8797",
  //     "password": "hbjbj"
  //   },
  //   {
  //     "user_id": 96,
  //     "name": "ft",
  //     "email": "vhbjn",
  //     "mobile": "677879",
  //     "password": "ghk"
  //   },
  //   {
  //     "user_id": 97,
  //     "name": "lako",
  //     "email": "string",
  //     "mobile": "1835649254",
  //     "password": "pofghj"
  //   },
  //   {
  //     "user_id": 99,
  //     "name": "strin",
  //     "email": "string",
  //     "mobile": "2500806347",
  //     "password": "string"
  //   },
  //   {
  //     "user_id": 100,
  //     "name": "sdfgh",
  //     "email": "sdfghj@123",
  //     "mobile": "0234567890",
  //     "password": "c91e465ca781304b3114a85fe8f4944e57d81c57"
  //   },
  //   {
  //     "user_id": 101,
  //     "name": "sdfgh",
  //     "email": "sdfghj@123",
  //     "mobile": "0234567890",
  //     "password": "c91e465ca781304b3114a85fe8f4944e57d81c57"
  //   },
  //   {
  //     "user_id": 102,
  //     "name": "sdfgh",
  //     "email": "sdfghj@123",
  //     "mobile": "0234567890",
  //     "password": "c91e465ca781304b3114a85fe8f4944e57d81c57"
  //   },
  //   {
  //     "user_id": 103,
  //     "name": "lako",
  //     "email": "lako@123",
  //     "mobile": "1234567890",
  //     "password": "asdfg"
  //   },
  //   {
  //     "user_id": 104,
  //     "name": "lako",
  //     "email": "lako@123",
  //     "mobile": "1234567890",
  //     "password": "f1b699cc9af3eeb98e5de244ca7802ae38e77bae"
  //   },
  //   {
  //     "user_id": 105,
  //     "name": "lako",
  //     "email": "lako@123",
  //     "mobile": "1234567890",
  //     "password": "f1b699cc9af3eeb98e5de244ca7802ae38e77bae"
  //   },
  //   {
  //     "user_id": 106,
  //     "name": "das",
  //     "email": "das@123",
  //     "mobile": "5432165432",
  //     "password": "03e78078a133db82281053ac566a8344df926c3b"
  //   },
  //   {
  //     "user_id": 107,
  //     "name": "can",
  //     "email": "can@123",
  //     "mobile": "7654321246",
  //     "password": "6769db84adc092afdd64a08e74a3d0e78d790044"
  //   },
  //   {
  //     "user_id": 108,
  //     "name": "string",
  //     "email": "string",
  //     "mobile": "0827427380",
  //     "password": "ecb252044b5ea0f679ee78ec1a12904739e2904d"
  //   }
  // ];
  searchText: string = '';
  items: any[] = [];
  totalRows: number = 0;
  pagination: any;




  constructor(private apiService: ApiService, private toast: HotToastService) {

  }

  ngOnInit() {
    this.FetchFormData();
    console.log(this.formData);

  }

  //pagination
  itemsPerPage: number = 5;
  currentPage: number = 1;
  totalPages!: number;



  userData: any = [];

  formData: any = {
    user_id: "",
    name: "",
    email: "",
    mobile: "",
    updated_on: "",
  }

  FetchFormData() {
    // this.userData = [];
    this.apiService.InitiateGetRequest("User/GetAllUser").subscribe((res) => {
      // console.log(res);
      if (res) {
        console.log(res);

        this.userData = res;
        console.log("userData", this.userData);
      }
    });
  }

  GetEdit(user: any) {
    this.formData = {
      user_id: user.user_id,
      name: user.name,
      email: user.email,
      mobile: user.mobile,
    }
  }

  UpdateUserData() {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Update"
    }).then((res: any) => {
      if (res.isConfirmed) {
        console.log(this.formData);
        this.apiService.initiatePutRequest("User/UpdateAllUser", this.formData).subscribe((res) => {
          console.log(res);
          if (res) {
            this.toast.success("User Updated Successfully");
            this.FetchFormData();
            this.resetFrom();
          }
        });
      }
    })
  }

  DeleteUserData(userid: number, index: number) {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      iconColor: '#d33',
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, Delete"
    }).then((res: any) => {
      if (res.isConfirmed) {
        // debugger
        console.log(userid);
        this.apiService.initiateDeleteRequest(`User/DeleteUser/${userid}`).subscribe((res) => {
          console.log(res);
          if (res) {
            this.FetchFormData();
            console.log(this.userData);
            console.log(index);
            this.userData.splice(index, 1);
            this.toast.success("User Deleted Successfully");
            if (this.userData.length % this.itemsPerPage === 0 && this.currentPage > 1) {
              this.currentPage = this.currentPage - 1;
            }
          }
        }, (error) => {
          this.toast.error("User Deleted Unsuccessfully");
        });
      }
    })

  }

  resetFrom() {
    this.formData = {
      user_id: "",
      name: "",
      email: "",
      mobile: "",
    }
    $('.closeModal').click();

  }



}
