import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LandingComponent} from "./pages/landing/landing.component";
import {RegistrationComponent} from "./pages/registration/registration.component";
import {LoginComponent} from "./pages/login/login.component";
import {HomeComponent} from "./pages/home/home.component";
import {authGuard} from "./guards/auth.guard";
import {ProfileComponent} from "./pages/profile/profile.component";
import {UserTableComponent} from "./pages/user-table/user-table.component";
import {roleGuard} from "./guards/role.guard";
import {BlockedComponent} from "./pages/blocked/blocked.component";
import {blockGuard} from "./guards/block.guard";
import {ProjectTableComponent} from "./pages/project-table/project-table.component";

const routes: Routes = [
  {
    path: 'landing',
    component: LandingComponent
  },
  {
    path: 'register',
    component: RegistrationComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    component: HomeComponent,
    canActivate: [authGuard]
  },
  {
    path: 'users',
    component: UserTableComponent,
    canActivate: [roleGuard],
    data: {
      roles: ['Admin']
    }
  },
  {
    path: 'blocked',
    component: BlockedComponent,
    canActivate: [blockGuard]
  },
  {
    path: 'projects',
    component: ProjectTableComponent,
    canActivate: [roleGuard],
    data: {
      roles: ['Admin', 'Manager', 'Employee']
    }
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
