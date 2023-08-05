import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharePageComponent } from './share-page.component';

describe('SharePageComponent', () => {
  let component: SharePageComponent;
  let fixture: ComponentFixture<SharePageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SharePageComponent]
    });
    fixture = TestBed.createComponent(SharePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
