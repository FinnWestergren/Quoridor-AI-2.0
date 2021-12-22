import { getBoard, getHumanPlayer, getPlayerTwo, getWinner, isGameOver, isGameInProgress } from "../../state/ticTacToe.state.js";
import { CommitAction, GetMinimaxAction } from "../../api/TicTacToe.api.js";

var disabled = false;

const spacing = () => window.gameSize * 0.33;
const drawVerticalLine = (x) => line(x, 0, x, window.gameSize);
const drawHorizontalLine = (y) => line(0, y, window.gameSize, y);

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

const disableScreen = () =>
{
    push();
    fill(255,255,255,200);
    noStroke();
    rect(0,0,window.gameSize,window.gameSize);
    pop();
}

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

const drawGameOver = () => {
    const winner = getWinner();
    if (winner) overLay(`${winner} Wins!`);
    else overLay('Tie Game');
}

const overLay = (displayText) => {
    push();
    fill(255, 100, 100);
    textAlign(CENTER, CENTER);
    text(displayText, window.gameSize*0.5, window.gameSize*0.5);
    textSize(20);
    text(`press any key to play again`, window.gameSize*0.5, window.gameSize*0.7);
    pop();
}

const getAIAction = async () => {
    disabled = true;
    await GetMinimaxAction();
    disabled = false;
}

export const setup = async () => {
    if (getHumanPlayer() == getPlayerTwo()) {
        await getAIAction();
    }
}

export const draw = () => {
    const board = getBoard();
    const gameOver = isGameOver();
    const humanPlayer = getHumanPlayer();
    if (board && humanPlayer) {
        drawBoard(board);
    }
    if (disabled) {
        disableScreen();
    }
    if (gameOver) {
        disableScreen();
        drawGameOver();
    }
}

window.addEventListener('click', async () => {
    const board = getBoard();
    if (board && isGameInProgress() && !disabled){
        const mousedTile = mousedOverTile(board);
        if (mousedTile) {
            const success = await CommitAction(mousedTile.serializedCell);
            if (success && !isGameOver()) {
                await getAIAction();
            }
        }
    }
});


window.addEventListener('keydown', async () => {
    if(isGameOver()) {
        window.newGame();
    }
});