import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { DashboardComponent } from './dashboard/dashboard.component';

const routes: Routes = [
  { path:"", component:LoginComponent, pathMatch:"full" },
  { path:"register", component:RegisterComponent, pathMatch:"full" },
  { path:"login", component:LoginComponent, pathMatch:"full" },
  { path:"dashboard", component:DashboardComponent, pathMatch:"full" },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
