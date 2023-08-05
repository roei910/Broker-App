import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TradingPageComponent } from './trading-page.component';

describe('TradingPageComponent', () => {
  let component: TradingPageComponent;
  let fixture: ComponentFixture<TradingPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TradingPageComponent]
    });
    fixture = TestBed.createComponent(TradingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
