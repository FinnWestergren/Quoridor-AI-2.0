import * as ticTacToe from "./tictactoe/index.js";

async function setup() {
    createCanvas(600, 400);
    textFont('Georgia');
    textSize(64);
    await ticTacToe.setup();
}

function draw() {
    push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    pop();
    ticTacToe.draw();
    Object.keys(window.customButtons).forEach(k => window.customButtons[k].draw());
}
window.customButtons = {};
window.gameSize = 400;
window.setup = setup;
window.draw = draw;