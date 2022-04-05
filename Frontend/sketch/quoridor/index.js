export { NewGame } from "../../api/quoridor.api.js";

import { getGameState, GameStates } from "../../state/quoridor.state.js";
import { draw as drawBoard } from "./board.js"
import { draw as drawPS } from "./playerSelectScreen.js"

export const draw = () => {
    switch(getGameState()) {
        case GameStates.PLAYER_SELECT:
            drawPS();
            return;
        case GameStates.IN_PROGRESS:
            drawBoard();
            return;
        case GameStates.GAME_OVER:
            rect(10,10,10,10)
            return;
    }
}