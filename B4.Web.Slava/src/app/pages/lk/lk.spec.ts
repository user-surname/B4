import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LK } from './lk';

describe('LK', () => {
  let component: LK;
  let fixture: ComponentFixture<LK>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LK]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LK);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
