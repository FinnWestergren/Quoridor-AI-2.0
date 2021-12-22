import {
    getHumanPlayer,
    getPlayerOne,
    getPlayerTwo,
    selectPlayer,
} from "../../state/ticTacToe.state.js";
import { createCustomButton, removeCustomButtons } from "../../utilities/buttons.js";

const panelSize = () => width - window.optionsOffset;
const optionsId = "ttt_options_";

export const drawPlayerSelectorHeader = () => {
    push();
    textAlign(CENTER, TOP);
    textSize(15);
    text("take yer pick", panelSize() * 0.2, 0);
    pop();
};

export const drawComputerSelectorHeader = () => {
    const topOffset = 100;
    push();
    translate(40, topOffset);
    textAlign(CENTER, CENTER);
    textSize(10);
    text("minimax", 0, 5);
    text("alphabeta", 0, 20);
    text("neural net", 0, 35);
    pop();
};

const createPlayerSelectorButtons = () => {
    const buttonSize = 30;
    const topOffset = 20;
    const common = {
        y: topOffset,
        w: buttonSize,
        h: buttonSize,
        txtSize: 20
    };
    createCustomButton({
        ...common,
        x: window.optionsOffset,
        displayText: "X",
        onClick: () => selectPlayer("X"),
        id: `${optionsId}_xbutton`,
        drawBackground: () => {
            if (getHumanPlayer() == getPlayerOne()) {
                fill(255, 204, 0);
            }
        },
    });

    createCustomButton({
        ...common,
        x: window.optionsOffset + buttonSize * 1.5,
        displayText: "O",
        onClick: () => {
            selectPlayer("O");
        },
        id: `${optionsId}_obutton`,
        drawBackground: () => {
            if (getHumanPlayer() == getPlayerTwo()) {
                fill(255, 100, 100);
            }
        },
    });
};

const createComputerSelectorButtons = () => {
    const buttonSize = 10;
    const topOffset = 100;
    const common = {
        x: window.optionsOffset,
        w: buttonSize,
        h: buttonSize,
    };
    createCustomButton({
        ...common,
        y: topOffset,
        onClick: () => {},
        id: `${optionsId}_minimax`,
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
        id: `${optionsId}_alphabeta-minimax`,
        drawBackground: () => {
            if (getHumanPlayer() == getPlayerTwo()) {
                fill(255, 204, 0);
            }
        },
    });
    createCustomButton({
        ...common,
        y: topOffset + buttonSize * 3,
        onClick: () => {},
        id: `${optionsId}_neuralnet`,
        drawBackground: () => {
            if (getHumanPlayer() == getPlayerTwo()) {
                fill(255, 204, 0);
            }
        },
    });
};

const createReadyButton = (onReady) => {
    createCustomButton({
        x: window.optionsOffset,
        y: 200,
        w: 70,
        h: 30,
        displayText: "Ready",
        onClick: onReady,
        id: `${optionsId}_ready`,
        disableFunc: () => !getHumanPlayer()
    });
}

export const setup = (onReady) => {
    window.optionsOffset = 100;
    createPlayerSelectorButtons();
    createComputerSelectorButtons();
    createReadyButton(() => {
        onReady();
        removeCustomButtons(optionsId);
    });
};

export const draw = () => {
    push();
    translate(window.optionsOffset);
    drawPlayerSelectorHeader();
    drawComputerSelectorHeader();
    pop();
};
