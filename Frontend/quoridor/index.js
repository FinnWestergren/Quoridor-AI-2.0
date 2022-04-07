import { NewGame } from "../shared/api/sharedApi.js";
import { getGameState, GameStates } from "../shared/state/sharedState.js";
import { draw as drawBoard } from "./sketch/board.js"
import { draw as drawPS } from "../shared/sketch/playerSelectScreen.js"

export const setup = async () => {
    window.controller = "quoridor"
    await NewGame()
}

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