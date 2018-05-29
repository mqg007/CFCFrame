import { BizUIDemoComponent } from './bizUIDemo.component';
import {BizCustomerUIDemoCardComponent} from './bizCustomerUIDemo.component';
import {BizInGoodsUIDemoComponent} from './bizInGoodsUIDemo.component';
import {BizOutGoodsUIDemoComponent} from './bizOutGoodsUIDemo.component';

export const BizUIDemoRoutes = [
  {
    path: '',
    component: BizUIDemoComponent,
    children: [
      {
        path: '', redirectTo: '001001', pathMatch: 'full'
      },
      {
        path: '001001',
        children: [
          {
            path: '', redirectTo: '001001001', pathMatch: 'full'
          },
          {
            path: '001001001',
            component: BizCustomerUIDemoCardComponent
          }
        ]
      },
      {
        path: '001005',
          children: [
          {
            path: '', redirectTo: '001005001', pathMatch: 'full'
          },
          {
            path: '001005001',
            component: BizInGoodsUIDemoComponent
          },
          {
            path: '001005002',
            component: BizOutGoodsUIDemoComponent
          }
        ] 
      }
    ]
  }
];
