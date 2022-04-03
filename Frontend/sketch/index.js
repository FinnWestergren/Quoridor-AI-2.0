import { NewGame } from "../api/TicTacToe.api.js";
import * as tictactoe from "./tictactoe/index.js";

window.gameSize = 600;
window.game = tictactoe;
window.customButtons = {};

window.setup = async () => {
    createCanvas(window.gameSize + 200, window.gameSize);
    textFont('Georgia');
    textSize(64);
    await NewGame();
    window.game.setup();
};

window.draw = () => {
    push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    pop();
    window.game.draw();
    Object.keys(window.customButtons).forEach(k => window.customButtons[k].draw());
};
