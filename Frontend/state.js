const key = "uncletony_420_69"

export function getState() {
    const outString = localStorage.getItem(key);
    return JSON.parse(outString);
}

export function updateState(newState) {
    localStorage.setItem(key, JSON.stringify(newState));
}