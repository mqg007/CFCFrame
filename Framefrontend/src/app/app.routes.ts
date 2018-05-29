import {LoginComponent} from './login/login.component';
import {QuitLoginComponent} from './login/quitlogin.component';

export const appRoutes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'workbench',
    loadChildren: './workbench/workbench.module#WorkbenchModule'
  },
  {
    path: 'quitsystem',
    component: QuitLoginComponent
  },
  {
    path: '**',
    component: LoginComponent
  }
];
