"use strict";

// Snow animation namespace.
const Snow = (function() {
    const MAX_SNOWFLAKES = 1000|0;
    let $SNOWFLAKES = new Float32Array(6 * MAX_SNOWFLAKES);
    let $CANVAS = null;
    let $CONTEXT = null;
    let ACTIVE = false;
    let STOP = false;
    let SHOW_FPS = false;
    let FPS = 0;
    let FRAMES = 0;
    let FRAME_TIME = performance.now();

    // Shim layer with setTimeout fallback
    const requestAnimFrame = (function() {
        return window.requestAnimationFrame       ||
               window.webkitRequestAnimationFrame ||
               window.mozRequestAnimationFrame    ||
               function(callback){
                   window.setTimeout(callback, 1000 / 60);
               };
    })();

    // Morally a snowflake is a structure of 6 elements:
    //
    // struct snowflake {
    //     float32_t x;
    //     float32_t y;
    //     float32_t vx;
    //     float32_t vy;
    //     float32_t r;
    //     float32_t opacity;
    // };
    //
    // We use a compact encoding of snowflakes by flattening them into
    // a single typed array:
    // Float32Array(6 * MAX_SNOWFLAKES) // [0,...,5,...,i,...,i+5,...,MAX_SNOWFLAKES-1] stores x, y, vx, vy, r, opacity in order.

    function snowflakeReset(i, width, height) {
        // x
        $SNOWFLAKES[i] = Math.random() * width;
        // y
        $SNOWFLAKES[i+1] = Math.random() * -height;
        // vx
        $SNOWFLAKES[i+2] = 0.5 - Math.random();
        // vy
        $SNOWFLAKES[i+3] = 1.0 + Math.random() * 3.0;
        // r
        $SNOWFLAKES[i+4] = 1.0 + Math.random() * 2.0;
        // opacity
        $SNOWFLAKES[i+5] = 0.5 + Math.random() * 0.5;
    }

    function resetScreen() {
        const width = $CANVAS.width;
        const height = $CANVAS.height;
        $CONTEXT.clearRect(0, 0, width, height);
        for (let i = 0|0; i < $SNOWFLAKES.length; i += 6) {
            snowflakeReset(i, width, height);
        }
    }

    function updateScreen() {
        for (let i = 0|0; i < $SNOWFLAKES.length; i += 6) {
            // Update x += vx
            $SNOWFLAKES[i] += $SNOWFLAKES[i+2];
            // Update y += vy
            $SNOWFLAKES[i+1] += $SNOWFLAKES[i+3];
            if ($SNOWFLAKES[i+1] > $CANVAS.height) snowflakeReset(i, $CANVAS.width, $CANVAS.height);
        }
    }

    function renderScreen() {
        $CONTEXT.clearRect(0, 0, $CANVAS.width, $CANVAS.height);
        if (SHOW_FPS) {
            $CONTEXT.globalAlpha = 0.75;
            $CONTEXT.font = '48px serif';
            $CONTEXT.fillText(FPS.toFixed(2) + ' fps', 10, 50);
        }
        for (let i = 0|0; i < $SNOWFLAKES.length; i += 6) {
            $CONTEXT.beginPath();
            $CONTEXT.arc($SNOWFLAKES[i], $SNOWFLAKES[i+1], $SNOWFLAKES[i+4], 0, Math.PI * 2, false);
            $CONTEXT.closePath();
            $CONTEXT.globalAlpha = $SNOWFLAKES[i+5];
            $CONTEXT.fill();
        }
    }


    function loop() {
        if (STOP) return;
        const time = performance.now(); // in milliseconds
        const delta = time - FRAME_TIME;
        if (delta > 1000) {
            FPS = FRAMES / (1000 / delta);
            FRAMES = 0;
            FRAME_TIME = performance.now();
            if (FPS > 60) return requestAnimFrame(loop);
        }
        updateScreen();
        renderScreen();
        FRAMES += 1;
        requestAnimFrame(loop);
    }

    function resizeScreen() {
        $CANVAS.width = document.body.clientWidth;
        $CANVAS.height = document.body.clientHeight;
        $CONTEXT.fillStyle = '#FFF';
        resetScreen();
        if (STOP) return;
        const wasActive = ACTIVE;
        ACTIVE = true;
        if (!wasActive && ACTIVE)
            requestAnimFrame(loop);
    }

    function snowStart(canvas) {
        if ($CANVAS === null) {
            $CANVAS = document.createElement('canvas');
            $CANVAS.style.position = 'absolute';
            $CANVAS.style.left = '0';
            $CANVAS.style.top = '0';
            document.body.appendChild($CANVAS);
            $CONTEXT = $CANVAS.getContext('2d');
        }
        STOP = false;
        resizeScreen();
        window.addEventListener('resize', resizeScreen, false);
    }

    function snowStop() {
        resetScreen();
        ACTIVE = false;
        STOP = true;
    }

    function isActive() {
        return ACTIVE;
    }

    return {'start': snowStart, // Starts the animations
            'stop': snowStop,   // Stops the animations
            'isActive': isActive,
            'toggleFPS': function() { // Toggles FPS display
                SHOW_FPS = !SHOW_FPS;
            }};
})();
