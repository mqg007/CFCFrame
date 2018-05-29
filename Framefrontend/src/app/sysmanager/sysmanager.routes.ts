import {SysmanagerEntryComponent} from "./sysmanager.entry.component";
import {SysmanagerGroupComponent} from "./sysmanager.group.component";
import { SysmanagerPageComponent } from "./sysmanager.page.component";
import { SysmanagerUserComponent } from "./sysmanager.user.component";
import { SysmanagerPriviliegeComponent } from "./sysmanager.priviliege.component";

import { SysmanagerResetpwdComponent } from "./sysmanager.resetpwd.component";
import { SysmanagerModifypwdComponent } from "./sysmanager.modifypwd.component";
import { SysmanagerQuerylogComponent } from "./sysmanager.querylog.component";
import { SysmanagerServicehelpComponent } from "./sysmanager.servicehelp.component";
import { SysmanagerComdictComponent } from "./sysmanager.comdict.component";
import { SysmanagerBizdictComponent } from "./sysmanager.bizdict.component";

export const sysmanagerRoutes = [
  {
    path: '',
    component: SysmanagerEntryComponent,
    children: [
      {
        path: '', redirectTo: '800001', pathMatch: 'full'
      },
      {
        path: '800001',
        children: [
          {
            path: '', redirectTo: '800001001', pathMatch: 'full'
          },
          {
            path: '800001001',
            component: SysmanagerGroupComponent
          },
          {
            path: '800001005',
            component: SysmanagerUserComponent
          },
          {
            path: '800001010',
            component: SysmanagerPageComponent
          },
          {
            path: '800001015',
            component: SysmanagerPriviliegeComponent
          },
          {
            path: '800001020',
            component: SysmanagerResetpwdComponent
          },
          {
            path: '800001025',
            component: SysmanagerModifypwdComponent
          }
        ]
      },
      {
        path: '800010',
          children: [
          {
            path: '', redirectTo: '800010001', pathMatch: 'full'
          },
          {
            path: '800010001',
            component: SysmanagerQuerylogComponent
          }
        ] 
      },
      {
        path: '800015',
          children: [
          {
            path: '', redirectTo: '800015001', pathMatch: 'full'
          },
          {
            path: '800015001',
            component: SysmanagerServicehelpComponent
          }
        ] 
      },
      {
        path: '800020',
          children: [
          {
            path: '', redirectTo: '800020001', pathMatch: 'full'
          },
          {
            path: '800020001',
            component: SysmanagerComdictComponent
          },
          {
            path: '800020005',
            component: SysmanagerBizdictComponent
          }
        ] 
      }
    ]
  }
];
