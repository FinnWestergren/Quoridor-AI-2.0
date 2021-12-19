import { getBoard, getWinner } from "../state/ticTacToe.state.js";
import { CommitAction, GetMinimaxAction, NewGame } from "../api/TicTacToe.api.js";

const spacing = () => width * 0.33;

const drawVerticalLine = (x) => line(x, 0, x, height);
const drawHorizontalLine = (y) => line(0, y, width, y);

const drawBoard = (board) => {
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

const renderTile = ({row, col, occupiedBy}) => {
    const tileBoundaries = boundaries(row,col);
    let char = '';
    if (occupiedBy == 0) char = 'X';
    if (occupiedBy == 1) char = 'O';
    push();
    fill(0);
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

const drawWinner = (winner) => overLay(`${winner} Wins!`);

const drawTieGame = () => overLay('Tie Game');

const overLay = (displayText) => {
    push();
    fill(255,255,255,200);
    noStroke();
    rect(0,0,width,height);
    fill(255, 100, 100);
    textAlign(CENTER, CENTER);
    text(displayText, width*0.5, height*0.5);
    textSize(20);
    text(`press any key to play again`, width*0.5, height*0.7);
    pop();
}

const isTieGame = (board) => board && !board.some(t => !t.isOccupied);

export const draw = () => {
    const board = getBoard();
    const winner = getWinner();
    if (board) {
        drawBoard(board);
    }
    if (winner) {
        drawWinner(winner);
    }
    if (isTieGame(board)){
        drawTieGame();
    }
}

window.addEventListener('click', async () => {
    const board = getBoard();
    if (board) {
        const mousedTile = mousedOverTile(board);
        if (mousedTile) {
            const success = await CommitAction(mousedTile.serializedCell);
            if (success){
                await GetMinimaxAction();
            }
        }
    }
});


window.addEventListener('keydown', async () => {
    const winner = getWinner();
    const board = getBoard();
    if (winner || isTieGame(board)) {
        await NewGame();
    }
});