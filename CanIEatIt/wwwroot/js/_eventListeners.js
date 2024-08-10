
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
    if (e.target.classList[1] == 'search-form-input') {
        fillInputBox(e.target);
    }
});
