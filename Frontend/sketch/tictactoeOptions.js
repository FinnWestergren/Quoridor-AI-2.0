import { getHumanPlayer, getPlayerOne, getPlayerTwo, selectPlayer } from "../state/ticTacToe.state.js";
import { createCustomButton } from "../utilities/buttons.js";

const panelSize = () => width - window.gameSize;

export const drawPlayerSelectorHeader = () => {
    push();
    textAlign(CENTER, TOP);
    textSize(15);
    text("take yer pick", panelSize() * 0.2, 0)
    pop();
}

export const drawComputerSelectorHeader = () => {
    const topOffset = 100;
    push();
    translate(40, topOffset);
    textAlign(CENTER, CENTER);
    textSize(10);
    text("minimax", 0, 5);
    text("neural net", 0, 20);
    pop();
}


const createPlayerSelectorButtons = () => {
    const buttonSize = 30;
    const topOffset = 20;
    const common = {
        y: topOffset, 
        w: buttonSize, 
        h: buttonSize,
        txtSize: 20,
        disableFunc: () => !!getHumanPlayer()
    }
    createCustomButton({
        ...common,
        x: window.gameSize, 
        displayText: "X",
        onClick: () => selectPlayer("X"),
        id: "xbutton",
        drawBackground: () => {
           if (getHumanPlayer() == getPlayerOne()) {
                fill(255, 204, 0);
           }
        },
    });

    createCustomButton({
        ...common,
        x: window.gameSize + buttonSize * 1.5, 
        displayText: "O",
        onClick: () => selectPlayer("O"),
        id: "obutton",
        drawBackground: () => {
           if (getHumanPlayer() == getPlayerTwo()) {
                fill(255, 100, 100);
           }
        }
    });
} 

const createComputerSelectorButtons = () => {
    const buttonSize = 10;
    const topOffset = 100;
    const common = {
        x: window.gameSize, 
        w: buttonSize, 
        h: buttonSize
    }
    createCustomButton({
        ...common,
        y: topOffset, 
        onClick: () => {},
        id: "alphabeta minimax",
        drawBackground: () => {
           if (getHumanPlayer() == getPlayerOne()) {
                fill(255, 204, 0);
           }
        },
    });
    createCustomButton({
        ...common,
        y: topOffset + buttonSize * 1.5, 
        onClick: () => {},
        id: "neuralnet",
        drawBackground: () => {
           if (getHumanPlayer() == getPlayerTwo()) {
                fill(255, 204, 0);
           }
        }
    });
} 

export const setup = () => {
    console.log(getHumanPlayer());
    createPlayerSelectorButtons();
    createComputerSelectorButtons();
}

export const draw = () => {
    push();
    translate(window.gameSize, 0);
    drawPlayerSelectorHeader();
    drawComputerSelectorHeader();
    pop();
}
