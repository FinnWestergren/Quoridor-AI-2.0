import { p1Color, p2Color } from "../utilities/colors.js"
import { getHumanPlayer, getPlayerOne, getPlayerTwo, selectPlayer } from "../state/sharedState.js";
import { drawButton, isButtonMousedOver } from "../utilities/buttons.js";

const p1Button = {
    boundaries: { x: -40, y: 0, w: 50, h: 50 },
    content: "P1",
    id: "p1_button",
    onClick: () => selectPlayer(1),
}

const p2Button = {
    boundaries: { x: 40, y: 0, w: 50, h: 50 },
    content: "P2",
    id: "p2_button",
    onClick: () => {
        selectPlayer(2)
    }
}

const drawSingleButton = (button, color, selected) => {
    push();
    if (isButtonMousedOver(button.id) || selected) fill(color);
    drawButton(button);
    pop();
}

export const draw = () => {
    textAlign(CENTER, TOP);
    text("Take Yer Pick", window.gameSize/2 , 50)
    window.tf.push()
    window.tf.translate(window.gameSize/2, window.gameSize/4)
    drawSingleButton(p1Button, p1Color, getHumanPlayer() == getPlayerOne());
    drawSingleButton(p2Button, p2Color, getHumanPlayer() == getPlayerTwo());
    window.tf.pop()
}