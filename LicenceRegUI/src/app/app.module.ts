import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http' ;
import { HotToastModule, provideHotToastConfig } from '@ngneat/hot-toast';
import { NgxPaginationModule } from 'ngx-pagination';
import { SearchFilterPipe } from './dashboard/search-filter-pipe';
 

// import { ReactiveFormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    DashboardComponent,
    SearchFilterPipe

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    NgxPaginationModule,
    
    // HotToastModule.forRoot({position:"top-right"})
   // ReactiveFormsModule
  
  ],
  providers: [
    provideHotToastConfig()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
