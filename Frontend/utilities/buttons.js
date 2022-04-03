window.customButtons = {};
window.buttonBoundaries = {};
window.buttonDisabled = {};
window.lastClicked = 0;

const DEBOUNCE_TIMER = 100

export const removeButton = (id) => delete window.customButtons[id];

export const drawButton = ({
    boundaries,
    content, 
    onClick, 
    id, 
    txtSize = 20, 
    isDisabled
}) => {
    setButtonBoundaries(id, boundaries)
    setButtonDisabled(id, isDisabled)
    if (!window.customButtons[id]) createButton(id, onClick)
    
    const { x, y, w, h } = boundaries
    window.tf.push()
    translate(x, y)
    rect(0, 0, w, h);
    fill(0);
    if (content) {
        textSize(txtSize);
        textAlign(CENTER, CENTER);
        text(content, w * 0.5, h * 0.5);
    }
    window.tf.pop()
}

const createButton = (id, onClick) => {
    window.customButtons = {...window.customButtons, 
        [id]: {
            draw,
            onClick
    }};
}

export const isButtonMousedOver = (id) => {
    const boundaries = getButtonBoundaries(id);
    if(!boundaries) return false;

    const { x, y, w, h } = boundaries
    const satX = mouseX > x && mouseX < x + w;
    const satY = mouseY > y && mouseY < y + h;
    return satX && satY && !isButtonDisabled(id);
}
const getButtonBoundaries = (id) => window.buttonBoundaries[id]
const setButtonBoundaries = (id, boundaries) => {
    window.buttonBoundaries[id] = { ...boundaries, x: boundaries.x + window.tf.x, y: boundaries.y + window.tf.y }
}

const isButtonDisabled = (id) => window.buttonDisabled[id]
const setButtonDisabled = (id, val) => {
    window.buttonDisabled[id] = val
}

export const isOnDebouceCooldown = () => Date.now() - window.lastClicked <= DEBOUNCE_TIMER

window.addEventListener('click', async () => {
    if (isOnDebouceCooldown()) return;
    Object.keys(window.customButtons).forEach(k => {
        const customButton = window.customButtons[k];
        if (isButtonMousedOver(k)) {
            customButton.onClick();
            window.lastClicked = Date.now();
        }
    });
});