import { NewGame } from "../api/TicTacToe.api.js";
import * as tictactoe from "./tictactoe/index.js";
import { Transformer } from "../utilities/transformer.js"

window.gameSize = 600;
window.game = tictactoe;
window.tf = new Transformer()

window.setup = async () => {
    createCanvas(window.gameSize + 200, window.gameSize);
    textFont('Georgia');
    textSize(64);
    await NewGame();
};

window.draw = () => {
    window.tf.push();
    fill(255);
    noStroke();
    rect(0, 0, width, height);
    window.tf.pop();
    window.game.draw();
};
