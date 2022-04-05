export { NewGame } from "../../api/quoridor.api.js";
import { getWalls, getPositions } from "../../state/quoridor.state.js";
import mockWalls from "../../mock/mockWalls.js";
import { wallColor, tileColor, getPlayerColor } from "../../utilities/colors.js"

const DIMENSION = 9
const SPACING_FACTOR = 1/DIMENSION;
const SUBSPACING_FACTOR = 0.05;

export const draw = () => {
    const gameReady = !!getWalls()
    drawBoardCanvas()
    if (gameReady) {
        drawWalls(mockWalls)
        drawPlayers(getPositions())
    }
}

const spacing = () => window.gameSize * SPACING_FACTOR;
const subSpacing = () => spacing() * SUBSPACING_FACTOR;

const drawTile = (i, j) => {
    window.tf.push()
    window.tf.translate(i * spacing(), j * spacing())
    fill(tileColor)
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
    for (let i = 0; i < walls.length; i++){
        for (let j = 0; j < walls.length; j++){
            const wall = walls[j][i]
            wall && drawWall(i,j, wall);
        }
    }
}

const drawPlayers = (playerPositions) => {
    Object.keys(playerPositions).forEach(key => {
        const pos = playerPositions[key]
        drawPlayer(pos.Col, pos.Row, key)
    })
}
