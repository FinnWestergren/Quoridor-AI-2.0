const gameStateKey = "uncletony_420_69"
const selectedPlayerKey = "selectedPlayerKey"
const gameState = "gameState"

const NOT_STARTED = "NOT_STARTED";
const IN_PROGRESS = "IN_PROGRESS";
const GAME_OVER = "GAME_OVER";

function getState() {
    return window[gameStateKey];
}

export function updateState(newState) {
    window[gameStateKey] = newState;
    if(getWinner() || !getBoard().find(t => !t.isOccupied)) {
        setGameOver();
    }
}

export function selectPlayer(character) {
    console.log(character)
    if (character == 'X') {
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

export const getBoard = () => getState()?.currentBoard; 
export const getGameId = () => getState()?.gameId;
export const getPlayerOne = () => getState()?.playerOne; 
export const getPlayerTwo = () => getState()?.playerTwo;
export const getWinner = () => { 
    var state = getState();
    var winner = state?.winner;
    if(!winner) return null;
    var playerOne = state?.playerOne;
    return winner === playerOne ? 'X' : 'O';
}


export const isGameNotStarted = () => window[gameState] == NOT_STARTED;
export const setGameNotStarted = () => {
    clearSelectedPlayer();
    window[gameState] = NOT_STARTED;
}
export const isGameInProgress = () => window[gameState] == IN_PROGRESS;
export const setGameInProgress = () => window[gameState] = IN_PROGRESS;
export const isGameOver = ()  => window[gameState] == GAME_OVER;
export const setGameOver = ()  => window[gameState] = GAME_OVER;
