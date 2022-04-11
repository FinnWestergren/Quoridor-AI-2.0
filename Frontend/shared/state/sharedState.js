export const gameStateKey = "state_key"
export const selectedPlayerKey = "selected_player"

export const GameStates = {
    PLAYER_SELECT: "PLAYER_SELECT",
    IN_PROGRESS: "IN_PROGRESS",
    GAME_OVER: "GAME_OVER"
}

function getState() {
    return window[gameStateKey];
}

export function updateState(newState) {
    console.log(newState);
    const pwc = newState.playerWallCounts
    const pp = newState.playerPositions
    if (pwc && typeof pwc === "string") newState.playerWallCounts = JSON.parse(pwc)
    if (pp && typeof pp === "string") newState.playerPositions = JSON.parse(pp.toLowerCase()) // THIS IS SKETCHY Row -> row, Col -> col TODO REFACTOR BACKEND
    window[gameStateKey] = newState;
}

export function selectPlayer(playerNumber) {
    console.log("selecting player", playerNumber)
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

export function clearState() {
    window[gameStateKey] = null
}

export const getHumanPlayer = () => window[selectedPlayerKey];
export const getComputerPlayer = () => getHumanPlayer() == getPlayerOne() ? getPlayerTwo() : getPlayerOne();

export const getWalls = () => getState()?.walls; 
export const getGameId = () => getState()?.gameId;
export const getPlayerOne = () => getState()?.playerOne; 
export const getPlayerTwo = () => getState()?.playerTwo;
export const getCurrentPlayer = () => getState()?.whosTurn;
export const getWinnerId = () => getState()?.winner;
export const getIsTie = () => getState()?.isTie;

export const getGameState = () => {
    if (!getState() || !getHumanPlayer()) return GameStates.PLAYER_SELECT
    if (getWinnerId() || getIsTie()) return GameStates.GAME_OVER
    return GameStates.IN_PROGRESS
}

