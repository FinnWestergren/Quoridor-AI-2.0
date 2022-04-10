import { getWalls, getPositions } from "../state/quoridorState.js";
import { getCurrentPlayer, getHumanPlayer } from "../../shared/state/sharedState.js";
import { wallColor, tileColor, getPlayerColor, ghostTileColor } from "../../shared/utilities/colors.js"

export const draw = () => {
    drawBoardCanvas()
    drawWalls(getWalls())
    drawPlayers(getPositions())
}

const DIMENSION = 9
const SPACING_FACTOR = 1/DIMENSION;
const SUBSPACING_FACTOR = 0.07;

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
    let xLower, yLower, xUpper, yUpper;
    if (r == 1) { // vertical
        xLower = (i + 1) * spacing() - (0.5 * subSpacing())
        yLower = (j + 0.5) * spacing()
        xUpper = xLower + subSpacing()
        yUpper = yLower + spacing()
    }
    else { // horizontal
        xLower = (i + 0.5) * spacing()
        yLower = (j + 1) * spacing() - (0.5 * subSpacing())
        xUpper = xLower + spacing()
        yUpper = yLower + subSpacing()
    }
    const xSat = mouseX > xLower && mouseX < xUpper;
    const ySat = mouseY > yLower && mouseY < yUpper;
    return xSat && ySat;
}

const getTileColor = (i, j) => {
    if (getCurrentPlayer() != getHumanPlayer()) return tileColor
    if (!mouseWithinBoard()) return tileColor
    if (mouseWithinTile(i, j)) return tileColor
    return ghostTileColor
}

const drawTile = (i, j) => {
    window.tf.push()
    window.tf.translate(i * spacing(), j * spacing())
    fill(getTileColor(i,j))
    rect(subSpacing(), subSpacing(), spacing() - subSpacing() * 2, spacing() - subSpacing() * 2)
    window.tf.pop()
}

const drawWall = (i, j, r) => {
    window.tf.push()
    window.tf.translate((i + 1) * spacing(), (j + 1) * spacing())
    window.tf.rotate((r - 1) * 1.57) // pi/2
    fill(wallColor)
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

const drawWalls = (walls) => {
    let moused = false;
    for (let i = 0; i < walls.length; i++){
        for (let j = 0; j < walls.length; j++){
            const wall = walls[j][i]
            wall && drawWall(i,j, wall);
            if (!moused && mouseWithinWallSlot(i, j, 1)) {
                drawWall(i, j, 1)
                moused = true;
            }
            if (!moused && mouseWithinWallSlot(i, j, 2)) {
                drawWall(i, j, 2)
                moused = true;
                continue
            }
        }
    }
}

const drawPlayers = (playerPositions) => {
    Object.keys(playerPositions).forEach(key => {
        const pos = playerPositions[key]
        drawPlayer(pos.Col, pos.Row, key)
    })
}
