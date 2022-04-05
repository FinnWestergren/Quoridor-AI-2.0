import { getComputerPlayer } from "../state/quoridor.state.js"
import { getHumanPlayer, getPlayerOne, getPlayerTwo } from "../state/ticTacToe.state.js"

export const p1Color = "#e07a5f"
export const p2Color = "#3d405b"
export const wallColor = "#81b29a"
export const tileColor = "#f2cc8f"
export const ghostTileColor = "#f2cc8f55"

export const getPlayerColor = (playerId) => {
    if (playerId == getPlayerOne()) return p1Color
    if (playerId == getPlayerTwo()) return p2Color
} 

export const getHumanColor = () => getPlayerColor(getHumanPlayer())
export const getCPUColor = () => getPlayerColor(getComputerPlayer())