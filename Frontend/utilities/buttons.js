
export const removeCustomButtons = (id) => {
    const filteredButtons = Object.keys(window.customButtons).filter(key => key.startsWith(id));
    filteredButtons.forEach(fId => {
        delete window.customButtons[fId];
    })
}

export const createCustomButton = ({x, y, w, h, displayText, txtSize, onClick, id, drawBackground, disableFunc}) => {
    const isMousedOver = () => {
        const satX = mouseX > x && mouseX < x + w;
        const satY = mouseY > y && mouseY < y + h;
        return satX && satY;
    }
    const isDisabled = () => !!disableFunc && disableFunc();
    window.customButtons = {...window.customButtons, [id]: {
        draw: () => {
            push();
            drawBackground ? drawBackground() : fill(255);
            translate(x,y);
            rect(0,0,w,h);
            fill(0);
            if (displayText) {
                textSize(txtSize ?? 20);
                textAlign(CENTER, CENTER);
                text(displayText, w * 0.5, h * 0.5);
            }
            if(isDisabled()){
                fill(255,255,255,200);
                rect(0,0,w,h);
            }
            pop();
        },
        onClick: () => !isDisabled() && onClick(),
        isMousedOver,
        isDisabled
    }};
}

window.addEventListener('click', async () => {
    Object.keys(window.customButtons).forEach(k => {
        const customButton = window.customButtons[k];
        customButton.isMousedOver() && customButton.onClick();
    });
});