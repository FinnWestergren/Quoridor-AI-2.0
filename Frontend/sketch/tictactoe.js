import { getState } from "../state.js";

const spacing = () => width * 0.33;

const drawVerticalLine = (x) => line(x, 0, x, height);
const drawHorizontalLine = (y) => line(0, y, width, y);

const drawBoard = () => {
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

const renderTile = ({row, col, occupiedBy}) => {
    const tileBoundaries = boundaries(row,col);
    let char = '';
    if (occupiedBy == 0) char = 'X';
    if (occupiedBy == 1) char = 'O';
    push();
    fill(0);
    textSize(64);
    textFont('Georgia');
    textAlign(CENTER, CENTER);
    text(char, tileBoundaries.centerX, tileBoundaries.centerY)
    pop();
}

const fillTile = ({row, col, occupiedBy}) => {
    const tileBoundaries = boundaries(row,col);
    push();
    noStroke();
    occupiedBy == 2 ? fill(255, 204, 0) : fill(255, 100, 100);
    rect(tileBoundaries.leftX, tileBoundaries.upperY, spacing(), spacing());
    pop();
}

export const draw = () => {
    const state = getState();
    if (state?.currentBoard) {
        const mousedTile = mousedOverTile(state.currentBoard);
        if(mousedTile) fillTile(mousedTile);
        state.currentBoard.forEach(tile => {
            renderTile(tile);
        });
    }
    drawBoard();
}

window.addEventListener("click", () => {
    const state = getState();
    if (state?.currentBoard) {
        const mousedTile = mousedOverTile(state.currentBoard);
        mousedTile && console.log(mousedTile);
    }
});