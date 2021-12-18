const base = 'https://localhost/UncleTony/TicTacToe'

export async function NewGame() {
    return await axios.get(`${base}/NewGame?board=xxxooo---`);
}