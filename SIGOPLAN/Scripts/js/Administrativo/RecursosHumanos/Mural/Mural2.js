(() => {
    controller = function () {

        const canvas = $('#canvas');

        const app = new PIXI.Application({
            autoResize: true,
            backgroundColor: 0x1099bb,
            resolution: window.devicePixelRatio
        });

        const postIt = new PIXI.TextInput({
            input: {
                fontSize: '25pt',
                padding: '14px',
                width: '500px',
                color: '#26272E',
                textAlign: 'center'
            },
            box: {
                fill: 0xE8E9F3,
                rounded: 16,
                stroke: {
                    color: 0xCBCEE0,
                    width: 4
                }
            }
        });
        postIt.x = 500;
        postIt.y = 500;
        postIt.placeholder = 'Enter your text...';

        const rectangulo = new PIXI.Graphics();
        rectangulo.lineStyle(4, 0xFF3300, 1);
        rectangulo.beginFill(0x66CCFF);
        rectangulo.drawRect(0, 0, 200, 200);
        rectangulo.endFill();
        rectangulo.x = 170;
        rectangulo.y = 170;

        function onDragStart(event) {
            // store a reference to the data
            // the reason for this is because of multitouch
            // we want to track the movement of this particular touch
            this.data = event.data;
            this.alpha = 0.5;
            this.dragging = true;
        }

        function onDragEnd(event) {
            this.alpha = 1;
            this.dragging = false;
            // set the interaction data to null
            this.data = null;
        }

        function onDragMove(event) {
            if (this.dragging) {
                //const newPosition = this.data.getLocalPosition(this.parent);
                const newPosition = this.data.getLocalPosition(this);
                // this.x = newPosition.x;
                // this.y = newPosition.y;
                console.log('widht: ' + this.width)
                console.log('mouse: ' + event.data.global.x);
                console.log('mouseStart: ' + event.data.target);
                console.log('localPosition: ' + newPosition.x);
                // this.width = event.data.global.x - this.x;
                // // this.height = event.data.global.y - this.y;
                // postIt.setInputStyle('width', event.data.global.x - this.x + 'px');
                // postIt.setInputStyle('height', event.data.global.y - this.y + 'px');
            }
        }

        function resize() {
            app.renderer.resize(window.innerWidth, window.innerHeight);
        }

        $(window).on('resize', function () {
            resize();
        });

        postIt.on('blur', function (event) {
            postIt.setInputStyle('textAlign', 'right');
        });

        (function init() {
            canvas.append(app.view);
            resize();

            rectangulo.hitArea = new PIXI.Polygon([
                200, 200,
                200, 400,
                400, 400,
                400, 200
            ])

            rectangulo.interactive = true;
            rectangulo.buttonMode = true;

            rectangulo.on('pointerdown', onDragStart);
            rectangulo.on('pointerup', onDragEnd);
            rectangulo.on('pointerupoutside', onDragEnd);
            rectangulo.on('pointermove', onDragMove);
            app.stage.addChild(rectangulo);
            app.stage.addChild(postIt);
            postIt.focus();
        })();

    }
    $(document).ready(() => controller = new controller())
        // .ajaxStart(() => $.blockUI({ message: 'Procesando...' }))
        // .ajaxStop($.unblockUI);
})();