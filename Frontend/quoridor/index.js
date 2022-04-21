import { getGameState, GameStates } from "../shared/state/sharedState.js";
import { draw as drawBoard, setup as setupBoard } from "./sketch/board.js"
import { draw as drawPS } from "../shared/sketch/playerSelectScreen.js"
import { draw as drawGO } from "../shared/sketch/gameOverScreen.js"
import { NewGame, GetMinimaxAction, FetchPossibleActions } from "../shared/api/sharedApi.js";
import { isOnDebouceCooldown } from "../shared/utilities/buttons.js";
import { useMutex } from "../shared/utilities/mutex.js";

export const setup = async () => {
    window.controller = "quoridor"
    await NewGame();
    setupBoard();
    
    window.addEventListener('keydown', async () => {
        if (getGameState() == GameStates.GAME_OVER) {
            await setup();
        }
    });


    window.addEventListener('human_action', async (e) => {
        if (getGameState() !== GameStates.IN_PROGRESS) return;
        if (isOnDebouceCooldown()) return;
        useMutex(async () => {
            window.drawWaitOverlay = true;
            e.detail.action().then(() =>
                GetMinimaxAction().then(() =>
                    FetchPossibleActions()
                )
            );
            window.drawWaitOverlay = false;
        }, 'human_action')
    });
}

window.drawWaitOverlay = false;

export const draw = () => {
    switch(getGameState()) {
        case GameStates.PLAYER_SELECT:
            drawPS();
            return;
        case GameStates.IN_PROGRESS:
            window.drawWaitOverlay && drawLoadingOverlay();
            drawBoard();
            return;
        case GameStates.GAME_OVER:
            drawBoard();
            drawGO()
            return;
    }
}



const drawLoadingOverlay = () => {
    window.tf.push();
    fill(0);
    textAlign(CENTER, CENTER);
    text("...", window.gameSize * 0.95, window.gameSize * 0.95);
    window.tf.pop();
}
