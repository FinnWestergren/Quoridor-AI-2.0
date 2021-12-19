import { NewGame } from "../api/TicTacToe.api.js";
import * as tictactoe from "./tictactoe.js";


async function setup() {
    createCanvas(400, 400);
    textFont('Georgia');
    textSize(64);
    await NewGame();
}

function draw() {
    push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    pop();
    tictactoe.draw();
}

window.setup = setup;
window.draw = draw;