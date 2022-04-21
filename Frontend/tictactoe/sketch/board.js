import { getHumanColor, getPlayerColor } from "../../shared/utilities/colors.js";
import { getBoard, optimisticallyCommitAction } from "../state/tictactoeState.js";

const spacing = () => window.gameSize * 0.33;

const drawVerticalLine = (x) => line(x, 0, x, window.gameSize);
const drawHorizontalLine = (y) => line(0, y, window.gameSize, y);


export const draw = () => {
    const board = getBoard();
    const mousedTile = mousedOverTile(board);
    if(mousedTile) fillTile(mousedTile);

    board.forEach(tile => {
        renderTile(tile);
    });

    const s = spacing();
    drawVerticalLine(s);
    drawVerticalLine(s * 2);
    drawHorizontalLine(s);
    drawHorizontalLine(s * 2);
}


const boundaries = (row, col) => {
    const s = spacing();
    return {
        leftX: s * col,
        rightX: s * (col + 1),
        upperY: s * row,
        lowerY: s * (row + 1),
        centerX: s * (col + 0.5),
        centerY: s * (row + 0.5)
    }
}

const mouseWithinBounds = (tileBoundaries) => {
    const satX = mouseX > tileBoundaries.leftX && mouseX < tileBoundaries.rightX;
    const satY = mouseY > tileBoundaries.upperY && mouseY < tileBoundaries.lowerY;
    return satX && satY;
}

const mousedOverTile = (board) => board.find(t => mouseWithinBounds(boundaries(t.row,t.col)));


const renderTile = ({row, col, printCell}) => {
    if (printCell == '-') return;
    const tileBoundaries = boundaries(row,col);
    window.tf.push();
    textAlign(CENTER, CENTER);
    text(printCell, tileBoundaries.centerX, tileBoundaries.centerY)
    window.tf.pop();
}

const fillTile = ({row, col, occupiedBy}) => {
    const tileBoundaries = boundaries(row,col);
    window.tf.push();
    noStroke();
    occupiedBy == 0 && fill(getHumanColor());
    rect(tileBoundaries.leftX, tileBoundaries.upperY, spacing(), spacing());
    window.tf.pop();
}

export const setup = () => {
    window.addEventListener('click', () => {
        const board = getBoard();
        const mousedTile = mousedOverTile(board);
        if (mousedTile) {
            console.log(mousedTile);
            const event = new CustomEvent("human_action", {detail: {action: async () => await optimisticallyCommitAction(mousedTile.serializedCell, board)}});
            window.dispatchEvent(event)
        }
    });
}
