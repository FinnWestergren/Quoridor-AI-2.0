import { NewGame } from "../api/TicTacToe.api.js";
import { getState, updateState } from "../state.js";
import * as tictactoe from "./tictactoe.js";


function setup() {
    createCanvas(400, 400);
    NewGame().then(resp => updateState(resp.data));
}

function draw() {
    const state = getState();
    push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    pop();
    tictactoe.draw(state?.currentBoard);
}

window.setup = setup;
window.draw = draw;