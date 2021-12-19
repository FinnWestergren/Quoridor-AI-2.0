import { updateState, getGameId, getPlayerOne } from "../state/ticTacToe.state.js";

const instance = axios.create({
    baseURL: 'https://localhost/UncleTony/TicTacToe',
    'Content-Type': 'application/json'
  });

export async function NewGame() {
    const newGame = await instance.get('NewGame');
    updateState(newGame.data);
    return true;
}

export async function CommitAction(tileNumber) {
    const data = {
        gameId: getGameId(),
        playerId: getPlayerOne(),
        serializedAction: tileNumber
    }

    const resp = await instance.post('CommitAction',  data);
    updateState(resp.data);
    return true;
}

export async function GetMinimaxAction() {
    const params = {
        gameId: getGameId(),
        player: 'O'
    };
    const resp = await instance.get('GetMinimaxMove',  { params });
    updateState(resp.data);
    return true;
}