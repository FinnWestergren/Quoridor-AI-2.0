const gameStateKey = "uncletony_420_69"
const selectedPlayerKey = "selectedPlayerKey"

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
export const getWinner = () => { 
    var state = getState();
    var winner = state?.winner;
    if(!winner) return null;
    var playerOne = state?.playerOne;
    return winner === playerOne ? 'Player One' : 'Player Two';
}

export const isBoardEmpty = () => getBoard() && !getBoard().some(t => t.isOccupied);
const isBoardfull = () => getBoard() && !getBoard().some(t => !t.isOccupied);
export const isGameOver = () => getWinner() || isBoardfull();