import { NewGame } from "../api/TicTacToe.api.js";
import * as tictactoe from "./tictactoe.js";
import * as tictactoeOptions from "./tictactoeOptions.js";

window.gameSize = 600;

async function setup() {
    createCanvas(window.gameSize + 200, window.gameSize);
    textFont('Georgia');
    textSize(64);
    await NewGame();
    tictactoeOptions.setup();
}

function draw() {
    push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    pop();
    tictactoe.draw();
    tictactoeOptions.draw();
    Object.keys(window.customButtons).forEach(k => window.customButtons[k].draw());
}

window.customButtons = {};
window.setup = setup;
window.draw = draw;
