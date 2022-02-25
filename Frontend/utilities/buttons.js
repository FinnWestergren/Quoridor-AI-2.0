
export const removeCustomButton = (id) => delete window.customButtons[id];

export const createCustomButton = ({x, y, w, h, displayText, txtSize, onClick, id, drawBackground, disableFunc}) => {
    const isMousedOver = () => {
        const satX = mouseX > x && mouseX < x + w;
        const satY = mouseY > y && mouseY < y + h;
        return satX && satY;
    }
    window.customButtons = {...window.customButtons, [id]: {
        draw: () => {
            push();
            drawBackground && drawBackground();
            translate(x,y);
            rect(0,0,w,h);
            fill(0);
            if (displayText) {
                textSize(txtSize ?? 20);
                textAlign(CENTER, CENTER);
                text(displayText, w * 0.5, h * 0.5);
            }
            pop();
        },
        onClick,
        isMousedOver,
        isDisabled: () => !!disableFunc && disableFunc()
    }};
}

window.addEventListener('click', async () => {
    Object.keys(window.customButtons).forEach(k => {
        const customButton = window.customButtons[k];
        customButton.isMousedOver() && !customButton.isDisabled() && customButton.onClick();
    });
});