import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {RouterLink, RouterModule, RouterOutlet} from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import {HttpClientModule, provideHttpClient, withInterceptors} from '@angular/common/http';
import {FormsModule} from "@angular/forms";

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AppComponent } from './app.component';
import {LandingComponent} from "./pages/landing/landing.component";
import {RegistrationComponent} from "./pages/registration/registration.component";
import {LoginComponent} from "./pages/login/login.component";
import {AppRoutingModule} from "./app-routing.module";
import { HomeComponent } from './pages/home/home.component';
import {ToolbarComponent} from "./shared/toolbar/app.toolbar";
import {MatMenu, MatMenuItem, MatMenuTrigger} from "@angular/material/menu";
import {MatIcon} from "@angular/material/icon";
import { ProfileComponent } from './pages/profile/profile.component';
import {tokenInterceptor} from "./interceptor/token.interceptor";
import { UserTableComponent } from './pages/user-table/user-table.component';
import {
  MatCell,
  MatCellDef,
  MatColumnDef,
  MatHeaderCell,
  MatHeaderCellDef,
  MatHeaderRow, MatHeaderRowDef, MatRow, MatRowDef, MatTable
} from "@angular/material/table";
import {MatSort} from "@angular/material/sort";
import {MatPaginator} from "@angular/material/paginator";
import { ConfirmDialogComponent } from './shared/confirm-dialog/confirm-dialog.component';
import {MatDialogActions, MatDialogClose, MatDialogContent, MatDialogTitle} from "@angular/material/dialog";
import { AssignRoleDialogComponent } from './shared/role-dialog/role-dialog.component';
import {MatOption, MatSelect} from "@angular/material/select";
import {MatCheckbox} from "@angular/material/checkbox";
import { UserStatisticsComponent } from './components/user-statistics/user-statistics.component';
import {MatSlideToggle} from "@angular/material/slide-toggle";
import { BlockedComponent } from './pages/blocked/blocked.component';
import { StatItemComponent } from './shared/stat-item/stat-item.component';

@NgModule({
  declarations: [
    AppComponent,
    LandingComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    ToolbarComponent,
    ProfileComponent,
    UserTableComponent,
    ConfirmDialogComponent,
    AssignRoleDialogComponent,
    UserStatisticsComponent,
    BlockedComponent,
    StatItemComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatFormFieldModule,
    RouterOutlet,
    RouterLink,
    AppRoutingModule,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger,
    MatIcon,
    MatColumnDef,
    MatHeaderCell,
    MatCell,
    MatHeaderCellDef,
    MatCellDef,
    MatHeaderRow,
    MatRow,
    MatTable,
    MatSort,
    MatPaginator,
    MatRowDef,
    MatHeaderRowDef,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    MatDialogTitle,
    MatSelect,
    MatOption,
    MatCheckbox,
    FormsModule,
    MatSlideToggle
  ],
  providers: [
    provideHttpClient(withInterceptors([tokenInterceptor]))
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
