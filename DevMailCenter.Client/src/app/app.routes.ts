import {Routes} from '@angular/router';

export const routes: Routes = [{
  path: "",
  children: [
    {
      path: 'portal',
      loadComponent: () => import('./portal/portal.component').then(c => c.PortalComponent),
      children: [
        {
          path: 'email',
          children: [
            {
              path: 'list',
              loadComponent: () => import('./portal/mail/list/list.component').then(c => c.ListComponent),
            },
            {
              path: 'view/:id',
              loadComponent: () => import('./portal/mail/view/view.component').then(c => c.ViewComponent),
            },
            {
              path: '**',
              redirectTo: '/404-not-found',
              pathMatch: "full"
            }
          ]
        },
        {
          path: 'emailserver',
          children: [
            {
              path: 'list',
              loadComponent: () => import('./portal/mailserver/list/list.component').then(c => c.ListComponent),
            },
            {
              path: 'view/:id',
              loadComponent: () => import('./portal/mailserver/view/view.component').then(c => c.ViewComponent),
            },
            {
              path: 'add',
              loadComponent: () => import('./portal/mailserver/add/add.component').then(c => c.AddComponent),
            },
            {
              path: '**',
              redirectTo: '/404-not-found',
              pathMatch: "full"
            }
          ]
        },
        {
          path: '',
          redirectTo: '/email/list',
          pathMatch: "full"
        },
        {
          path: '**',
          redirectTo: '/404-not-found',
          pathMatch: "full"
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
          redirectTo: '/404-not-found',
          pathMatch: "full"
        }
      ]
    },
    {
      path: '404-not-found',
      loadComponent: () => import('./page-not-found/page-not-found.component').then(c => c.PageNotFoundComponent),
    },
    {
      path: "",
      redirectTo: '/portal/email/list',
      pathMatch: "full"
    },
    {
      path: "**",
      redirectTo: '/404-not-found',
      pathMatch: "full"
    }
  ],
}];
