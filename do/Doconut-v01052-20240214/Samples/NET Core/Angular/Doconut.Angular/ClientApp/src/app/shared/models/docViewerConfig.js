"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ImageViewerConfig = void 0;
var ImageViewerConfig = /** @class */ (function () {
    function ImageViewerConfig() {
        this.isShowThumbs = true;
        this.pageZoom = 75;
        this.zoomStep = 10;
        this.maxZoom = 200;
        this.basePath = '/';
        this.resPath = 'images';
        this.isfixedZoom = true;
        this.fixedZoomPercent = 100;
        this.fixedZoomPercentMobile = 75;
    }
    return ImageViewerConfig;
}());
exports.ImageViewerConfig = ImageViewerConfig;
//# sourceMappingURL=imageViewerConfig.js.map