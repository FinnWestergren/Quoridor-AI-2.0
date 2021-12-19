const key = "uncletony_420_69"

function getState() {
    const outString = localStorage.getItem(key);
    return JSON.parse(outString);
}

export function updateState(newState) {
    localStorage.setItem(key, JSON.stringify(newState));
}

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

