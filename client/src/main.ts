import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

var appData;

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
