import {WorkbenchComponent} from './workbench.component';
import {PageNotFoundComponent} from '../not-found.component';
import { WorkbenchNoPrevilegeComponent } from "./workbench.noprevilege.component";

export const workbenchRoutes = [
  {
    path: '',
    component: WorkbenchComponent,
    children: [
      {
        path: '', redirectTo: '000', pathMatch: 'full'
      },
      {
        path: '000', component:WorkbenchNoPrevilegeComponent
      },
      {
        path: '001',
        loadChildren: '../bizUIDemo/bizUIDemo.module#BizUIDemoModule',
        data: {preload: true}
      },
      {
        path: '800',
        loadChildren: '../sysmanager/sysmanager.module#SysmanagerModule',
        data: {preload: true}
      },
      {
        path: '**',
        component:PageNotFoundComponent
      }
    ]
  }
];
