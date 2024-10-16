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
import {apiUrlInterceptor} from "./interceptor/api-url.interceptor";
import { CreateProjectDialogComponent } from './shared/create-project-dialog/create-project-dialog.component';
import {
  MatDatepicker,
  MatDatepickerInput,
  MatDatepickerToggle,
  MatDateRangeInput,
  MatDateRangePicker
} from "@angular/material/datepicker";
import {MatStep, MatStepLabel, MatStepper, MatStepperNext, MatStepperPrevious} from "@angular/material/stepper";
import {MatChip, MatChipInput, MatChipListbox, MatChipRemove} from "@angular/material/chips";
import { ProjectDashboardComponent } from './pages/project-dashboard/project-dashboard.component';
import {CdkDrag, CdkDropList} from "@angular/cdk/drag-drop";
import {MatProgressBar} from "@angular/material/progress-bar";
import { CreateTaskDialogComponent } from './shared/create-task-dialog/create-task-dialog.component';
import { ShowTaskDialogComponent } from './shared/show-task-dialog/show-task-dialog.component';
import { PhotoDialogComponent } from './shared/photo-dialog/photo-dialog.component';
import {MatTab, MatTabGroup} from "@angular/material/tabs";
import {MatList, MatListItem} from "@angular/material/list";
import { EditUserDialogComponent } from './shared/edit-user-dialog/edit-user-dialog.component';
import {
  MatAccordion,
  MatExpansionPanel, MatExpansionPanelDescription,
  MatExpansionPanelHeader,
  MatExpansionPanelTitle
} from "@angular/material/expansion";
import { OnlineUsersComponent } from './shared/online-users/online-users.component';
import { UserAvatarComponent } from './shared/user-avatar/user-avatar.component';
import { NotificationComponent } from './shared/notification/notification.component';
import {MatBadge} from "@angular/material/badge";
import {MatDivider} from "@angular/material/divider";
import { NotificationsComponent } from './pages/notifications/notifications.component';
import {MatLine} from "@angular/material/core";

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
    StatItemComponent,
    CreateProjectDialogComponent,
    ProjectDashboardComponent,
    CreateTaskDialogComponent,
    ShowTaskDialogComponent,
    PhotoDialogComponent,
    EditUserDialogComponent,
    OnlineUsersComponent,
    UserAvatarComponent,
    NotificationComponent,
    NotificationsComponent
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
    MatSlideToggle,
    MatDateRangeInput,
    MatDatepickerToggle,
    MatDateRangePicker,
    MatDatepicker,
    MatDatepickerInput,
    MatStepper,
    MatStep,
    MatStepLabel,
    MatChipInput,
    MatChip,
    MatChipRemove,
    MatStepperPrevious,
    MatChipListbox,
    MatStepperNext,
    CdkDropList,
    CdkDrag,
    MatProgressBar,
    MatTabGroup,
    MatTab,
    MatList,
    MatListItem,
    MatAccordion,
    MatExpansionPanel,
    MatExpansionPanelHeader,
    MatExpansionPanelTitle,
    MatExpansionPanelDescription,
    MatBadge,
    MatDivider,
    MatLine
  ],
  providers: [
    provideHttpClient(withInterceptors([apiUrlInterceptor, tokenInterceptor]))
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
