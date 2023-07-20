import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssesmentReportComponent } from './assesment-report.component';

describe('AssesmentReportComponent', () => {
  let component: AssesmentReportComponent;
  let fixture: ComponentFixture<AssesmentReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AssesmentReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AssesmentReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
