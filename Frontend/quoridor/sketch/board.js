import { getWalls, getPositions, getPossibleMoveActions, getPossibleWallActions, optimisticallyCommitAction } from "../state/quoridorState.js";
import { getCurrentPlayer, getHumanPlayer } from "../../shared/state/sharedState.js";
import { wallColor, tileColor, getPlayerColor, ghostTileColor, validMoveColor, invalidMoveColor } from "../../shared/utilities/colors.js"

let queuedMove = null

export const draw = () => {
    queuedMove = null;
    drawBoardCanvas()
    drawWalls(getWalls())
    drawPlayers(getPositions())
}

const DIMENSION = 9
const SPACING_FACTOR = 1/DIMENSION;
const SUBSPACING_FACTOR = 0.1;

const spacing = () => window.gameSize * SPACING_FACTOR;
const subSpacing = () => spacing() * SUBSPACING_FACTOR;

const mouseWithinTile = (row, col) => {
    const s = spacing();
    const ss = subSpacing();
    const xSat = mouseX > col * s + ss && mouseX < (col + 1) * s - ss;
    const ySat = mouseY > row * s + ss && mouseY < (row + 1) * s - ss;
    return xSat && ySat;
}

const mouseWithinBoard = () => {
    const xSat = mouseX > 0 && mouseX < window.gameSize;
    const ySat = mouseY > 0 && mouseY < window.gameSize;
    return xSat && ySat;
}

const mouseWithinWallSlot = (row, col, r) => {
    const extension = spacing() * 0.5
    let xLower, yLower, xUpper, yUpper;
    if (r == 1) { // vertical
        xLower = (col + 1) * spacing() - (0.5 * subSpacing())
        yLower = (row + 0.5) * spacing()
        xUpper = xLower + subSpacing()
        yUpper = yLower + spacing()
        if (row == 0) yLower -= extension;
        if (row == DIMENSION - 2) yUpper += extension;
    }
    else { // horizontal
        xLower = (col + 0.5) * spacing()
        yLower = (row + 1) * spacing() - (0.5 * subSpacing())
        xUpper = xLower + spacing()
        yUpper = yLower + subSpacing()
        if (col == 0) xLower -= extension;
        if (col == DIMENSION - 2) xUpper += extension;
    }
    const xSat = mouseX > xLower && mouseX < xUpper;
    const ySat = mouseY > yLower && mouseY < yUpper;
    return xSat && ySat;
}

const getValidMove = (row, col) => {
    return getPossibleMoveActions().find(c => c.cell.row == row && c.cell.col == col)
}

const getValidWall = (row, col, r) => {
    return getPossibleWallActions().find(c => c.row == row && c.col == col && c.orientation == r)
}

const getTileColor = (row, col) => {
    if (getCurrentPlayer() != getHumanPlayer()) return tileColor
    if (!mouseWithinBoard()) return tileColor
    if (mouseWithinTile(row, col)) {
        const move = getValidMove(row, col)
        if (move) {
            queuedMove = { ...move, type: "MOVE" }
            return validMoveColor
        }
        return invalidMoveColor
    } 
    return ghostTileColor
}

const drawTile = (row, col) => {
    window.tf.push()
    window.tf.translate(col * spacing(), row * spacing())
    fill(getTileColor(row,col))
    rect(subSpacing(), subSpacing(), spacing() - subSpacing() * 2, spacing() - subSpacing() * 2)
    window.tf.pop()
}

const drawWall = (row, col, r, color = wallColor) => {
    window.tf.push()
    window.tf.translate((col + 1) * spacing(), (row + 1) * spacing())
    window.tf.rotate((r - 1) * 1.57) // pi/2
    fill(color)
    noStroke()
    rect(subSpacing() * - 0.6, (spacing() * -1) + (subSpacing() * 0.5), subSpacing() * 1.2, spacing() * 2 - subSpacing())
    window.tf.pop()
}

const drawPlayer = (row, col, playerId) => {
    window.tf.push()
    window.tf.translate((col + 0.5) * spacing(), (row + 0.5) * spacing())
    fill(getPlayerColor(playerId))
    ellipse(0, 0, spacing() * 0.7)
    window.tf.pop()
}

const drawBoardCanvas = () => {
    rect(0, 0, window.gameSize,  window.gameSize);
    for (let i = 0; i < DIMENSION; i++){
        for (let j = 0; j < DIMENSION; j++){
            drawTile(i, j);
        }
    }
}


const tryDrawWallAction = (row, col, r) => {
    if (!mouseWithinWallSlot(row, col, r)) return false;
    const wallAction = getValidWall(row, col, r)
    const color = wallAction ? validMoveColor : invalidMoveColor
    drawWall(row, col, r, color)
    if (wallAction) {
        queuedMove = { ...wallAction, type: "WALL" }
    }
    return true;
}

const drawWalls = (walls) => {
    let moused = false;
    const drawWallAction = (i, j, r) => {
        if (!moused) {
            moused = tryDrawWallAction(i, j, r);
        }
    }
    for (let i = 0; i < walls.length; i++){
        for (let j = 0; j < walls.length; j++){
            const wall = walls[j][i]
            if (wall) {
                drawWall(i,j, wall);
                continue;
            }
            drawWallAction(i, j, 1);
            drawWallAction(i, j, 2);
        }
    }
}

const drawPlayers = (playerPositions) => {
    Object.keys(playerPositions).forEach(key => {
        const pos = playerPositions[key]
        drawPlayer(pos.row, pos.col, key)
    })
}

export const setup = () => {
    window.addEventListener('click', () => {
        if (queuedMove) {
            const event = new CustomEvent("human_action", {detail: {action: async () => await optimisticallyCommitAction(queuedMove)}});
            window.dispatchEvent(event)
        }
    });
}