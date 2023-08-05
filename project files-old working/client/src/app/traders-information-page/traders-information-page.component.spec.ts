import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TradersInformationPageComponent } from './traders-information-page.component';

describe('TradersInformationPageComponent', () => {
  let component: TradersInformationPageComponent;
  let fixture: ComponentFixture<TradersInformationPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TradersInformationPageComponent]
    });
    fixture = TestBed.createComponent(TradersInformationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
