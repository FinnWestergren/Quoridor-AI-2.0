import { updateState, getGameId, getComputerPlayer, getHumanPlayer, clearSelectedPlayer, clearState } from "../state/sharedState.js";
import { useMutex } from "../utilities/mutex.js" 

const API_MUTEX_ID = "API_MUTEX_ID"

const axiosInstance = () => axios.create({
    baseURL: `https://localhost/UncleTony/${window.controller}`,
    'Content-Type': 'application/json'
  });

export async function NewGame() {
    console.log("new game...")
    clearSelectedPlayer();
    clearState();
    return await useMutex(async () => {
        const newGame = await axiosInstance().get('NewGame');
        updateState(newGame.data);
        console.log("new game ready!")
        return true;
    }, API_MUTEX_ID)
}

export async function CommitAction(tileNumber) {
    console.log("commiting action...")
    return await useMutex(async () => {
        const data = {
            gameId: getGameId(),
            playerId: getHumanPlayer(),
            serializedAction: tileNumber
        }

        const resp = await axiosInstance().post('CommitAction',  data);
        updateState(resp.data);
        console.log("action commited!")
        return true;
    }, API_MUTEX_ID)
}

export async function GetMinimaxAction() {
    console.log("getting action...")
    return await useMutex(async () => {
        const params = {
            gameId: getGameId(),
            playerId: getComputerPlayer()
        };
        const resp = await axiosInstance().get('GetMinimaxMove',  { params });
        updateState(resp.data);
        console.log("action recieved!")

        return true;
    }, API_MUTEX_ID)
}
