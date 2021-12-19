import { NewGame } from "../api/TicTacToe.api.js";
import * as tictactoe from "./tictactoe.js";
import * as tictactoeOptions from "./tictactoeOptions.js";


async function setup() {
    createCanvas(600, 400);
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
window.gameSize = 400;
window.setup = setup;
window.draw = draw;