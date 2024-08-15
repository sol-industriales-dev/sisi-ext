export class DocViewerConfig {

  public showThumbs?: boolean;
  public pageZoom?: number;
  public zoomStep?: number;
  public maxZoom?: number;
  public basePath?: string;
  public resPath?: string;
  public fixedZoom?: boolean;
  public fixedZoomPercent?: number;
  public fixedZoomPercentMobile?: number;

  constructor() {
    this.showThumbs = true;
    this.pageZoom = 75;
    this.zoomStep = 10;
    this.maxZoom = 200;
    this.basePath = '/';
    this.resPath = 'images';
    this.fixedZoom = true;
    this.fixedZoomPercent = 100;
    this.fixedZoomPercentMobile = 75;
  }
}
