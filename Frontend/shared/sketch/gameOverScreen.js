import { getWinnerColor } from "../../shared/utilities/colors.js";
import { getIsTie, getPlayerOne, getWinnerId } from "../state/sharedState.js";

export const draw = () => {
    disableScreen();
    drawGameOver()
}

const disableScreen = () =>
{
    window.tf.push();
    fill(255,255,255,200);
    noStroke();
    rect(0,0,window.gameSize,window.gameSize);
    window.tf.pop();
}

const drawGameOver = () => {
    if (getIsTie()) {
        overLay('Tie Game')
        return
    }
    const winnertext = getWinnerId() === getPlayerOne() ? 'Player One Wins!' : 'Player Two Wins!'
    overLay(winnertext);
}

const overLay = (displayText) => {
    window.tf.push();
    disableScreen();
    fill(getWinnerColor());
    textAlign(CENTER, CENTER);
    text(displayText, window.gameSize*0.5, window.gameSize*0.5);
    textSize(20);
    text(`press any key to play again`, window.gameSize*0.5, window.gameSize*0.7);
    window.tf.pop();
}