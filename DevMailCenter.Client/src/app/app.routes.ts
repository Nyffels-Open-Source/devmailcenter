import {Routes} from '@angular/router';

export const routes: Routes = [{
  path: "",
  children: [{
    path: 'portal',
    loadComponent: () => import('./portal/portal.component').then(c => c.PortalComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./portal/dashboard/dashboard.component').then(c => c.DashboardComponent),
      },
      {
        path: 'e-mails',
        children: [
          {
            path: 'list',
            loadComponent: () => import('./portal/mail/list/list.component').then(c => c.ListComponent),
          },
          {
            path: 'view',
            loadComponent: () => import('./portal/mail/view/view.component').then(c => c.ViewComponent),
          },
          {
            path: '**',
            loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
          }
        ]
      },
      {
        path: 'e-mailservers',
        children: [{
          path: 'list',
          loadComponent: () => import('./portal/mailserver/list/list.component').then(c => c.ListComponent),
        },
          {
            path: 'view',
            loadComponent: () => import('./portal/mailserver/view/view.component').then(c => c.ViewComponent),
          },
          {
            path: 'add',
            loadComponent: () => import('./portal/mailserver/add/add.component').then(c => c.AddComponent),
          },
          {
            path: '**',
            loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
          }
        ]
      },
      {
        path: '**',
        loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
      }
    ],
  },
    {
      path: 'callback',
      loadComponent: () => import('./callback/callback.component').then(c => c.CallbackComponent),
      children: [
        {
          path: 'microsoft',
          loadComponent: () => import("./callback/microsoft/microsoft.component").then(c => c.MicrosoftComponent),
        },
        {
          path: 'google',
          loadComponent: () => import('./callback/google/google.component').then(c => c.GoogleComponent),
        },
        {
          path: '**',
          loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
        }
      ]
    },
    {
      path: "",
      redirectTo: '/portal/dashboard',
      pathMatch: "full"
    },
    {
      path: "**",
      loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
    }
  ],
}];
