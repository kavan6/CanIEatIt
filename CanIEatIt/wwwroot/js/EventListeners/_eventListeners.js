
document.addEventListener('DOMContentLoaded', function () {
    const capRange = document.getElementById('cap-range');
    setRangeAny(capRange);

    const stemRange = document.getElementById('stem-range');
    setRangeAny(stemRange);
});

window.addEventListener('keyup', function (e) {
    if (e.key == ' ' && e.target == document.getElementById("input-keywords")) {
        createLabel(e.target);
    }
});

window.addEventListener('focusin', function (e) {
    if (e.target.classList[1] == 'search-form-input') {
        wipeInputBox(e.target);
    }
});
window.addEventListener('focusout', function (e) {
    if (e.target.classList[0] == 'input') {
        fillInputBox(e.target);
    }
});

window.addEventListener('click', function (e) {
    if (e.target.id == 'dropdown-chevron') {
        const select = document.getElementById('searchbar-selector');
        // Create and dispatch a mouse event to simulate a click
        const event = new MouseEvent('mousedown', { bubbles: true, cancelable: true, view: window });
        select.dispatchEvent(event);
    }
});
