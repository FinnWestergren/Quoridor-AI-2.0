import { gameStateKey } from "../../shared/state/sharedState.js";

function getState() {
    return window[gameStateKey];
}

export const getWalls = () => getState()?.walls; 
export const getWallCounts = () => getState()?.playerWallCounts;
export const getPositions = () => getState()?.playerPositions;
export const getPossibleMoveActions = () => getState()?.possibleActions?.moveActions;
export const getPossibleWallActions = () => getState()?.possibleActions?.wallActions;