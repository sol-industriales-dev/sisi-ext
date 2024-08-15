import { Component, OnInit } from '@angular/core';
import { DocViewerConfig } from '../../shared/models/docViewerConfig'

@Component({

  templateUrl: './document.component.html',

})
export class DocumentComponent implements OnInit {

  constructor() {
  }
  docConfig: DocViewerConfig = new DocViewerConfig();

  ngOnInit(): void {

    this.docConfig = {
      showThumbs: true,
      pageZoom: 75,
      zoomStep: 10,
      maxZoom: 200,
      basePath: '/',
      resPath: 'images',
      fixedZoom: true,
      fixedZoomPercent: 100,
      fixedZoomPercentMobile:75
    };

  }

}
