import { CommitAction } from "../../shared/api/sharedApi.js";
import { gameStateKey, getHumanPlayer, getPlayerOne, updateState } from "../../shared/state/sharedState.js";

function getState() {
    return window[gameStateKey];
}

export const getBoard = () => getState()?.currentBoard
const getHumanPlayerCharacter = () => getHumanPlayer() == getPlayerOne() ? 'X' : 'O'

export const optimisticallyCommitAction = async (serializedCell, board) => {
    if (board[serializedCell].occupiedBy != 0) return
    const state = getState()
    const newBoard = [...board]
    const newCell = {...board[serializedCell], printCell: getHumanPlayerCharacter(), occupiedBy: getHumanPlayer()}
    newBoard[serializedCell] = newCell
    updateState({...state, currentBoard: newBoard})
    const success = await CommitAction(serializedCell);
    if (!success) updateState({...state, currentBoard: board})
    return success
}