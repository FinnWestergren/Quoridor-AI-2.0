const gameStateKey = "uncletony_420_69"
const selectedPlayerKey = "selectedPlayerKey"

export const GameStates = {
    PLAYER_SELECT: "PLAYER_SELECT",
    IN_PROGRESS: "IN_PROGRESS",
    GAME_OVER: "GAME_OVER"
}

function getState() {
    return window[gameStateKey];
}

export function updateState(newState) {
    const pwc = newState.playerWallCounts
    const pp = newState.playerPositions
    if (pwc) newState.playerWallCounts = JSON.parse(pwc)
    if (pp) newState.playerPositions = JSON.parse(pp)
    window[gameStateKey] = newState;
}

export function selectPlayer(playerNumber) {
    if (playerNumber == 1) {
        window[selectedPlayerKey] = getPlayerOne();
    }
    else {
        window[selectedPlayerKey] = getPlayerTwo();
    }
}

export function clearSelectedPlayer() {
    window[selectedPlayerKey] = null
}

export const getHumanPlayer = () => window[selectedPlayerKey];
export const getComputerPlayer = () => getHumanPlayer() == getPlayerOne() ? getPlayerTwo() : getPlayerOne();

export const getWalls = () => getState()?.walls; 
export const getWallCounts = () => getState()?.playerWallCounts; 
export const getPositions = () => getState()?.playerPositions; 
export const getGameId = () => getState()?.gameId;
export const getPlayerOne = () => getState()?.playerOne; 
export const getPlayerTwo = () => getState()?.playerTwo;
export const getCurrentPlayer = () => getState()?.whosTurn;
export const getGameState = () => {
    if (!getState() || !getHumanPlayer()) return GameStates.PLAYER_SELECT
    if (getWinner()) return GameStates.GAME_OVER
    return GameStates.IN_PROGRESS
}
export const getWinner = () => { 
    var state = getState();
    var winner = state?.winner;
    if(!winner) return null;
    var playerOne = state?.playerOne;
    return winner === playerOne ? 'Player One' : 'Player Two';
}
export const getWinnerId = () => getState()?.winner;

