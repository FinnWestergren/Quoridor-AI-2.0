import { getWalls, getPositions, getPossibleMoveActions, getPossibleWallActions } from "../state/quoridorState.js";
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

const mouseWithinTile = (i, j) => {
    const s = spacing();
    const ss = subSpacing();
    const xSat = mouseX > i * s + ss && mouseX < (i + 1) * s - ss;
    const ySat = mouseY > j * s + ss && mouseY < (j + 1) * s - ss;
    return xSat && ySat;
}

const mouseWithinBoard = () => {
    const xSat = mouseX > 0 && mouseX < window.gameSize;
    const ySat = mouseY > 0 && mouseY < window.gameSize;
    return xSat && ySat;
}

const mouseWithinWallSlot = (i, j, r) => {
    const extension = spacing() * 0.5
    let xLower, yLower, xUpper, yUpper;
    if (r == 1) { // vertical
        xLower = (i + 1) * spacing() - (0.5 * subSpacing())
        yLower = (j + 0.5) * spacing()
        xUpper = xLower + subSpacing()
        yUpper = yLower + spacing()
        if (j == 0) yLower -= extension;
        if (j == DIMENSION - 2) yUpper += extension;
    }
    else { // horizontal
        xLower = (i + 0.5) * spacing()
        yLower = (j + 1) * spacing() - (0.5 * subSpacing())
        xUpper = xLower + spacing()
        yUpper = yLower + subSpacing()
        if (i == 0) xLower -= extension;
        if (i == DIMENSION - 2) xUpper += extension;
    }
    const xSat = mouseX > xLower && mouseX < xUpper;
    const ySat = mouseY > yLower && mouseY < yUpper;
    return xSat && ySat;
}

const getValidMove = (i, j) => {
    return getPossibleMoveActions().find(c => c.cell.row == j && c.cell.col == i)
}

const getValidWall = (i, j, r) => {
    return getPossibleWallActions().find(c => c.row == j && c.col == i && c.orientation == r)
}

const getTileColor = (i, j) => {
    if (getCurrentPlayer() != getHumanPlayer()) return tileColor
    if (!mouseWithinBoard()) return tileColor
    if (mouseWithinTile(i, j)) {
        const move = getValidMove(i, j)
        if (move) {
            queuedMove = move.serializedAction
            return validMoveColor
        }
        return invalidMoveColor
    } 
    return ghostTileColor
}

const drawTile = (i, j) => {
    window.tf.push()
    window.tf.translate(i * spacing(), j * spacing())
    fill(getTileColor(i,j))
    rect(subSpacing(), subSpacing(), spacing() - subSpacing() * 2, spacing() - subSpacing() * 2)
    window.tf.pop()
}

const drawWall = (i, j, r, color = wallColor) => {
    window.tf.push()
    window.tf.translate((i + 1) * spacing(), (j + 1) * spacing())
    window.tf.rotate((r - 1) * 1.57) // pi/2
    fill(color)
    noStroke()
    rect(subSpacing() * - 0.6, (spacing() * -1) + (subSpacing() * 0.5), subSpacing() * 1.2, spacing() * 2 - subSpacing())
    window.tf.pop()
}

const drawPlayer = (i, j, playerId) => {
    window.tf.push()
    window.tf.translate((i + 0.5) * spacing(), (j + 0.5) * spacing())
    fill(getPlayerColor(playerId))
    ellipse(0, 0, spacing() * 0.7)
    window.tf.pop()
}

const drawBoardCanvas = () => {
    rect(0, 0, window.gameSize,  window.gameSize);
    for (let i = 0; i < DIMENSION; i++){
        for (let j = 0; j < DIMENSION; j++){
            drawTile(i,j);
        }
    }
}


const tryDrawWallAction = (i, j, r) => {
    if (!mouseWithinWallSlot(i, j, r)) return false;
    const wallAction = getValidWall(i, j, r)
    const color = wallAction ? validMoveColor : invalidMoveColor
    drawWall(i, j, r, color)
    if (wallAction) {
        queuedMove = wallAction.serializedAction
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
        drawPlayer(pos.Col, pos.Row, key)
    })
}
