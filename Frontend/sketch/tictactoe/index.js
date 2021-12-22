import { isGameInProgress, isGameNotStarted, setGameInProgress } from "../../state/ticTacToe.state.js";
import * as tictactoe from "./tictactoe.js";
import * as tictactoeOptions from "./tictactoeOptions.js";
import { NewGame } from "../../api/TicTacToe.api.js";

export const setup = async () => {
    NewGame();
    tictactoeOptions.setup(() => {
        setGameInProgress()
        tictactoe.setup();
    });
}

export const draw = () => {
    if (isGameNotStarted()) {
        tictactoeOptions.draw();
    }
    else {
        tictactoe.draw();
    }
}

window.newGame = setup;
