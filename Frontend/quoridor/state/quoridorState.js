import { gameStateKey, getHumanPlayer, updateState } from "../../shared/state/sharedState.js";
import { CommitAction } from "../../shared/api/sharedApi.js";

function getState() {
    return window[gameStateKey];
}

export const getWalls = () => getState()?.walls; 
export const getWallCounts = () => getState()?.playerWallCounts;
export const getPositions = () => getState()?.playerPositions;
export const getPossibleMoveActions = () => getState()?.possibleActions?.moveActions;
export const getPossibleWallActions = () => getState()?.possibleActions?.wallActions;

const optimisticMoveAction = async (move) => {
    const oldPositions = getPositions()
    const newPositions = {...oldPositions, [getHumanPlayer()]: move.cell}
    updateState({...getState(), playerPositions: newPositions})
    console.log(move)
    return await CommitAction(move.serializedAction);
}

const optimisticWallAction = async (wall) => {
    const oldWalls = getWalls()
    const wallCount = getWallCounts()[getHumanPlayer()]
    const newWalls = [...oldWalls]
    newWalls[wall.col][wall.row] = wall.orientation
    updateState({ ...getState(), walls: newWalls, playerWallCounts: {...getWallCounts(), [getHumanPlayer()]: wallCount - 1 }})
    return await CommitAction(wall.serializedAction);
}


export const optimisticallyCommitAction = async (action) => {
    // return await CommitAction(action.serializedAction)
    const oldState = getState();
    let success = false;
    if (action.type == "MOVE") {
        success = await optimisticMoveAction(action);
    }
    if (action.type == "WALL") {
        success = await optimisticWallAction(action);
    }

    if (!success){
        console.log("resetting")
        updateState(oldState)
    } 
    return success
}