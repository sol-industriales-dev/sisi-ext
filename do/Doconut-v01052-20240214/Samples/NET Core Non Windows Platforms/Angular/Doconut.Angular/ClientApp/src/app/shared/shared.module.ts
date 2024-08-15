import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { DocViewerComponent } from './components/doc-viewer/doc-viewer.component';
import { SharedService } from './components/shared.service';

@NgModule({
  declarations: [
    DocViewerComponent
  ],
  imports: [ CommonModule,FormsModule, ReactiveFormsModule, RouterModule ],
  exports: [
    DocViewerComponent,
    CommonModule,
  ],
  providers: [SharedService]
})
export class SharedModule {}
