import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomePageComponent } from './pages/welcome-page/welcome-page.component';
import { PersonalPageComponent } from './pages/personal-page/personal-page.component';
import { SharePageComponent } from './pages/share-page/share-page.component';
import { TradersInformationPageComponent } from './pages/traders-information-page/traders-information-page.component';
import { TradingPageComponent } from './pages/trading-page/trading-page.component';
import { ToolbarComponent } from './toolbar/toolbar.component';

import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';

@NgModule({
  declarations: [
    AppComponent,
    WelcomePageComponent,
    PersonalPageComponent,
    SharePageComponent,
    TradersInformationPageComponent,
    TradingPageComponent,
    ToolbarComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CanvasJSAngularChartsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
