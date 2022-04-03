import { updateState, getGameId, getComputerPlayer, getHumanPlayer, clearSelectedPlayer } from "../state/ticTacToe.state.js";
import { useMutex } from "./mutex.js" 

const apiId = "api"
const instance = axios.create({
    baseURL: 'https://localhost/UncleTony/TicTacToe',
    'Content-Type': 'application/json'
  });

export async function NewGame() {
    console.log("new game...")
    return await useMutex(async () => {
        clearSelectedPlayer();
        const newGame = await instance.get('NewGame');
        updateState(newGame.data);
        console.log("new game ready!")
        return true;
    }, apiId)
}

export async function CommitAction(tileNumber) {
    console.log("commiting action...")
    return await useMutex(async () => {
        const data = {
            gameId: getGameId(),
            playerId: getHumanPlayer(),
            serializedAction: tileNumber
        }

        const resp = await instance.post('CommitAction',  data);
        updateState(resp.data);
        console.log("action commited!")
        return true;
    }, apiId)
}

export async function GetMinimaxAction() {
    console.log("getting action...")
    return await useMutex(async () => {
        const params = {
            gameId: getGameId(),
            playerId: getComputerPlayer()
        };
        const resp = await instance.get('GetMinimaxMove',  { params });
        updateState(resp.data);
        console.log("action recieved!")

        return true;
    }, apiId)
}
