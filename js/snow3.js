"use strict";

// Improved snow animation.
let FPS = 0;
let FRAMES = 0;
let DELTA = 1.0 / 60.0;
let FRAME_DELTA_TIME = 0;
let FRAME_TIME = performance.now();
let ACTIVE = false;
let SHOW_FPS = true;
const MAX_SNOWFLAKES = 1000|0;
const defaultConfig = {
    'maxSnowflakes': MAX_SNOWFLAKES,
    'density': 0.26,
};

// Shim layer with setTimeout fallback
window.requestAnimFrame = (function() {
  return  window.requestAnimationFrame       ||
          window.webkitRequestAnimationFrame ||
          window.mozRequestAnimationFrame    ||
          function(callback){
              window.setTimeout(callback, 1000 / 60);
          };
})();

// Morally a snowflake is a structure of 6 elements:
//
// struct snowflake {
//     int32_t x;
//     int32_t y;
//     int32_t vx;
//     int32_t vy;
//     int32_t r;
//     float32_t opacity;
// };
//
// We use a compact encoding of snowflakes by flattening them into two
// typed arrays:
// Int32Array(5 * MAX_SNOWFLAKES) // [0,...,4,...,i,...,i+4,...,MAX_SNOWFLAKES-1] stores x, y, vx, vy, r in order.
// Float32Array(MAX_SNOWFLAKES);  // [0,...,i,...,MAX_SNOWFLAKES-1] stores the opacity field

const SNOWFLAKES = new Int32Array(MAX_SNOWFLAKES);
const SNOWFLAKES_OPACITY = new Float32Array(MAX_SNOWFLAKES);
const canvas = document.createElement('canvas');
const context = canvas.getContext('2d');
canvas.style.position = 'absolute';
canvas.style.left = '0';
canvas.style.top = '0';
context.fillStyle = '#FFF';

function snowflakeReset(i, width, height) {
    // x
    SNOWFLAKES[i] = Math.floor(Math.random() * width);
    // y
    SNOWFLAKES[i+1] = Math.floor(Math.random() * -1.0 * height);
    // vx
    SNOWFLAKES[i+2] = Math.floor(0.5 - Math.random());
    // vy
    SNOWFLAKES[i+3] = Math.floor(1.0 + Math.random() * 3.0);
    // r
    SNOWFLAKES[i+4] = Math.ceil(1.0 + Math.random() * 2.0);
    // opacity
    SNOWFLAKES_OPACITY[i] = 0.5 + Math.random() * 0.5;
}

function snowflakeUpdate(i) {
    // Update x += vx
    SNOWFLAKES[i] += SNOWFLAKES[i+2];
    // Update y += vy
    SNOWFLAKES[i+1] += SNOWFLAKES[i+3];
}

function resetScreen() {
    const width = canvas.width;
    const height = canvas.height;
    context.clearRect(0, 0, width, height);
    for (let i = 0|0; i < defaultConfig.maxSnowflakes; i += 5) {
        snowflakeReset(i, width, height);
    }
}

function updateScreen() {
    for (let i = 0|0; i < defaultConfig.maxSnowflakes; i += 5) {
        snowflakeUpdate(i);
        if (SNOWFLAKES[i+1] > canvas.height) snowflakeReset(i, canvas.width, canvas.height);
    }
}

function renderScreen() {
    context.clearRect(0, 0, canvas.width, canvas.height);
    if (SHOW_FPS) {
        context.globalAlpha = 0.75;
        context.font = '48px serif';
        context.fillText(FPS.toFixed(2) + ' fps', 10, 50);
    }
    for (let i = 0|0; i < defaultConfig.maxSnowflakes; i += 5) {
        context.globalAlpha = SNOWFLAKES_OPACITY[i];
        context.beginPath();
        context.arc(SNOWFLAKES[i], SNOWFLAKES[i+1], SNOWFLAKES[i+4], 0, Math.PI * 2, false);
        context.closePath();
        context.fill();
    }
}


function loop() {
    const time = performance.now();
    const delta = time - FRAME_TIME;
    if (delta > 1000) {
        FPS = FRAMES / (1000 / delta);
        FRAMES = 0;
        FRAME_TIME = performance.now();
        if (FPS > 60) return window.requestAnimFrame(loop);
    }
    updateScreen();
    renderScreen();
    FRAMES += 1;
    window.requestAnimFrame(loop);
}

function resizeScreen() {
    canvas.width = document.body.clientWidth;
    canvas.height = document.body.clientHeight;
    context.fillStyle = '#FFF';
    resetScreen();
    const wasActive = ACTIVE;
    ACTIVE = true;
    if (!wasActive && ACTIVE)
        window.requestAnimFrame(loop);
}

function snowStart() {
    document.body.appendChild(canvas);
    resizeScreen();
    window.addEventListener('resize', resizeScreen, false);
}

window.addEventListener('load', snowStart);

