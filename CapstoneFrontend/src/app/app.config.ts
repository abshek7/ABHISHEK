import { ApplicationConfig, importProvidersFrom, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { en_US, provideNzI18n } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en';
import { authInterceptorInterceptor } from './interceptors/auth-interceptor-interceptor';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { provideTranslateService } from '@ngx-translate/core';
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader';
import { provideNzIcons } from 'ng-zorro-antd/icon';
import {
  UserOutline,
  LogoutOutline,
  FileTextOutline,
  DashboardOutline,
  AuditOutline,
  TeamOutline,
  PieChartOutline,
  LockOutline,
  SafetyCertificateOutline
} from '@ant-design/icons-angular/icons';

const icons = [
  UserOutline,
  LogoutOutline,
  FileTextOutline,
  DashboardOutline,
  AuditOutline,
  TeamOutline,
  PieChartOutline,
  LockOutline,
  SafetyCertificateOutline
];


registerLocaleData(en);

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([authInterceptorInterceptor])),
    provideNzI18n(en_US),
    provideNzIcons(icons),
    importProvidersFrom(NzTabsModule),
    provideTranslateService({
      defaultLanguage: 'en'
    }),
    provideTranslateHttpLoader({
      prefix: '/assets/i18n',
      suffix: '.json'
    })
  ]
};

