import { Routes } from '@angular/router';
import { authGuardGuard } from './guards/auth-guard-guard';
import { roleGuardGuard } from './guards/role-guard-guard';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'welcome' },

  {
    path: 'welcome',
    loadComponent: () =>
      import('./components/public-page/public-page').then(m => m.PublicPage),
  },

  {
    path: 'login',
    loadComponent: () =>
      import('./components/login/login').then(m => m.Login),
  },

  {
    path: 'register',
    loadComponent: () =>
      import('./components/register/register').then(m => m.Register),
  },

  {
    path: 'dashboard/admin',
    canActivate: [authGuardGuard, roleGuardGuard],
    data: { roles: ['Admin'] },
    loadComponent: () =>
      import('./components/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'users' },
      {
        path: 'users',
        loadComponent: () =>
          import('./components/admin-dashboard/user-management/admin-user-management').then(m => m.AdminUserManagement)
      },
      {
        path: 'oversight',
        loadComponent: () =>
          import('./components/admin-dashboard/oversight/admin-oversight').then(m => m.AdminOversight)
      },
      {
        path: 'analytics',
        loadComponent: () =>
          import('./components/admin-dashboard/analytics/admin-analytics').then(m => m.AdminAnalytics)
      }
    ]
  },

  {
    path: 'dashboard/agent',
    canActivate: [authGuardGuard, roleGuardGuard],
    data: { roles: ['Agent'] },
    loadComponent: () =>
      import('./components/agent-dashboard/agent-dashboard').then(m => m.AgentDashboard),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'workload' },
      {
        path: 'workload',
        loadComponent: () =>
          import('./components/agent-dashboard/workload/agent-workload').then(m => m.AgentWorkload)
      },
      {
        path: 'policies',
        loadComponent: () =>
          import('./components/agent-dashboard/policies/agent-policies').then(m => m.AgentPolicies)
      },
      {
        path: 'customers',
        loadComponent: () =>
          import('./components/agent-dashboard/customers/agent-customers').then(m => m.AgentCustomers)
      }
    ]
  },

  {
    path: 'dashboard/customer',
    canActivate: [authGuardGuard, roleGuardGuard],
    data: { roles: ['Customer'] },

    loadComponent: () =>
      import('./components/customer-dashboard/customer-dashboard').then(
        m => m.CustomerDashboard
      ),

    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'policies'
      },
      {
        path: 'create-policy',
        loadComponent: () =>
          import('./components/customer-dashboard/create-policy/customer-create-policy').then(
            m => m.CustomerCreatePolicy
          ),
      },
      {
        path: 'policies',
        loadComponent: () =>
          import('./components/customer-dashboard/policy-list/customer-policy-list').then(
            m => m.CustomerPolicyList
          ),
      },
      {
        path: 'raise-claim',
        loadComponent: () =>
          import('./components/customer-dashboard/raise-claim/customer-raise-claim').then(
            m => m.CustomerRaiseClaim
          ),
      },
      {
        path: 'claims',
        loadComponent: () =>
          import('./components/customer-dashboard/claim-status/customer-claim-status').then(
            m => m.CustomerClaimStatus
          ),
      },
      {
        path: 'payments',
        loadComponent: () =>
          import('./components/customer-dashboard/payments/customer-payments').then(
            m => m.CustomerPayments
          ),
      },
      {
        path: 'nominee',
        loadComponent: () =>
          import('./components/customer-dashboard/nominee/customer-nominee').then(
            m => m.CustomerNominee
          ),
      },
      {
        path: 'analytics',
        loadComponent: () =>
          import('./components/customer-dashboard/analytics/customer-analytics').then(
            m => m.CustomerAnalytics
          ),
      },
    ],
  },
  {
    path: 'dashboard/claims-officer',
    canActivate: [authGuardGuard, roleGuardGuard],
    data: { roles: ['ClaimsOfficer'] },
    loadComponent: () =>
      import('./components/claims-dashboard/claims-dashboard').then(m => m.ClaimsDashboard),
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'queue' },
      {
        path: 'queue',
        loadComponent: () =>
          import('./components/claims-dashboard/queue/claims-queue').then(m => m.ClaimsQueue)
      },
      {
        path: 'analytics',
        loadComponent: () =>
          import('./components/claims-dashboard/analytics/claims-analytics').then(m => m.ClaimsAnalytics)
      }
    ]
  },

  {
    path: 'reports',
    canActivate: [authGuardGuard, roleGuardGuard],
    data: { roles: ['Admin'] },
    loadComponent: () =>
      import('./components/reports-page/reports-page').then(m => m.ReportsPage),
  },
];