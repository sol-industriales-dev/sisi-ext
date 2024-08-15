let mainCanvas;
let currentMural = 0;
var _state = [];
var _stateRendo = [];
var _mods = 0;
const btnAtras = $("#btnAtras");
const btnAdelante = $("#btnAdelante");
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
        const mural_container = $('.mural-container-content');
        var copiedObject;
        var copiedObjects = new Array();
        var _clipboard;
        let miniMap;
        const toolbar_sigoplan  = $(".toolbar_sigoplan");
        const toolbar_texto_general  = $(".toolbar_texto_general");
        const tb_clsDisminuirLetra  = $(".tb_clsDisminuirLetra");
        const tb_clsAumentarLetra  = $(".tb_clsAumentarLetra");
        const tb_clsFormatoTexto  = $(".tb_clsFormatoTexto");
        const tb_clsColorTexto  = $(".tb_clsColorTexto");
        const tb_clsBorde  = $(".tb_clsBorde");
        const tb_clsColorFondo  = $(".tb_clsColorFondo");
        const ttg_FotmatoTexto = $(".ttg_FotmatoTexto");

        const ttg_FotmatoTexto_BoldParent = $(".ttg_FotmatoTexto_BoldParent");
        const ttg_FotmatoTexto_ItalicParent = $(".ttg_FotmatoTexto_ItalicParent");
        const ttg_FotmatoTexto_UnderlineParent = $(".ttg_FotmatoTexto_UnderlineParent");
        const ttg_FotmatoTexto_StrikeParent = $(".ttg_FotmatoTexto_StrikeParent");
        const ttg_FotmatoTexto_LeftParent = $(".ttg_FotmatoTexto_LeftParent");
        const ttg_FotmatoTexto_CenterParent = $(".ttg_FotmatoTexto_CenterParent");
        const ttg_FotmatoTexto_RightParent = $(".ttg_FotmatoTexto_RightParent");

        const ttg_FotmatoTexto_Bold = $(".ttg_FotmatoTexto_Bold");
        const ttg_FotmatoTexto_Italic = $(".ttg_FotmatoTexto_Italic");
        const ttg_FotmatoTexto_Underline = $(".ttg_FotmatoTexto_Underline");
        const ttg_FotmatoTexto_Strike = $(".ttg_FotmatoTexto_Strike");
        const ttg_FotmatoTexto_Left = $(".ttg_FotmatoTexto_Left");
        const ttg_FotmatoTexto_Center = $(".ttg_FotmatoTexto_Center");
        const ttg_FotmatoTexto_Right = $(".ttg_FotmatoTexto_Right");

        const clsContextMenu = $('.clsContextMenu');
        const cls_CM_TraerEnfrente = $('.cls_CM_TraerEnfrente');
        const cls_CM_EnviarAtras = $('.cls_CM_EnviarAtras');
        const cls_CM_Duplicar = $('.cls_CM_Duplicar');
        const cls_CM_DesbloqueoCualquiera = $('.cls_CM_DesbloqueoCualquiera');
        const cls_CM_DesbloqueoFacilitador = $('.cls_CM_DesbloqueoFacilitador');
        const cls_CM_Desbloquear = $('.cls_CM_Desbloquear');
        const cls_CM_Agrupar = $('.cls_CM_Agrupar');
        const cls_CM_Desagrupar = $('.cls_CM_Desagrupar');
        const cls_CM_MeterArea = $('.cls_CM_MeterArea');
        const cls_CM_AgregarMarcador = $('.cls_CM_AgregarMarcador');
        const cls_CM_CrearContenido = $('.cls_CM_CrearContenido');
        const cls_CM_CopiarEstilo = $('.cls_CM_CopiarEstilo');
        const cls_CM_PegarEstilo = $('.cls_CM_PegarEstilo');
        const cls_CM_Eliminar = $('.cls_CM_Eliminar');
        var clonarStyle = {
            copiado : false,
            fontWeight : '',
            underline : '',
            linethrough : '',
            textAlign : '',
            backgroundColor : '',
            fill : ''
        };

        var clsExportar = $(".clsExportar");
        var colorPicker = $("#colorPicker");
        
        var menuNode = document.getElementById('cxMenuShape');
        var currentShape = { tipo:'', color:'',width:0,height:0};
        var zoomField = $(".zoom-field");
        
        let muralName = $('.muralName');
        let clientX = 0;
        let clienty = 0;

        var groupItems = [];
        let permisoUsuario = -1;
        var clsPrueba = $(".clsPrueba");
        const clsBtnHand = $(".clsBtnHand");
        const _renameMural = new URL(window.location.origin + '/Administrativo/Mural/renameMural');
        function init() {


            if($.urlParam('id')!=undefined){
                currentMural = $.urlParam('id');
            }
            getPermiso();
            fabric.Object.prototype.objectCaching = false;
            fabric.util.addListener(document.getElementsByClassName('upper-canvas')[0], 'contextmenu', function(e) {
                e.preventDefault();
            });
            
            toggleLeftBar();
            initMainCanva();
            fnZoomMouseWheel();
            initMiniMap();
            updateMiniMapVP
            
            dragFromMenu();
            clsExportar.click(function(){
                let width = mainCanvas.getWidth(); 
                let height = mainCanvas.getHeight();

                var pdf = new jsPDF();
                //then we get the dimensions from the 'pdf' file itself
                width = pdf.internal.pageSize.getWidth();
                height = pdf.internal.pageSize.getHeight();

                var zoomTemp = mainCanvas.getZoom();
                mainCanvas.setZoom(0.22);
                var dataImg = mainCanvas.toDataURL({
                    format: 'jpeg',
                    quality: 1,
                    multiplier: 8
                });
                mainCanvas.setZoom(zoomTemp);
                pdf.addImage(dataImg, 'jpeg', 0, 0,width,height);
                pdf.save("download.pdf");
                
            });
            if($.urlParam('id')!=undefined){
                fnGetMural();
            }
            let screenLog = document.querySelector('.mural-container-content');
            document.addEventListener('mousemove', logKey);
            clsPrueba.click(function(){
                saveImg();
            });
            clsBtnHand.hide();
            muralName.change(fnRenameMural);
            btnAtras.click(fnUndo);
            btnAdelante.click(fnRedo);
       
        }
        function updateModifications(savehistory,json) {
            if (savehistory === true) {
                _state.push(json);
                if(_mods>0){
                    _stateRendo = [];
                    btnAdelante.prop("disabled",true);
                }

                if(_state.length == 1)
                {
                    btnAtras.prop("disabled",true);
                }
                else{
                    btnAtras.prop("disabled",false);
                }
            }
        }
        function fnUndo() {
            if ( _state.length > 1) {
                mainCanvas.clear().renderAll();
                
                var indexOff =_state.length - 1;
                _stateRendo.push(_state[indexOff]);
                _state.pop(indexOff);
                indexOff =_state.length - 1;
                mainCanvas.loadFromJSON(_state[indexOff]);
                mainCanvas.renderAll();

                if(indexOff== 0)
                {
                    btnAtras.prop("disabled",true);
                }
                else{
                    btnAtras.prop("disabled",false);
                }
                _mods += 1;
                
                btnAdelante.prop("disabled",false);
                //console.log("mods " + mods);
            }
            else{
                btnAtras.prop("disabled",true);
                btnAdelante.prop("disabled",false);
            }
        }

        function fnRedo() {
            if (_mods > 0) {
                mainCanvas.clear().renderAll();
                var indexOff =_stateRendo.length - 1;
                mainCanvas.loadFromJSON(_stateRendo[indexOff]);
                mainCanvas.renderAll();
                _state.push(_stateRendo[indexOff]);
                _stateRendo.pop(indexOff);

                if(_stateRendo.length>0)
                {
                    btnAdelante.prop("disabled",false);
                }
                else{
                    btnAdelante.prop("disabled",true);
                }
                _mods -= 1;
                
                btnAtras.prop("disabled",false);

            }
        }
        function getPermiso(){
            $.ajax({
                datatype: "json",
                type: "POST",
                url: '/Administrativo/Mural/getTipoPermiso',
                data: { muralID: currentMural },
                async: false,
                success: function (response) {
                    if (response.success) {
                        var tipo = response.tipo;
                        permisoUsuario = tipo;
                    }
                },
                error: function () {
       
                }
            });
        }
        function saveImg(){    
            //console.log(mainCanvas.toDataURL({
            //    format: 'jpeg',
            //    quality: 1,
            //    multiplier: 0.1
            //}));
        }
        function getMuralIcono()
        {
            //var icono = mainCanvas.toDataURL({
            //    format: 'jpeg',
            //    quality: 1,
            //    multiplier: 0.1
            //});
            var icono = "nosave";
            return icono;
        }
        function zoom (width)
        {
            var scale = width / mainCanvas.getWidth();
            height = scale * mainCanvas.getHeight();

            mainCanvas.setDimensions({
                "width": width,
                "height": height
            });

            mainCanvas.calcOffset();
            var objects = mainCanvas.getObjects();
            for (var i in objects) {
                var scaleX = objects[i].scaleX;
                var scaleY = objects[i].scaleY;
                var left = objects[i].left;
                var top = objects[i].top;

                objects[i].scaleX = scaleX * scale;
                objects[i].scaleY = scaleY * scale;
                objects[i].left = left * scale;
                objects[i].top = top * scale;

                objects[i].setCoords();
            }
            mainCanvas.renderAll();
        }
        function groupagain(){
            if(groupItems.length>0){
                var items = [];

                groupItems.forEach(function (obj) {
                    items.push(obj);
                    mainCanvas.remove(obj);
                });
                var grp = new fabric.Group(items.reverse(), {});
                mainCanvas.add(grp);
                mainCanvas.renderAll();
                fnSetMural(false);
                groupItems = [];
            }
        }
        function fnUngroupEdit(group) {
            groupItems = group._objects;
            group._restoreObjectsState();
            mainCanvas.remove(group);
            for (var i = 0; i < groupItems.length; i++) {
                mainCanvas.add(groupItems[i]);
            }
            // if you have disabled render on addition
            mainCanvas.renderAll();
        }
        function fnUngroup(group) {
            var groupItemsTemp = group._objects;
            group._restoreObjectsState();
            mainCanvas.remove(group);
            for (var i = 0; i < groupItemsTemp.length; i++) {
                mainCanvas.add(groupItemsTemp[i]);
            }
            // if you have disabled render on addition
            mainCanvas.renderAll();
        };
        function logKey(e) {
            clientX = e.clientX;
            clientY = e.clientY;
        }
        function dragFromMenu()
        {
            //$( ".draggable" ).draggable({
            //    start: function() {
            //        var _this = $(this);
            //        currentShape.tipo = _this.data("tipo");
            //        currentShape.color = _this.data("color");
            //        currentShape.width = _this.data("width");
            //        currentShape.height = _this.data("height");
            //    },
            //    drag: function() {

            //    },
            //    stop: function() {
            //        createNewShape(currentShape);
            //    }
            //});

            dragItems.on('dragstart','.draggable',function(e){
                var _this = $(this);
                currentShape.tipo = _this.data("tipo");
                currentShape.familia = _this.data("familia");
                currentShape.color = _this.data("color");
                currentShape.width = _this.data("width");
                currentShape.height = _this.data("height");
            });
            dragItems.on('dragend','.draggable',function(e){
                createNewShape(currentShape,e);
            });

        }
        function createNewShape(data,e)
        {
            var obj;
            var pointer = fnGetPointer(e);
            var zoom = mainCanvas.getZoom();
            switch(data.tipo){
                case 'titulo':
                    {
                        var zs = getZoomSize(200,200,25);

                        obj = new fabric.Textbox('Este es un titulo', {
                            left: pointer.x,
                            top: pointer.y,
                            width: zs.width,
                            fontFamily: 'proxima nova',
                            fontSize: zs.fontSize,
                            fontWeight: 'bold',
                            textAlign: 'center',
                            //lockScalingY: true,
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'texto':
                    {
                        var zs = getZoomSize(200,200,16);

                        obj = new fabric.Textbox('Este es un campo de texto', {
                            left: pointer.x,
                            top: pointer.y,
                            width: zs.width,
                            fontFamily: 'proxima nova',
                            fontSize: zs.fontSize,
                            fontWeight: 'normal',
                            textAlign: 'left',
                            //lockScalingY: true,
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'nota':
                    {
                        var zs = getZoomSize(138,138,16);

                        obj = new fabric.Textbox('', {
                            left: pointer.x,
                            top: pointer.y,
                            backgroundColor: data.color,
                            width: zs.width,
                            height: zs.height,
                            fontFamily: 'proxima nova',
                            fontSize: zs.fontSize,
                            textAlign: 'center',
                            fill : data.color == '#000000' ? 'white': 'black',
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'circulo':
                    {
                        var zs = getZoomSize(50,50,16);

                        obj = new fabric.Circle({radius: zs.width,
                            fill: '',
                            stroke: 'black',
                            strokeWidth: 1,
                            //originX: 'center', 
                            //originY: 'center',
                            left: pointer.x,
                            top: pointer.y,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'cuadrado':
                    {
                        var zs = getZoomSize(150,150,16);
     
                        obj = new fabric.Rect({
                            left: pointer.x,
                            top: pointer.y,
                            stroke: 'black',
                            width: zs.width,
                            height: zs.height,
                            fill : 'transparent',
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'triangulo':
                    {
                        var zs = getZoomSize(150,150,16);

                        obj = new fabric.Triangle({
                            left: pointer.x,
                            top: pointer.y,
                            stroke: 'black',
                            width: zs.width,
                            height: zs.height,
                            fill : 'transparent',
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'diamante':
                    {
                        var zs = getZoomSize(150,150,16);

                        obj = new fabric.Rect({
                            left: pointer.x,
                            top: pointer.y,
                            stroke: 'black',
                            width: zs.width,
                            height: zs.height,
                            fill : 'transparent',
                            angle: 45,
                            hasControls: true,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });
                    }
                    break;
                case 'pentagono':
                    {
                        
                    }
                    break;
                case 'hexagono':
                    {
                        
                    }
                    break;
                case 'layout_cuadrado':
                    {
                        var group = [];

                        fabric.loadSVGFromURL("/static/images/stickers/Spaces/2x2-Thumb.svg",function(objects,options) {

                            var loadedObjects = new fabric.Group(group);
                            loadedObjects.set({
                                left: pointer.x,
                                top: pointer.y,
                                borderColor: 'black',
                                cornerColor: 'black',
                                cornerSize: 12,
                                transparentCorners: true
                            });

                            mainCanvas.add(loadedObjects);
                            mainCanvas.renderAll();
                            fnSetMural(true);

                        },function(item, object) {
                            object.set('id',item.getAttribute('id'));
                            group.push(object);
                        });
                    }
                    break;
                default:
                    {
                    
                    }
            }
            if(data.familia != 'layout'){
                mainCanvas.add(obj);
                mainCanvas.setActiveObject(obj);
                fnSetMural(true);
            }
        }
        function getZoomSize(w,h,fs)
        {
            var data = null;
            var zoom = mainCanvas.getZoom();


            if(zoom<=0.09){
                data = getFactorZoom(w,h,fs,22);
            }
            else if(zoom<=0.095){
                data = getFactorZoom(w,h,fs,18);
            }
            else if(zoom<=0.1){
                data = getFactorZoom(w,h,fs,14);
            }
            else if(zoom<=0.2){
                data = getFactorZoom(w,h,fs,12);
            }
            else if(zoom<=0.4){
                data = getFactorZoom(w,h,fs,8);
            }
            else if(zoom<=0.4){
                data = getFactorZoom(w,h,fs,4);
            }
            else if(zoom<=0.5){
                data = getFactorZoom(w,h,fs,1);
            }
            else if(zoom<=0.6){
                data = getFactorZoom(w,h,fs,0.95);
            }
            else if(zoom<=0.7){
                data = getFactorZoom(w,h,fs,0.9);
            }
            else if(zoom<=0.8){
                data = getFactorZoom(w,h,fs,0.85);
            }
            else if(zoom<=0.9){
                data = getFactorZoom(w,h,fs,0.8);
            }
            else if(zoom<=1){
                data = getFactorZoom(w,h,fs,0.75);
            }
            else if(zoom<=1.1){
                data = getFactorZoom(w,h,fs,0.70);
            }
            else if(zoom<=1.2){
                data = getFactorZoom(w,h,fs,0.65);
            }
            else if(zoom<=1.3){
                data = getFactorZoom(w,h,fs,0.60);
            }
            else if(zoom<=1.4){
                data = getFactorZoom(w,h,fs,0.55);
            }
            else if(zoom<=1.5){
                data = getFactorZoom(w,h,fs,0.50);
            }
            else if(zoom<=1.6){
                data = getFactorZoom(w,h,fs,0.45);
            }
            else if(zoom<=1.7){
                data = getFactorZoom(w,h,fs,0.40);
            }
            else if(zoom<=1.8){
                data = getFactorZoom(w,h,fs,0.35);
            }
            else if(zoom<=1.9){
                data = getFactorZoom(w,h,fs,0.22);
            }
            else if(zoom<=1.95){
                data = getFactorZoom(w,h,fs,0.15);
            }
            else{
                data = getFactorZoom(w,h,fs,0);
            }

            return data;
        }
        function getFactorZoom(w,h,fs,m)
        {
            var data = {};
            data.width = w + (w*m);
            data.height = h + (h*m);
            data.fontSize = fs + (fs*m);
            return data;
        }
        function initMainCanva() {
            var width = window.innerWidth;
            var height = window.innerHeight;
            //var html = '<canvas id="canvaPrincipal" width="'+9216+'px" height="'+6237+'px" class="render-engine" tabindex="1"></canvas>';
            //$('#canvaContainer').html(html);
            
            mainCanvas = this.__canvas = new fabric.Canvas('canvaPrincipal');
            mainCanvas.fireRightClick = true;
            mainCanvas.fireMiddleClick = true;
            mainCanvas.preserveObjectStacking = true;
            mainCanvas.stopContextMenu = true;
            mainCanvas.backgroundColor = 'rgb(250, 250, 250)';

            //mainCanvas.width = 9216;
            //mainCanvas.height = 6237;
            mainCanvas.setWidth(2000);
            mainCanvas.setHeight(2000);
            mainCanvas.setZoom(1);
            mainCanvas.renderAll();
            $('.zoom-field').html((mainCanvas.getZoom()*100)+'%');
            miniMap = new fabric.Canvas('previewCanvas',  { selection: false });
            miniMap.width = 160;
            miniMap.height = 108;
            
            //canvas.isDrawingMode = true;
            miniMap.renderAll();
            
            fnAddPostIt();
            fnCopyPasteCut();
            tb_clsDisminuirLetra.click(fnDecrementFontSize);
            tb_clsAumentarLetra.click(fnIncreaseFontSize);
            tb_clsBorde.click(fnSetShapeBorder);
            fnToggleFormatoTextoTB();
            fnFormatoTextoTBMethods();
            fnContextMenu();
            //initCenteringGuidelines(mainCanvas);
            //initAligningGuidelines(mainCanvas);
        }

        function fnCopyPasteCut(){
            $(document).keydown(function(e) {
                e = e || window.event;  // Event object 'ev'
                var key = e.which || e.keyCode; // Detecting keyCode
      
                // Detecting Ctrl
                var ctrl = e.ctrlKey ? e.ctrlKey : ((key === 17)
                    ? true : false);
  
                // If key pressed is V and if ctrl is true.
                if (key == 86 && ctrl) {
                    if(_clipboard!=null){
                        _clipboard.clone(function(clonedObj) {
                            mainCanvas.discardActiveObject();
                        
                            clonedObj.set({
                                left: clonedObj.left + 10,
                                top: clonedObj.top + 10,
                                evented: true,
                            });
                            if (clonedObj.type === 'activeSelection') {
                                // active selection needs a reference to the canvas.
                                clonedObj.canvas = mainCanvas;
                                clonedObj.forEachObject(function(obj) {
                                    mainCanvas.add(obj);
                                });
                                // this should solve the unselectability
                                clonedObj.setCoords();
                            } else {
                                mainCanvas.add(clonedObj);
                            }
                        
                            _clipboard.top += 10;
                            _clipboard.left += 10;

                            mainCanvas.setActiveObject(clonedObj);
                            mainCanvas.requestRenderAll();
                            _clipboard = null;
                        });
                        //_clipboard = null;
                        fnSetMural(false);
                    }
                    else{
                        if(mainCanvas.getActiveObject().isEditing==true){
                            var actual = mainCanvas.getActiveObject();
                            actual.fontSize =5;
                            //var copied = navigator.clipboard.readText().then(text => 
                            //        findLongestWord(text,actual)
                            //    );
      
                        }
                    }
                }
                else if (key == 67 && ctrl) {
                    if(mainCanvas.getActiveObject().isEditing==false){
                        mainCanvas.getActiveObject().clone(function(cloned) {
                            _clipboard = cloned;
                        });
                    }
                    else{
                        _clipboard = null;
                    }
                    
                }
                else if (key == 46) {
                    $.each(mainCanvas.getActiveObjects(),function(i,object){
                        if(object.isEditing==false || object.isEditing == undefined){
                            mainCanvas.remove(object);
                        }
                    });
                    fnSetMural(true);
                }
            });
        }
        function findLongestWord(str,obj) {
            var strSplit = str.split(' ');
            var longestWord = 0;
            for(var i = 0; i < strSplit.length; i++){
                if(strSplit[i].length > longestWord){
                    longestWord = strSplit[i].length;
                }
            }

            const canvas = document.getElementById('canvasMedida');
            const ctx = canvas.getContext('2d');

            let text = ctx.measureText(obj.text);
            obj.set({
                width : text.width
            });
            //return text;
        }
        function fnAddPostIt(){
            if(permisoUsuario==0 || permisoUsuario == 1){
                mainCanvas.on('mouse:dblclick', function (opt) {

                    var activeObject = mainCanvas.getActiveObject();
                    if(activeObject == undefined){

                        var pointer = fnGetPointer(opt.e);
                        var zs = getZoomSize(150,150,16);

                        var t1 = new fabric.Textbox('Nota', {
                            //left: opt.e.offsetX - (285/2),
                            //top: opt.e.offsetY - (285/2),
                            left: (pointer.x),
                            top: (pointer.y),
                            backgroundColor: 'rgb(252, 254, 125,0.9)',
                            textAling: 'center',
                            width: zs.width,
                            height: zs.height,
                            fontFamily: 'proxima nova',
                            fontSize: zs.fontSize,
                            textAlign: 'center',
                            hasControls: true,
                            lockMovementX: false,
                            lockMovementY: false,
                            borderColor: 'black',
                            cornerColor: 'black',
                            cornerSize: 12,
                            transparentCorners: true
                        });

                        mainCanvas.add(t1);
                        mainCanvas.setActiveObject(t1);
                        fnSetMural(true);
                    }
                    else{
                        if(activeObject.type == 'group'){
                            fnUngroupEdit(activeObject);
                        }
                    }

                });
            }
        }
        function fnGetPointer(e){
            var pointer = mainCanvas.getPointer(e);
            return pointer;
        }
        function fnToggleFormatoTextoTB(){
            tb_clsFormatoTexto.click(function(){
                if(tb_clsFormatoTexto.is(":visible")){
                    if(!ttg_FotmatoTexto.is(":visible")){
                        tb_clsFormatoTexto.addClass("open");
                        fnFormatoTextoTBSetData();
                        ttg_FotmatoTexto.show();
                    }
                }
            });
        }
        function fnFormatoTextoTBHide(){
            tb_clsFormatoTexto.removeClass("open");
            ttg_FotmatoTexto.hide();

            ttg_FotmatoTexto_Bold.removeClass("enabled");
            ttg_FotmatoTexto_Italic.removeClass("enabled");
            ttg_FotmatoTexto_Underline.removeClass("enabled");
            ttg_FotmatoTexto_Strike.removeClass("enabled");
            ttg_FotmatoTexto_Left.removeClass("enabled");
            ttg_FotmatoTexto_Center.removeClass("enabled");
            ttg_FotmatoTexto_Right.removeClass("enabled");
        }
        function fnFormatoTextoTBAlign(){
            ttg_FotmatoTexto_Left.removeClass("enabled");
            ttg_FotmatoTexto_Center.removeClass("enabled");
            ttg_FotmatoTexto_Right.removeClass("enabled");
        }
        function fnContextMenu()
        {
            cls_CM_TraerEnfrente.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    o.bringToFront();
                    mainCanvas.renderAll();
                    fnSetMural(false);
                    clsContextMenu.hide();
                }
            });
            cls_CM_EnviarAtras.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    o.sendToBack();
                    mainCanvas.renderAll();
                    fnSetMural(false);
                    clsContextMenu.hide();
                }
            });
            cls_CM_Duplicar.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    mainCanvas.getActiveObject().clone(function(cloned) {
                        _clipboard = cloned;
                    });
                    _clipboard.clone(function(clonedObj) {
                        mainCanvas.discardActiveObject();
                        
                        clonedObj.set({
                            left: clonedObj.left + 10,
                            top: clonedObj.top + 10,
                            evented: true,
                        });
                        if (clonedObj.type === 'activeSelection') {
                            // active selection needs a reference to the canvas.
                            clonedObj.canvas = mainCanvas;
                            clonedObj.forEachObject(function(obj) {
                                mainCanvas.add(obj);
                            });
                            // this should solve the unselectability
                            clonedObj.setCoords();
                        } else {
                            mainCanvas.add(clonedObj);
                        }
                        
                        _clipboard.top += 10;
                        _clipboard.left += 10;

                        mainCanvas.setActiveObject(clonedObj);
                        mainCanvas.requestRenderAll();
                    });
                    fnSetMural(false);
                    clsContextMenu.hide();
                }
            });
            cls_CM_DesbloqueoCualquiera.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    o.hasControls = false;
                    o.lockMovementX = true;
                    o.lockMovementY = true;
                    mainCanvas.renderAll();
                    fnSetMural(false);
                    clsContextMenu.hide();
                }
                
            });
            cls_CM_DesbloqueoFacilitador.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        e.hasControls = false;
                        e.lockMovementX = true;
                        e.lockMovementY = true;
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                    clsContextMenu.hide();
                    

                }
            });
            cls_CM_Desbloquear.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        e.hasControls = true;
                        e.lockMovementX = false;
                        e.lockMovementY = false;
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                    clsContextMenu.hide();

                    //o.hasControls = true;
                    //o.lockMovementX = false;
                    //o.lockMovementY = false;
                    //mainCanvas.renderAll();
                    //fnSetMural(false);
                    //clsContextMenu.hide();
                }
            });
            cls_CM_Agrupar.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    if (mainCanvas.getActiveObject().type == 'activeSelection') {
                        mainCanvas.getActiveObject().toGroup();
                        mainCanvas.requestRenderAll();
                        fnSetMural(false);
                    }
                    clsContextMenu.hide();
                }
            });
            cls_CM_Desagrupar.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    if (mainCanvas.getActiveObject().type == 'group') {
                        //mainCanvas.getActiveObject().toActiveSelection();
                        //mainCanvas.requestRenderAll();
                        fnUngroup(mainCanvas.getActiveObject());
                        fnSetMural(false);
                    }
                    clsContextMenu.hide();
                }
            });
            cls_CM_CopiarEstilo.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    if(mainCanvas.getActiveObjects().length==1){
                        $.each(mainCanvas.getActiveObjects(),function(i,object){
                            clonarStyle.copiado = true;
                            clonarStyle.fontWeight = object.fontWeight;
                            clonarStyle.underline = object.underline;
                            clonarStyle.linethrough = object.linethrough;
                            clonarStyle.textAlign = object.textAlign;
                            clonarStyle.backgroundColor = object.backgroundColor;
                            clonarStyle.fill = object.fill;
                        });
                        fnSetMural(false);
                        clsContextMenu.hide();
                    }
                    else{
                        alert("Para clonar estilo seleccione un solo elemento");
                        clsContextMenu.hide();
                    }
                }
            });
            cls_CM_PegarEstilo.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,object){
                        clonarStyle.copiado = true;
                        object.fontWeight = clonarStyle.fontWeight;
                        object.underline = clonarStyle.underline;
                        object.linethrough = clonarStyle.linethrough;
                        object.textAlign = clonarStyle.textAlign;
                        object.backgroundColor = clonarStyle.backgroundColor;
                        object.fill = clonarStyle.fill;
                    });

                    fnSetMural(false);
                    clsContextMenu.hide();
                }
            });
            cls_CM_Eliminar.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,object){
                        mainCanvas.remove(object);
                    });

                    fnSetMural(false);
                    clsContextMenu.hide();
                }
            });
        }
        function fnFormatoTextoTBMethods(){
            ttg_FotmatoTexto_BoldParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        if(e.fontWeight.includes('bold') && e.fontWeight.includes('italic'))
                        {
                            ttg_FotmatoTexto_Bold.removeClass("enabled");
                            e.fontWeight = 'italic';
                        }
                        else if(e.fontWeight.includes('bold'))
                        {
                            ttg_FotmatoTexto_Bold.removeClass("enabled");
                            e.fontWeight = 'normal';
                        }
                        else if(e.fontWeight.includes('italic'))
                        {
                            ttg_FotmatoTexto_Bold.addClass("enabled");
                            e.fontWeight = 'bold italic';
                        }
                        else{
                            ttg_FotmatoTexto_Bold.addClass("enabled");
                            e.fontWeight = 'bold';
                        }
                    });
                    
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_ItalicParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        if(e.fontWeight.includes('bold') && e.fontWeight.includes('italic'))
                        {
                            ttg_FotmatoTexto_Italic.removeClass("enabled");
                            e.fontWeight = 'bold';
                        }
                        else if(e.fontWeight.includes('italic'))
                        {
                            ttg_FotmatoTexto_Italic.removeClass("enabled");
                            e.fontWeight = 'normal';
                        }
                        else if(e.fontWeight.includes('bold'))
                        {
                            ttg_FotmatoTexto_Italic.addClass("enabled");
                            e.fontWeight = 'bold italic';
                        }
                        else{
                            ttg_FotmatoTexto_Italic.addClass("enabled");
                            e.fontWeight = 'italic';
                        }
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_UnderlineParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        if(e.underline)
                        {
                            ttg_FotmatoTexto_Underline.removeClass("enabled");
                            e.underline = false;
                        }
                        else{
                            ttg_FotmatoTexto_Underline.addClass("enabled");
                            e.underline = true;
                        }
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_StrikeParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        if(e.linethrough)
                        {
                            ttg_FotmatoTexto_Strike.removeClass("enabled");
                            e.linethrough = false;
                        }
                        else{
                            ttg_FotmatoTexto_Strike.addClass("enabled");
                            e.linethrough = true;
                        }
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_LeftParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    fnFormatoTextoTBAlign();
                    ttg_FotmatoTexto_Left.addClass("enabled");
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        e.textAlign = 'left';
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_CenterParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    fnFormatoTextoTBAlign();
                    ttg_FotmatoTexto_Center.addClass("enabled");
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        e.textAlign = 'center';
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
            ttg_FotmatoTexto_RightParent.click(function(){
                var o = mainCanvas.getActiveObject();
                if(o != undefined){
                    fnFormatoTextoTBAlign();
                    ttg_FotmatoTexto_Right.addClass("enabled");
                    $.each(mainCanvas.getActiveObjects(),function(i,e){
                        e.textAlign = 'right';
                    });
                    mainCanvas.renderAll();
                    fnSetMural(false);
                }
            });
        }
        function fnFormatoTextoTBSetData(){
            var o = mainCanvas.getActiveObject();
            if(o != undefined){
                if(o._objects == undefined){
                    if(o.fontWeight.includes('bold'))
                    {
                        ttg_FotmatoTexto_Bold.addClass("enabled");
                    }
                    if(o.fontWeight.includes('italic'))
                    {
                        ttg_FotmatoTexto_Italic.addClass("enabled");
                    }
                    if(o.underline)
                    {
                        ttg_FotmatoTexto_Underline.addClass("enabled");
                    }
                    if(o.linethrough)
                    {
                        ttg_FotmatoTexto_Strike.addClass("enabled");
                    }
                    if(o.textAlign == 'left')
                    {
                        ttg_FotmatoTexto_Left.addClass("enabled");
                    }
                    else if(o.textAlign == 'center')
                    {
                        ttg_FotmatoTexto_Center.addClass("enabled");
                    }
                    else if(o.textAlign == 'right')
                    {
                        ttg_FotmatoTexto_Right.addClass("enabled");
                    }
                }
            }
        }
        function fnToolbarShow(opt){
            var activeObject = mainCanvas.getActiveObject();
            if(activeObject != undefined){
                if(activeObject.type=='textbox' || activeObject.type=='circle' || activeObject.type=='activeSelection' ){
                    //var zoom = mainCanvas.getZoom();
                    //var absCoords = mainCanvas.getAbsoluteCoords(opt.target,zoom);

                    //var x = ((absCoords.left - (opt.target.width*zoom) / 2)) + 'px';
                    //var y = ((absCoords.top - (opt.target.height*zoom) / 2)) + 'px';
                    //toolbar_texto_general.css('transform', 'translate('+( x )+', '+( y )+')');
                   

                    toolbar_texto_general.show();
                }
                else{
                    toolbar_texto_general.hide();
                }
            }
            
        }
        
        function CenterCoord(){
            var zoom=mainCanvas.getZoom();
            return{
                x:fabric.util.invertTransform(mainCanvas.viewportTransform)[4]+(mainCanvas.width/zoom)/2,
                y:fabric.util.invertTransform(mainCanvas.viewportTransform)[5]+(mainCanvas.height/zoom)/2
            }
        }
        function fnToolbarHide(){
            toolbar_texto_general.hide();
            fnFormatoTextoTBHide();
        }
        function fillZoomValue(){
            var zoomNew = mainCanvas.getZoom();
            zoomField.html((zoomNew*100)+'%');
        }
        function fnZoomMouseWheel() {
            mainCanvas.on('mouse:wheel', function(opt) {
                var delta = opt.e.deltaY;
                var zoom = mainCanvas.getZoom();
                zoom *= 0.999 ** delta;
                if (zoom > 2) zoom = 2;
                if (zoom < 0.09) zoom = 0.09;

                mainCanvas.zoomToPoint({ x: opt.e.offsetX, y: opt.e.offsetY }, zoom);
                updateMiniMapVP();
                opt.e.preventDefault();
                opt.e.stopPropagation();
                fillZoomValue();
            });
    
            mainCanvas.on('mouse:down', function (opt) {
                var evt = opt.e;
                var activeObject = mainCanvas.getActiveObject();
                
    
                if (evt.altKey === true) {
                    this.isDragging = false;
                    this.selection = true;
                }
                else if (activeObject == undefined) {
                    this.isDragging = true;
                    this.selection = false;
                    this.lastPosX = evt.clientX;
                    this.lastPosY = evt.clientY;
                    groupagain();
                }
                if(activeObject !=undefined){
                    
                    
                    fnToolbarShow(opt);

                    if(evt.button === 2){
                        //var pointer = fnGetPointer(evt);
                        //var objects = mainCanvas.getObjects();
                        //for (var i = objects.length - 1; i >= 0; i--) {
                        //    if (objects[i].containsPoint(pointer)) {
                        //        mainCanvas.setActiveObject(objects[i]);
                        //        break;
                        //    }
                        //}

                        //if (i < 0) {
                        //    mainCanvas.discardActiveObject();
                        //}

                        //mainCanvas.renderAll();

   

                        clsContextMenu.css('top',clientY+'px')
                        clsContextMenu.css('left',clientX+'px')

                        clsContextMenu.show();
                        //opt.e.preventDefault();
                    }
                    else{

                        clsContextMenu.hide();
                    }
                }
                else{
                    fnToolbarHide();
                    clsContextMenu.hide();
                }

                
            });
 
            mainCanvas.on('mouse:move', function (opt) {
                if (this.isDragging) {
                    
                    var e = opt.e;
                    var vpt = this.viewportTransform;
                    vpt[4] += e.clientX - this.lastPosX;
                    vpt[5] += e.clientY - this.lastPosY;
                    this.requestRenderAll();
                    updateMiniMapVP();
                    this.lastPosX = e.clientX;
                    this.lastPosY = e.clientY;
                }
            });
            mainCanvas.on('mouse:up', function (opt) {
                // on mouse up we want to recalculate new interaction
                // for all objects, so we call setViewportTransform
                
                var activeObject = mainCanvas.getActiveObjects();
                if(activeObject.length>0){
                    var valido = true;
                    $.each(activeObject,function(i,e){
                        if(e.type!='textbox'){
                            valido = false;
                        }
                    });
                    if(valido){
                        fnToolbarShow(opt);
                    }
                }
                else{
                    fnToolbarHide();
                }
                this.setViewportTransform(this.viewportTransform);
                this.isDragging = false;
                this.selection = true;
            });
            //mainCanvas.on('object:added', function() {
            //    updateMiniMap();
            //    fnSetMural(true);
            //});
            mainCanvas.on('object:modified', function(opt) {

                updateMiniMap();
                fnSetMural(true);
            });

            mainCanvas.on('object:moving', function (opt) {
                fnToolbarHide();
            });
            mainCanvas.on('object:moved', function (opt) {
                var activeObject = mainCanvas.getActiveObject();
                if(activeObject !=undefined){
                    fnToolbarShow(opt);
                }
            });
            mainCanvas.on('text:changed', function(opt) {
                var t1 = opt.target;
                if (t1.width > t1.fixedWidth) {
                    t1.fontSize *= t1.fixedWidth / (t1.width + 1);
                    t1.width = t1.fixedWidth;
                }
            });
        }
        //MiniMap
        function createCanvasEl() {
            var designSize = { width: mainCanvas.width,height : mainCanvas.height };
            var originalVPT = mainCanvas.viewportTransform;
            // zoom to fit the design in the display canvas
            var designRatio = fabric.util.findScaleToFit(designSize, mainCanvas);

            // zoom to fit the display the design in the miniMap.
            var miniMapRatio = fabric.util.findScaleToFit(mainCanvas, miniMap);

            var scaling = miniMap.getRetinaScaling();

            var finalWidth =  designSize.width * designRatio;
            var finalHeight =  designSize.height * designRatio;

            mainCanvas.viewportTransform = [
              designRatio, 0, 0, designRatio,
              (mainCanvas.getWidth() - finalWidth) / 2,
              (mainCanvas.getHeight() - finalHeight) / 2
            ];
            var canvas = mainCanvas.toCanvasElement(miniMapRatio * scaling);
            mainCanvas.viewportTransform = originalVPT;
            return canvas;
        }
        function updateMiniMap() {
            var canvas = createCanvasEl();
            miniMap.backgroundImage._element = canvas;
            miniMap.requestRenderAll();
        }
        function updateMiniMapVP() {
            var designSize = { width: mainCanvas.width,height : mainCanvas.height };

            var rect = miniMap.getObjects()[0];
            var designRatio = fabric.util.findScaleToFit(designSize, mainCanvas);
            var totalRatio = fabric.util.findScaleToFit(designSize, miniMap);
            var finalRatio = designRatio / mainCanvas.getZoom();
            rect.scaleX = finalRatio;
            rect.scaleY = finalRatio;
            rect.top = miniMap.backgroundImage.top - mainCanvas.viewportTransform[5] * totalRatio / mainCanvas.getZoom();
            rect.left = miniMap.backgroundImage.left - mainCanvas.viewportTransform[4] * totalRatio / mainCanvas.getZoom();
            miniMap.requestRenderAll();
        }
        function initMiniMap() {
            var canvas = createCanvasEl();
            var backgroundImage = new fabric.Image(canvas);
            backgroundImage.scaleX = 1 / mainCanvas.getRetinaScaling();
            backgroundImage.scaleY = 1 / mainCanvas.getRetinaScaling();
            miniMap.centerObject(backgroundImage);
            miniMap.backgroundColor = 'white';
            miniMap.backgroundImage = backgroundImage;
            miniMap.requestRenderAll();
            var miniMapView = new fabric.Rect({
                top: backgroundImage.top,
                left: backgroundImage.left,
                width: backgroundImage.width / mainCanvas.getRetinaScaling(),
                height: backgroundImage.height/ mainCanvas.getRetinaScaling(),
                fill: 'rgba(0, 0, 255, 0.3)',
                cornerSize: 6,
                transparentCorners: false,
                cornerColor: 'blue',
                strokeWidth: 0,
            });
            miniMapView.controls = {
                br: fabric.Object.prototype.controls.br,
            };
            miniMap.add(miniMapView);
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
        
        function fnIncreaseFontSize(){
            fnFormatoTextoTBHide()
            var activeObject = mainCanvas.getActiveObject();
            if(activeObject!=undefined){
                $.each(mainCanvas.getActiveObjects(),function(i,e){
                    e.fontSize = e.fontSize+1;
                });
                mainCanvas.renderAll();
            }
        }
        function fnDecrementFontSize(){
            fnFormatoTextoTBHide()
            var activeObject = mainCanvas.getActiveObject();
            if(activeObject!=undefined){
                $.each(mainCanvas.getActiveObjects(),function(i,e){
                    e.fontSize = e.fontSize-1;
                });
                mainCanvas.renderAll();
            }
        }
        function fnSetShapeBorder(){
            fnFormatoTextoTBHide()
            var activeObject = mainCanvas.getActiveObject();
            if(activeObject!=undefined){
                if(activeObject.stroke   == ""){
                    activeObject.stroke = 'black';
                }
                else{
                    activeObject.stroke = '';
                }
                mainCanvas.renderAll();
            }
        }

        async function fnSetMural(ico)
        {
            try {
                var json = mainCanvas.toJSON(['hasControls', 'lockMovementX', 'lockMovementY', 'borderColor', 'cornerColor', 'cornerSize', 'transparentCorners']);
                var data = JSON.stringify(json);
                var icono = ico ?getMuralIcono():"nosave";
                response = await ejectFetchJsonNoBlock('/Administrativo/Mural/setMural', { id : currentMural, datos : data , icono: icono });
                if (response.success) {
                    updateModifications(true,json);
                } else {
                    AlertaGeneral(`Error`, `Ocurrió un error al actualizar`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function fnRenameMural()
        {
            try {
                var _this = $(this);
                var id = _this.data('id');
                var name = _this.val();
                response = await ejectFetchJsonNoBlock(_renameMural, {id:id, nombre : name });
                if (response.success) {
                    $(window).scrollTop(0);
                } else {
                    AlertaGeneral(`Error`, `Ocurrió un error al crear mural`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        async function fnGetMural()
        {
            try {
                response = await ejectFetchJson('/Administrativo/Mural/getMural', { id : currentMural});
                if (response.success) {
                    var data = response.data;
                    muralName.data("id",currentMural);
                    muralName.val(data.nombre);
                    mainCanvas.loadFromJSON(data.contenido);
                    if(data.modificado){
                        mainCanvas.setZoom(0.22);
                    }
                    else{
                        mainCanvas.setZoom(1);
                    }
                    if(permisoUsuario == 0 || permisoUsuario == 1)
                    {
                        
                        mainCanvas.renderAll();
                        fillZoomValue();
                        mainCanvas.forEachObject(function(o) {
                            o.borderColor = 'black';
                            o.cornerColor = 'black';
                            o.cornerSize = 12;
                            o.transparentCorners = true;
                        });
                        var json = mainCanvas.toJSON(['hasControls', 'lockMovementX', 'lockMovementY', 'borderColor', 'cornerColor', 'cornerSize', 'transparentCorners']);
                        updateModifications(true,json);
                    }
                    else if(permisoUsuario == 2)
                    {
                        mainCanvas.selection = false;
                        mainCanvas.forEachObject(function(o) {
                            o.selectable = false;
                        });
                        mainCanvas.renderAll();
                        $(".clsNoPermiso").hide();
                        fillZoomValue();
                    }
                    else{
                    
                    }
                    

                } else {
                    AlertaGeneral(`Error`, `Ocurrió un error al cargar`);
                }
            } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
        }
        function initAligningGuidelines(canvas) {

            var ctx = canvas.getSelectionContext(),
                aligningLineOffset = 5,
                aligningLineMargin = 4,
                aligningLineWidth = 1,
                aligningLineColor = 'rgb(0,255,0)',
                viewportTransform,
                zoom = 1;

            function drawVerticalLine(coords) {
                drawLine(
                  coords.x + 0.5,
                  coords.y1 > coords.y2 ? coords.y2 : coords.y1,
                  coords.x + 0.5,
                  coords.y2 > coords.y1 ? coords.y2 : coords.y1);
            }

            function drawHorizontalLine(coords) {
                drawLine(
                  coords.x1 > coords.x2 ? coords.x2 : coords.x1,
                  coords.y + 0.5,
                  coords.x2 > coords.x1 ? coords.x2 : coords.x1,
                  coords.y + 0.5);
            }

            function drawLine(x1, y1, x2, y2) {
                ctx.save();
                ctx.lineWidth = aligningLineWidth;
                ctx.strokeStyle = aligningLineColor;
                ctx.beginPath();
                ctx.moveTo(((x1+viewportTransform[4])*zoom), ((y1+viewportTransform[5])*zoom));
                ctx.lineTo(((x2+viewportTransform[4])*zoom), ((y2+viewportTransform[5])*zoom));
                ctx.stroke();
                ctx.restore();
            }

            function isInRange(value1, value2) {
                value1 = Math.round(value1);
                value2 = Math.round(value2);
                for (var i = value1 - aligningLineMargin, len = value1 + aligningLineMargin; i <= len; i++) {
                    if (i === value2) {
                        return true;
                    }
                }
                return false;
            }

            var verticalLines = [],
                horizontalLines = [];

            canvas.on('mouse:down', function () {
                viewportTransform = canvas.viewportTransform;
                zoom = canvas.getZoom();
            });

            canvas.on('object:moving', function(e) {

                var activeObject = e.target,
                    canvasObjects = canvas.getObjects(),
                    activeObjectCenter = activeObject.getCenterPoint(),
                    activeObjectLeft = activeObjectCenter.x,
                    activeObjectTop = activeObjectCenter.y,
                    activeObjectBoundingRect = activeObject.getBoundingRect(),
                    activeObjectHeight = activeObjectBoundingRect.height / viewportTransform[3],
                    activeObjectWidth = activeObjectBoundingRect.width / viewportTransform[0],
                    horizontalInTheRange = false,
                    verticalInTheRange = false,
                    transform = canvas._currentTransform;

                if (!transform) return;

                // It should be trivial to DRY this up by encapsulating (repeating) creation of x1, x2, y1, and y2 into functions,
                // but we're not doing it here for perf. reasons -- as this a function that's invoked on every mouse move

                for (var i = canvasObjects.length; i--; ) {

                    if (canvasObjects[i] === activeObject) continue;

                    var objectCenter = canvasObjects[i].getCenterPoint(),
                        objectLeft = objectCenter.x,
                        objectTop = objectCenter.y,
                        objectBoundingRect = canvasObjects[i].getBoundingRect(),
                        objectHeight = objectBoundingRect.height / viewportTransform[3],
                        objectWidth = objectBoundingRect.width / viewportTransform[0];

                    // snap by the horizontal center line
                    if (isInRange(objectLeft, activeObjectLeft)) {
                        verticalInTheRange = true;
                        verticalLines.push({
                            x: objectLeft,
                            y1: (objectTop < activeObjectTop)
                              ? (objectTop - objectHeight / 2 - aligningLineOffset)
                              : (objectTop + objectHeight / 2 + aligningLineOffset),
                            y2: (activeObjectTop > objectTop)
                              ? (activeObjectTop + activeObjectHeight / 2 + aligningLineOffset)
                              : (activeObjectTop - activeObjectHeight / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(objectLeft, activeObjectTop), 'center', 'center');
                    }

                    // snap by the left edge
                    if (isInRange(objectLeft - objectWidth / 2, activeObjectLeft - activeObjectWidth / 2)) {
                        verticalInTheRange = true;
                        verticalLines.push({
                            x: objectLeft - objectWidth / 2,
                            y1: (objectTop < activeObjectTop)
                              ? (objectTop - objectHeight / 2 - aligningLineOffset)
                              : (objectTop + objectHeight / 2 + aligningLineOffset),
                            y2: (activeObjectTop > objectTop)
                              ? (activeObjectTop + activeObjectHeight / 2 + aligningLineOffset)
                              : (activeObjectTop - activeObjectHeight / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(objectLeft - objectWidth / 2 + activeObjectWidth / 2, activeObjectTop), 'center', 'center');
                    }

                    // snap by the right edge
                    if (isInRange(objectLeft + objectWidth / 2, activeObjectLeft + activeObjectWidth / 2)) {
                        verticalInTheRange = true;
                        verticalLines.push({
                            x: objectLeft + objectWidth / 2,
                            y1: (objectTop < activeObjectTop)
                              ? (objectTop - objectHeight / 2 - aligningLineOffset)
                              : (objectTop + objectHeight / 2 + aligningLineOffset),
                            y2: (activeObjectTop > objectTop)
                              ? (activeObjectTop + activeObjectHeight / 2 + aligningLineOffset)
                              : (activeObjectTop - activeObjectHeight / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(objectLeft + objectWidth / 2 - activeObjectWidth / 2, activeObjectTop), 'center', 'center');
                    }

                    // snap by the vertical center line
                    if (isInRange(objectTop, activeObjectTop)) {
                        horizontalInTheRange = true;
                        horizontalLines.push({
                            y: objectTop,
                            x1: (objectLeft < activeObjectLeft)
                              ? (objectLeft - objectWidth / 2 - aligningLineOffset)
                              : (objectLeft + objectWidth / 2 + aligningLineOffset),
                            x2: (activeObjectLeft > objectLeft)
                              ? (activeObjectLeft + activeObjectWidth / 2 + aligningLineOffset)
                              : (activeObjectLeft - activeObjectWidth / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(activeObjectLeft, objectTop), 'center', 'center');
                    }

                    // snap by the top edge
                    if (isInRange(objectTop - objectHeight / 2, activeObjectTop - activeObjectHeight / 2)) {
                        horizontalInTheRange = true;
                        horizontalLines.push({
                            y: objectTop - objectHeight / 2,
                            x1: (objectLeft < activeObjectLeft)
                              ? (objectLeft - objectWidth / 2 - aligningLineOffset)
                              : (objectLeft + objectWidth / 2 + aligningLineOffset),
                            x2: (activeObjectLeft > objectLeft)
                              ? (activeObjectLeft + activeObjectWidth / 2 + aligningLineOffset)
                              : (activeObjectLeft - activeObjectWidth / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(activeObjectLeft, objectTop - objectHeight / 2 + activeObjectHeight / 2), 'center', 'center');
                    }

                    // snap by the bottom edge
                    if (isInRange(objectTop + objectHeight / 2, activeObjectTop + activeObjectHeight / 2)) {
                        horizontalInTheRange = true;
                        horizontalLines.push({
                            y: objectTop + objectHeight / 2,
                            x1: (objectLeft < activeObjectLeft)
                              ? (objectLeft - objectWidth / 2 - aligningLineOffset)
                              : (objectLeft + objectWidth / 2 + aligningLineOffset),
                            x2: (activeObjectLeft > objectLeft)
                              ? (activeObjectLeft + activeObjectWidth / 2 + aligningLineOffset)
                              : (activeObjectLeft - activeObjectWidth / 2 - aligningLineOffset)
                        });
                        activeObject.setPositionByOrigin(new fabric.Point(activeObjectLeft, objectTop + objectHeight / 2 - activeObjectHeight / 2), 'center', 'center');
                    }
                }

                if (!horizontalInTheRange) {
                    horizontalLines.length = 0;
                }

                if (!verticalInTheRange) {
                    verticalLines.length = 0;
                }
            });

            canvas.on('before:render', function() {
                canvas.clearContext(canvas.contextTop);
            });

            canvas.on('after:render', function() {
                for (var i = verticalLines.length; i--; ) {
                    drawVerticalLine(verticalLines[i]);
                }
                for (var i = horizontalLines.length; i--; ) {
                    drawHorizontalLine(horizontalLines[i]);
                }

                verticalLines.length = horizontalLines.length = 0;
            });

            canvas.on('mouse:up', function() {
                verticalLines.length = horizontalLines.length = 0;
                canvas.renderAll();
            });
        }
        function initCenteringGuidelines(canvas) {

            var canvasWidth = canvas.getWidth(),
                canvasHeight = canvas.getHeight(),
                canvasWidthCenter = canvasWidth / 2,
                canvasHeightCenter = canvasHeight / 2,
                canvasWidthCenterMap = { },
                canvasHeightCenterMap = { },
                centerLineMargin = 4,
                centerLineColor = 'rgba(255,0,241,0.5)',
                centerLineWidth = 1,
                ctx = canvas.getSelectionContext(),
                viewportTransform;

            for (var i = canvasWidthCenter - centerLineMargin, len = canvasWidthCenter + centerLineMargin; i <= len; i++) {
                canvasWidthCenterMap[Math.round(i)] = true;
            }
            for (var i = canvasHeightCenter - centerLineMargin, len = canvasHeightCenter + centerLineMargin; i <= len; i++) {
                canvasHeightCenterMap[Math.round(i)] = true;
            }

            function showVerticalCenterLine() {
                showCenterLine(canvasWidthCenter + 0.5, 0, canvasWidthCenter + 0.5, canvasHeight);
            }

            function showHorizontalCenterLine() {
                showCenterLine(0, canvasHeightCenter + 0.5, canvasWidth, canvasHeightCenter + 0.5);
            }

            function showCenterLine(x1, y1, x2, y2) {
                ctx.save();
                ctx.strokeStyle = centerLineColor;
                ctx.lineWidth = centerLineWidth;
                ctx.beginPath();
                ctx.moveTo(x1 * viewportTransform[0], y1 * viewportTransform[3]);
                ctx.lineTo(x2 * viewportTransform[0], y2 * viewportTransform[3]);
                ctx.stroke();
                ctx.restore();
            }

            var afterRenderActions = [],
                isInVerticalCenter,
                isInHorizontalCenter;

            canvas.on('mouse:down', function () {
                viewportTransform = canvas.viewportTransform;
            });

            canvas.on('object:moving', function(e) {
                var object = e.target,
                    objectCenter = object.getCenterPoint(),
                    transform = canvas._currentTransform;

                if (!transform) return;

                isInVerticalCenter = Math.round(objectCenter.x) in canvasWidthCenterMap,
                isInHorizontalCenter = Math.round(objectCenter.y) in canvasHeightCenterMap;

                if (isInHorizontalCenter || isInVerticalCenter) {
                    object.setPositionByOrigin(new fabric.Point((isInVerticalCenter ? canvasWidthCenter : objectCenter.x), (isInHorizontalCenter ? canvasHeightCenter : objectCenter.y)), 'center', 'center');
                }
            });

            canvas.on('before:render', function() {
                canvas.clearContext(canvas.contextTop);
            });

            canvas.on('after:render', function() {
                if (isInVerticalCenter) {
                    showVerticalCenterLine();
                }
                if (isInHorizontalCenter) {
                    showHorizontalCenterLine();
                }
            });

            canvas.on('mouse:up', function() {
                // clear these values, to stop drawing guidelines once mouse is up
                isInVerticalCenter = isInHorizontalCenter = null;
                canvas.renderAll();
            });
        }
        fabric.Canvas.prototype.getAbsoluteCoords = function(object,zoom) {
            return {
                left: (object.left*zoom) + (this._offset.left),
                top: (object.top*zoom) + (this._offset.top)
            };
        }
        init();
    }

    $(document).ready(function () {
        recursoshumanos.mural = new mural();
    });
});

function Mural_colorPicker(picker) {
    var a = picker;
    var o = mainCanvas.getActiveObject();
    if(o != undefined){
        $.each(mainCanvas.getActiveObjects(),function(i,e){
            if(e.type == 'textbox'){
                e.backgroundColor = a.toRGBAString();
            }
            else{
                e.fill = a.toRGBAString();
            }
        });
        mainCanvas.renderAll();
        fnSetMural2();
    }
}
function Mural_colorPickerFont(picker) {
    var a = picker;
    var o = mainCanvas.getActiveObject();
    if(o != undefined){
        $.each(mainCanvas.getActiveObjects(),function(i,e){
            e.fill = a.toRGBAString();
        });
        mainCanvas.renderAll();
        fnSetMural2();
    }
}
async function fnSetMural2()
{
    try {
        var json = mainCanvas.toJSON(['hasControls', 'lockMovementX', 'lockMovementY', 'borderColor', 'cornerColor', 'cornerSize', 'transparentCorners']);
        var data = JSON.stringify(json);
        var icono = "nosave";
        response = await ejectFetchJsonNoBlock('/Administrativo/Mural/setMural', { id : currentMural, datos : data, icono : icono });
        if (response.success) {
            updateModifications2(true,json);  
        } else {
            AlertaGeneral(`Error`, `Ocurrió un error al actualizar`);
        }
    } catch (o_O) { AlertaGeneral("Aviso", o_O.message) }
}

function updateModifications2(savehistory,json) {
    if (savehistory === true) {
        _state.push(json);
        btnAtras.prop("disabled",false);
    }
}