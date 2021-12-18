const key = "uncleTony"

export function getState() {
    const outString = localStorage.getItem(key);
    return JSON.parse(outString);
}

export function updateState(newState) {
    localStorage.setItem(key, JSON.stringify(newState));
}