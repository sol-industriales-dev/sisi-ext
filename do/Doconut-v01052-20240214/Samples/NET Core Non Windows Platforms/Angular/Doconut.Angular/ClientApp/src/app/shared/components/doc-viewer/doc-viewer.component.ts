import '../../../../assets/scripts/splitter.js';
import '../../../../assets/scripts/docViewer.js';
import '../../../../assets/scripts/bootstrap.min.js';

import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { first } from 'rxjs';
import { SharedService } from '../shared.service';
import { DocViewerConfig } from '../../models/docViewerConfig';
declare var jquery: any;
declare var $: any;

@Component({
  selector: 'doc-viewer',
  templateUrl: './doc-viewer.component.html',
  styleUrls: ['./doc-viewer.component.css']
})

export class DocViewerComponent implements OnInit, AfterViewInit {
  objImg: any = null;
  globalToken: string = "";
  loader: any = null;
  isMobile: boolean = false;
  resizing: boolean = false;
  docViewerDiv: any = null;
  w: number = 0;
  h: number = 0;
  @Input() docViewerConfig: DocViewerConfig = {};
 
  constructor(private sharedService: SharedService) {
  }

  ngOnInit(): void {
    
    this.docViewerDiv = $("#divDocViewer");
    this.loader = $(".loader");
   
  }
  ngAfterViewInit() {
    this.initViewer();
    this.isMobile = this.sharedService.checkIsMobile();
    this.docViewerDiv = $("#divDocViewer");
  }

  GotoPage(page: string) {
    if (page == "First") {
      this.objImg.GotoPage(1);
    }
    else if (page == "Prev") {
      this.objImg.Next(false);
    }
    else if (page == "Next") {
      this.objImg.Next(true);
    }
    else if (page == "Last") {
      this.objImg.GotoPage(parseInt(this.objImg.TotalPages()))
    }
  }

  zoom(param: any) {
    this.objImg.Zoom(param);
  }

  fit(param: string) {
    this.objImg.FitType(param);
  }

  thumbSize(thumbSize: string) {
    this.objImg.ThumbSize(thumbSize);
  }

  rotate(param: number) {
    this.objImg.Rotate(this.objImg.CurrentPage(), param);
  }

  showHideThumbs(isHide: boolean) {
    this.objImg.HideThumbs(isHide);
  }

  showHideSplitter(isHide: boolean) {
    this.objImg.HideSplitter(isHide);
  }

  loadDocument(fileName: string): void {
    this.loader.show();

    this.sharedService.openDocument(fileName)
      .pipe(first())
      .subscribe((result: any) => {

        var _token = JSON.stringify(result);
        _token = _token.replace(/['"]+/g, '');

        //console.log(_token);

        if (_token.indexOf("Error") > -1) {
          alert(_token);
        }
        else {
          this.globalToken = _token;
          this.objImg.View(_token);
        }

        this.loader.hide();

      });
  }

  initViewer(): void {
    this.objImg = $("#div_ctlDoc").docViewer(
      {
        showThumbs: this.docViewerConfig.showThumbs,
        pageZoom: this.docViewerConfig.pageZoom,
        zoomStep: this.docViewerConfig.zoomStep,
        maxZoom: this.docViewerConfig.maxZoom,
        BasePath: this.docViewerConfig.basePath,
        ResPath: this.docViewerConfig.resPath,
        fixedZoom: this.docViewerConfig.fixedZoom,
        fixedZoomPercent: this.docViewerConfig.fixedZoomPercent,
        fixedZoomPercentMobile: this.docViewerConfig.fixedZoomPercentMobile
      });

    this.Resize();
  };

  Resize() {

    if (this.resizing) { return; }

    this.resizing = true;

    var yPadding = 20;

    if (this.isMobile) {
      yPadding = 30; // change as required
    }

    this.docViewerDiv.css("height", "90vh");
    this.docViewerDiv.height(this.docViewerDiv.height() - yPadding);
    this.Refit();
    this.resizing = false;
  };

  Refit() {
  if (null !== this.objImg) {
    this.objImg.Refit();
  }
}
}
