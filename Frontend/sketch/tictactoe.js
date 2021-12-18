const spacing = () => width * 0.33;

const drawVerticalLine = (x) => line(x, 0, x, height);
const drawHorizontalLine = (y) => line(0, y, width, y);

const drawBoard = () => {
    drawVerticalLine(spacing());
    drawVerticalLine(spacing() * 2);
    drawHorizontalLine(spacing());
    drawHorizontalLine(spacing() * 2);
}

const renderTile = (row, col, occupant) => {
    const leftX = spacing() * col;
    const rightX = spacing() * (col + 1);
    const upperY = spacing() * row;
    const lowerY = spacing() * (row + 1);
    const mouseWithinBounds = () => {
        const satX = mouseX > leftX && mouseX < rightX;
        const satY = mouseY > upperY && mouseY < lowerY;
        return satX && satY;
    }

    if (mouseWithinBounds()) {
        push();
        noStroke();
        fill(255, 204, 0);
        rect(leftX, upperY, spacing(), spacing());
        pop();
    }

    let char = '';

    if (occupant == 0) char = 'X';
    if (occupant == 1) char = 'O';
    fill(0);
    textSize(24);
    textAlign(CENTER, CENTER);
    text(char, leftX + spacing() * 0.5, upperY + spacing() * 0.5)
}

export const draw = (board) => {
    if (board) {
        board.forEach(tile => renderTile(tile.row, tile.col, tile.occupiedBy))
    }
    drawBoard();
}