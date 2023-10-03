import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonalPageComponent } from './pages/personal-page/personal-page.component';
import { SharePageComponent } from './pages/share-page/share-page.component';
import { TradingPageComponent } from './pages/trading-page/trading-page.component';
import { TradersInformationPageComponent } from './pages/traders-information-page/traders-information-page.component';
import { WelcomePageComponent } from './pages/welcome-page/welcome-page.component';

const routes: Routes = [
  {path:'personal-page', component: PersonalPageComponent},
  {path:'trading-page', component: TradingPageComponent},
  {path:'traders-page', component: TradersInformationPageComponent},
  {path:'welcome-page', component: WelcomePageComponent},
  {path:'share-page', component: SharePageComponent},
  {path: '',   redirectTo: '/welcome-page', pathMatch: 'full' } //default: '/welcome-page
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
