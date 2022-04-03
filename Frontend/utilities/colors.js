import { getHumanPlayer, getPlayerOne, getPlayerTwo } from "../state/ticTacToe.state.js"

export const p1Color = [255, 204, 0]
export const p2Color = [255, 100, 100]
export const getHumanColor = () => {
    if (getHumanPlayer() == getPlayerOne()) return p1Color
    if (getHumanPlayer() == getPlayerTwo()) return p2Color
} 
export const getCPUColor = () => {
    if (getHumanPlayer() == getPlayerTwo()) return p1Color
    if (getHumanPlayer() == getPlayerOne()) return p2Color
} 