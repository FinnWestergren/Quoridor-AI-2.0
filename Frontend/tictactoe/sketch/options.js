import { getHumanPlayer, getPlayerOne, getPlayerTwo, selectPlayer } from "../../shared/state/sharedState.js";
import { drawButton, isButtonMousedOver } from "../../shared/utilities/buttons.js";
import {p1Color, p2Color} from "../../shared/utilities/colors.js"
import { GetMinimaxAction } from "../../shared/api/sharedApi.js";

const panelSize = () => width - window.gameSize;

const drawPlayerSelectorHeader = (content) => {
    window.tf.push();
    textAlign(CENTER, TOP);
    textSize(15);
    text(content, panelSize() * 0.2, 0)
    window.tf.pop();
}

const drawSingleButton = (button, color, selected) => {
    push();
    if (isButtonMousedOver(button.id) || selected) fill(color);
    drawButton(button);
    pop();
}

const drawPlayerSelectorButtons = () => {
    const xButton = {
        boundaries: { x: 10, y: 20, w: 25, h: 25 },
        content: "X",
        id: "x_button",
        onClick: () => selectPlayer(1),
        isDisabled: !!getHumanPlayer(),
    }
    
    const oButton = {
        boundaries: { x: 40, y: 20, w: 25, h: 25 },
        content: "O",
        id: "o_button",
        onClick: () => {
            selectPlayer(2)
            GetMinimaxAction()
        },
        isDisabled: !!getHumanPlayer(),
    }

    drawSingleButton(xButton, p1Color, getHumanPlayer() == getPlayerOne());
    drawSingleButton(oButton, p2Color, getHumanPlayer() == getPlayerTwo());
}


export const draw = (gameStarted) => {
    window.tf.push();
    if (gameStarted) window.tf.translate(window.gameSize, 0);
    else window.tf.translate(window.gameSize * 0.5, window.gameSize * 0.5);
    drawPlayerSelectorHeader(gameStarted ? "best of luck" : "take yer pick");
    drawPlayerSelectorButtons();
    window.tf.pop();
}
