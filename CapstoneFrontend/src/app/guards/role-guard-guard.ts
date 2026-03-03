import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Auth } from '../services/auth/auth';

export const roleGuardGuard: CanActivateFn = (route, state) => {
  const auth = inject(Auth);
  const router = inject(Router);
  const allowedRoles = route.data?.['roles'] as string[] | undefined;
  const currentRole = auth.role();
  if (!allowedRoles || allowedRoles.length === 0) {
    return true;
  }
  if (currentRole && allowedRoles.includes(currentRole)) {
    return true;
  }
  if (!currentRole) {
    return router.createUrlTree(['/login']);
  }
  if (currentRole === 'Admin') {
    return router.createUrlTree(['/dashboard/admin']);
  }
  if (currentRole === 'Agent') {
    return router.createUrlTree(['/dashboard/agent']);
  }
  if (currentRole === 'Customer') {
    return router.createUrlTree(['/dashboard/customer']);
  }
  if (currentRole === 'ClaimsOfficer') {
    return router.createUrlTree(['/dashboard/claims-officer']);
  }
  return router.createUrlTree(['/login']);
};

