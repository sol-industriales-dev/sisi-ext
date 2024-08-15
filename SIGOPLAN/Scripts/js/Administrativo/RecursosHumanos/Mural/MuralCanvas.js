$(function () {

    $.namespace('recursoshumanos.mural');

    mural = function () {
        const leftBar = $(".mural-sidebar");

        const rightBar = $(".mural-ui-right-bar");
        const menuTopBar = $(".mural-topbar");
        const menuExportar = $("#menuExportar");
        const menuComentarios = $("#menuComentarios");
        const menuChat = $("#menuChat");
        const btnExportPDF = $("#btnExportPDF");
        const btnExportPNG = $("#btnExportPNG");
        const dragItems = $(".drag-items");
        const cxMenuShape = $("#cxMenuShape");
        const menuEliminarshape = $(".menuEliminarshape");
        const menuDuplicarShape = $(".menuDuplicarShape");
        const menuEnviarAtrasShape = $(".menuEnviarAtrasShape");
        const menuTraerEnfrenteShape = $(".menuTraerEnfrenteShape");
        let mainStage;
        let mainLayer;
        let mainTr;
        let mainX;
        let mainY;
        let previewStage;
        let previewLayer;
        var GUIDELINE_OFFSET = 5;
        var mainItemType = '';
        var mainItemColor = '';
        let currentShape;
        var menuNode = document.getElementById('cxMenuShape');
        function init() {
            dragItems.on('dragstart','button',function(e){
                var _this = $(this);
                mainItemType = _this.data("type");
                mainItemColor = _this.data("color");
            });
            toggleLeftBar();
            initMainStage();
            //initMainLayer();
            initMainTr();
            initContenedorDraggable();
            fnZoomMouseWheel();
            fnButtonEvent();
 
            

           

        }
        function initMainStage() {
            var width = window.innerWidth;
            var height = window.innerHeight;

            mainStage = new Konva.Stage({
                container: 'canvaPrincipal',
                width: 9216,
                height: 6237,
                draggable: true
            });
            mainStage.container().style.backgroundColor = 'rgb(250, 250, 250)';
            initMainLayer();

            previewStage = new Konva.Stage({
                container: 'previewCanvas',
                width: window.innerWidth / 12,
                height: window.innerHeight / 10,
                scaleX: 1 / 12,
                scaleY: 1 / 10,
            });
            previewLayer = mainLayer.clone({ listening: false });
            previewStage.add(previewLayer);

            mainStage.on('dragmove', updatePreview);

            mainStage.on('dblclick dbltap', function () {
                var pos = getRelativePointerPosition(mainStage);
                //var rect1 = new Konva.Rect({
                //    //x: mainStage.getPointerPosition().x,
                //    //y: mainStage.getPointerPosition().y,
                //    x: pos.x - (285/2),
                //    y: pos.y - (285/2),
                //    width: 285,
                //    height: 285,
                //    fill: 'rgb(252, 254, 125,0.9)',
                //    draggable: true,
                //    name: 'shape-' + mainLayer.children.length,
                //});

                var rect1 = new fabric.Rect({
                    left: pos.x - (285/2),
                    top: pos.y - (285/2),
                    fill: 'rgb(252, 254, 125,0.9)',
                    width: 285,
                    height: 285
                });
                mainLayer.add(rect1);
                mainLayer.batchDraw();

                //reDrawMiniMap();
            });

            mainStage.on('contextmenu', function (e) {
                // prevent default behavior
                e.evt.preventDefault();
                if (e.target === mainStage) {
                    // if we are on empty place of the stage we will do nothing
                    return;
                }
                currentShape = e.target;
                // show menu
                menuNode.style.display = 'initial';
                var containerRect = mainStage.container().getBoundingClientRect();
                menuNode.style.top =
                  containerRect.top + mainStage.getPointerPosition().y + 4 + 'px';
                menuNode.style.left =
                  containerRect.left + mainStage.getPointerPosition().x + 4 + 'px';
            });
        }
        function initMainLayer() {
            mainLayer = new Konva.Layer();
            mainStage.add(mainLayer);

            mainLayer.on('dragmove', function (e) {
                // clear all previous lines on the screen
                mainLayer.find('.guid-line').destroy();

                // find possible snapping lines
                var lineGuideStops = getLineGuideStops(e.target);
                // find snapping points of current object
                var itemBounds = getObjectSnappingEdges(e.target);

                // now find where can we snap current object
                var guides = getGuides(lineGuideStops, itemBounds);

                // do nothing of no snapping
                if (!guides.length) {
                    return;
                }

                drawGuides(guides);

                var absPos = e.target.absolutePosition();
                // now force object position
                guides.forEach(function(lg) {
                    switch (lg.snap) {
                        case 'start': {
                            switch (lg.orientation) {
                                case 'V': {
                                    absPos.x = lg.lineGuide + lg.offset;
                                    break;
                                }
                                case 'H': {
                                    absPos.y = lg.lineGuide + lg.offset;
                                    break;
                                }
                            }
                            break;
                        }
                        case 'center': {
                            switch (lg.orientation) {
                                case 'V': {
                                    absPos.x = lg.lineGuide + lg.offset;
                                    break;
                                }
                                case 'H': {
                                    absPos.y = lg.lineGuide + lg.offset;
                                    break;
                                }
                            }
                            break;
                        }
                        case 'end': {
                            switch (lg.orientation) {
                                case 'V': {
                                    absPos.x = lg.lineGuide + lg.offset;
                                break;
                                }
                                case 'H': {
                                    absPos.y = lg.lineGuide + lg.offset;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                });
                e.target.absolutePosition(absPos);
            });

            mainLayer.on('dragend', function (e) {
                // clear all previous lines on the screen
                mainLayer.find('.guid-line').destroy();
                mainLayer.batchDraw();
            });
            mainLayer.draw();
        }
        function initMainTr() {
            mainTr = new Konva.Transformer();
            mainLayer.add(mainTr);
            mainStage.on('click tap', function (e) {
                if (e.target.colorKey == "#305de6") {
                    return;
                }
                // if click on empty area - remove all selections
                if (e.target === mainStage) {
                    mainTr.nodes([]);
                    mainLayer.draw();
                    return;
                }

                // do nothing if clicked NOT on our rectangles
                if (!e.target.hasName('stickyNote')) {
                    return;
                }
                
                // do we pressed shift or ctrl?
                const metaPressed = e.evt.shiftKey || e.evt.ctrlKey || e.evt.metaKey;
                const isSelected = mainTr.nodes().indexOf(e.target) >= 0;

                if (!metaPressed && !isSelected) {
                    // if no key pressed and the node is not selected
                    // select just one
                    mainTr.nodes([e.target]);
                } else if (metaPressed && isSelected) {
                    // if we pressed keys and node was selected
                    // we need to remove it from selection:
                    const nodes = mainTr.nodes().slice(); // use slice to have new copy of array
                    // remove node from array
                    nodes.splice(nodes.indexOf(e.target), 1);
                    mainTr.nodes(nodes);
                } else if (metaPressed && !isSelected) {
                    // add the node into selection
                    const nodes = mainTr.nodes().concat([e.target]);
                    mainTr.nodes(nodes);
                }
                mainLayer.draw();
            });
        }
        function initContenedorDraggable() {
            var con = mainStage.container();
            con.addEventListener('dragover', function (e) {
                e.preventDefault(); // !important
            });

            con.addEventListener('drop', function (e) {
                e.preventDefault();
                // now we need to find pointer position
                // we can't use stage.getPointerPosition() here, because that event
                // is not registered by Konva.Stage
                // we can register it manually:
                mainStage.setPointersPositions(e);
                var pos = getRelativePointerPosition(mainStage);
                switch (mainItemType) {
                    case 'notaCuadro': {
                        var rect1 = new Konva.Rect({
                            //x: mainStage.getPointerPosition().x,
                            //y: mainStage.getPointerPosition().y,
                            x: pos.x - (285/2),
                            y: pos.y - (285/2),
                            width: 285,
                            height: 285,
                            fill: mainItemColor,
                            draggable: true,
                            name: 'shape-' + mainLayer.children.length,
                        });
                        mainLayer.add(rect1);
                        break;
                    }
                    case 'notaRectangulo': {
                        var rect1 = new Konva.Rect({
                            //x: mainStage.getPointerPosition().x,
                            //y: mainStage.getPointerPosition().y,
                            x: pos.x - (385/2),
                            y: pos.y - (285/2),
                            width: 385,
                            height: 285,
                            fill: mainItemColor,
                            draggable: true,
                            name: 'shape-' + mainLayer.children.length,
                        });
                        mainLayer.add(rect1);
                        break;
                    }
                    case 'notaCirculo': {
                        
                        break;
                    }
                }
                mainLayer.batchDraw();
                reDrawMiniMap();
            });
        }
        function updatePreview(shape) {
            // we just need to update ALL nodes in the preview
            const clone = previewLayer.findOne('.' + shape.target.name());
            // update its position from the original
            clone.position(shape.target.position());
            previewLayer.batchDraw();
        }
        function fnZoomMouseWheel() {
            var scaleBy = 1.3;
            mainStage.on('wheel', function(e) {
                e.evt.preventDefault();
                var oldScale = mainStage.scaleX();

                var pointer = mainStage.getPointerPosition();

                var mousePointTo = {
                    x: (pointer.x - mainStage.x()) / oldScale,
                    y: (pointer.y - mainStage.y()) / oldScale,
                };

                var newScale = e.evt.deltaY <= 0 ? oldScale * scaleBy : oldScale / scaleBy;

                mainStage.scale({ x: newScale, y: newScale });

                var newPos = {
                    x: pointer.x - mousePointTo.x * newScale,
                    y: pointer.y - mousePointTo.y * newScale,
                };
                mainStage.position(newPos);
                mainStage.batchDraw();
            });
        }
        function getRelativePointerPosition(node) {
            var transform = node.getAbsoluteTransform().copy();
            // to detect relative position we need to invert transform
            transform.invert();

            // get pointer (say mouse or touch) position
            var pos = node.getStage().getPointerPosition();

            // now we can find relative point
            return transform.point(pos);
        }
        
        function toggleLeftBar() {
            leftBar.on("click", ".sidebar-button.clickable.dinamico , .m.m-close-slim", function () {
                leftBar.toggleClass('open full-open');
            });
            menuTopBar.on("click", ".ui-button-export", function () {
                menuExportar.toggleClass('hidden');
            });

            menuTopBar.on("click", "#menuChat , .m.m-close-slim.c-gray.close-btn", function () {
                rightBar.toggleClass('close open');
            });
        }
        function fnButtonEvent() {
            btnExportPDF.click(fnExportStagetoPDF);
            btnExportPNG.click(fnExportStagetoPNG);
            //menuComentarios.click(fnDespliegaComentarios);
            //menuChat.click(fnDespliegaChat);
            menuTraerEnfrenteShape.click(fnTraerEnfrenteShape);
            menuEnviarAtrasShape.click(fnEnviarAtrasShape);
            menuDuplicarShape.click(fnDuplicarShape);
            menuEliminarshape.click(fnDeleteShape);
            
        }
        function fnExportStagetoPDF() {
            menuExportar.toggleClass('hidden');
            var pdf = new jsPDF('l', 'px', [mainStage.width(), mainStage.height()]);
            pdf.setTextColor('#000000');
            // first add texts
            mainStage.find('Text').forEach(function(text) {
                const size = text.fontSize() / 0.75; // convert pixels to points
                pdf.setFontSize(size);
                pdf.text(text.text(), text.x(), text.y(), {
                    baseline: 'top',
                    angle: -text.getAbsoluteRotation(),
                });
            });

            // then put image on top of texts (so texts are not visible)
            pdf.addImage(
              mainStage.toDataURL({ pixelRatio: 2 }),
              0,
              0,
              mainStage.width(),
              mainStage.height()
            );

            pdf.save('canvas.pdf');
        }
        function fnExportStagetoPNG() {
            var dataURL = mainStage.toDataURL({ pixelRatio: 1 });
            downloadURI(dataURL, 'mural.png');
        }
        function downloadURI(uri, name) {
            var link = document.createElement('a');
            link.download = name;
            link.href = uri;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            delete link;
        }
        function fnDespliegaComentarios() {
            
        }
        function fnDespliegaChat() {
            
        }
        // were can we snap our objects?
        function getLineGuideStops(skipShape) {
            // we can snap to stage borders and the center of the stage
            var vertical = [0, mainStage.width() / 2, mainStage.width()];
            var horizontal = [0, mainStage.height() / 2, mainStage.height()];

            // and we snap over edges and center of each object on the canvas
            mainLayer.children.forEach(function(guideItem) {
                if (guideItem === skipShape) {
                return;
                }
                var box = guideItem.getClientRect();
                // and we can snap to all edges of shapes
                vertical.push([box.x, box.x + box.width, box.x + box.width / 2]);
                horizontal.push([box.y, box.y + box.height, box.y + box.height / 2]);
            });
            return {
                vertical: vertical.flat(),
                horizontal: horizontal.flat(),
            };
        }

        // what points of the object will trigger to snapping?
        // it can be just center of the object
        // but we will enable all edges and center
        function getObjectSnappingEdges(node) {
            var box = node.getClientRect();
            var absPos = node.absolutePosition();

            return {
                vertical: [
                  {
                      guide: Math.round(box.x),
                      offset: Math.round(absPos.x - box.x),
                      snap: 'start',
                  },
                  {
                      guide: Math.round(box.x + box.width / 2),
                      offset: Math.round(absPos.x - box.x - box.width / 2),
                      snap: 'center',
                  },
                  {
                      guide: Math.round(box.x + box.width),
                      offset: Math.round(absPos.x - box.x - box.width),
                      snap: 'end',
                  },
                ],
                horizontal: [
                  {
                      guide: Math.round(box.y),
                      offset: Math.round(absPos.y - box.y),
                      snap: 'start',
                  },
                  {
                      guide: Math.round(box.y + box.height / 2),
                      offset: Math.round(absPos.y - box.y - box.height / 2),
                      snap: 'center',
                  },
                  {
                      guide: Math.round(box.y + box.height),
                      offset: Math.round(absPos.y - box.y - box.height),
                      snap: 'end',
                  },
                ],
            };
        }

        // find all snapping possibilities
        function getGuides(lineGuideStops, itemBounds) {
            var resultV = [];
            var resultH = [];

            lineGuideStops.vertical.forEach(function(lineGuide) {
                itemBounds.vertical.forEach(function(itemBound) {
                    var diff = Math.abs(lineGuide - itemBound.guide);
                    // if the distance between guild line and object snap point is close we can consider this for snapping
                    if (diff < GUIDELINE_OFFSET) {
                        resultV.push({
                            lineGuide: lineGuide,
                            diff: diff,
                            snap: itemBound.snap,
                            offset: itemBound.offset,
                        });
                    }
                });
            });

            lineGuideStops.horizontal.forEach(function(lineGuide) {
                itemBounds.horizontal.forEach(function(itemBound) {
                    var diff = Math.abs(lineGuide - itemBound.guide);
                    if (diff < GUIDELINE_OFFSET) {
                        resultH.push({
                            lineGuide: lineGuide,
                            diff: diff,
                            snap: itemBound.snap,
                            offset: itemBound.offset,
                        });
                    }
                });
            });

            var guides = [];

            // find closest snap
            var minV = resultV.sort((a, b) => a.diff - b.diff)[0];
            var minH = resultH.sort((a, b) => a.diff - b.diff)[0];
            if (minV) {
                guides.push({
                    lineGuide: minV.lineGuide,
                    offset: minV.offset,
                    orientation: 'V',
                    snap: minV.snap,
                });
            }
            if (minH) {
                guides.push({
                    lineGuide: minH.lineGuide,
                    offset: minH.offset,
                    orientation: 'H',
                    snap: minH.snap,
                });
            }
            return guides;
        }

        function drawGuides(guides) {
            guides.forEach(function(lg) {
                if (lg.orientation === 'H') {
                    var line = new Konva.Line({
                        points: [-6000, 0, 6000, 0],
                        stroke: 'rgb(0, 161, 255)',
                        strokeWidth: 1,
                        name: 'guid-line',
                        dash: [4, 6],
                    });
                    mainLayer.add(line);
                    line.absolutePosition({
                        x: 0,
                        y: lg.lineGuide,
                    });
                    mainLayer.batchDraw();
                } else if (lg.orientation === 'V') {
                    var line = new Konva.Line({
                        points: [0, -6000, 0, 6000],
                        stroke: 'rgb(0, 161, 255)',
                        strokeWidth: 1,
                        name: 'guid-line',
                        dash: [4, 6],
                    });
                    mainLayer.add(line);
                    line.absolutePosition({
                        x: lg.lineGuide,
                        y: 0,
                    });
                    mainLayer.batchDraw();
                }
            });
        }
        function reDrawMiniMap() {
            // remove all layer
            previewLayer.destroy();
            // generate new one
            previewLayer = mainLayer.clone({ listening: false });
            previewStage.add(previewLayer);
        }
        function fnEditarTexto(e){

        }
        function fnTraerEnfrenteShape() {
            currentShape.moveToTop();
            mainLayer.draw();
        }
        function fnEnviarAtrasShape() {
            currentShape.moveToBottom();
            mainLayer.draw();
        }
        function fnDuplicarShape() {
            var newShape = currentShape.clone();
            newShape.attrs.x = currentShape.attrs.x + currentShape.attrs.width + 20;
            mainLayer.add(newShape);
            mainLayer.draw();
        }
        function fnDeleteShape() {
            currentShape.destroy();
            mainLayer.draw();
        }
        init();
    }

    $(document).ready(function () {
        recursoshumanos.mural = new mural();
    });
});



