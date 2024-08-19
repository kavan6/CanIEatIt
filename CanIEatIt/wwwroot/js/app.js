
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


function getKeywords() {
    var filter = document.getElementById('input-keywords-filter');
    var children = filter.childNodes;

    for (var i = 1; i < children.length; i++) {
        keywords.push(children[i].textContent);
    }

    document.getElementById('hidden-keywords').value = keywords;

}

function createLabel(e) {

    if (e.value == ' ') {
        e.value = '';
        return null;
    }

    e.parentNode.appendChild(createLabelItem(e.value), e);

    e.value = '';

    function createLabelItem(text) {
        const item = document.createElement("div");
        item.setAttribute("class", "keyword-label");
        const span = `<span>${text}</span>`;
        const close = `<div class="fa fa-close" onclick="removeNodeLabel(this)"><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-x" viewBox="0 0 16 16">
  <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708"/>
</svg></div>`;

        text = text.substring(0, text.length - 1);

        const input = `<input type="hidden" value="${text}" name="SearchKeyWords"></input>`;

        item.innerHTML = span + close + input;
        return item;
    }
}

function removeNodeLabel(e) {
    e.parentNode.remove();
}

function temp() {

}

function wipeInputBox(e) {
    if (e.value == "Keywords..." || e.value == "Name...") {
        e.value = "";
    }
}

function fillInputBox(e) {
    if ((e == document.getElementById("input-keywords")) && ("" == document.getElementById("input-keywords").value)) {
        e.value = "Keywords...";
    } else if (e == document.getElementById("input-name") && ("" == document.getElementById("input-name").value)) {
        e.value = "Name...";
    }
}

function checkAll(e) {

    var ID = e.id;

    var boxes;

    switch (ID) {

        case "location-check-default":
            boxes = document.getElementsByClassName('location-check');
            break;
        case "family-check-default":
            boxes = document.getElementsByClassName('family-check');
            break;
        default:
            break;
    }

    if (e.checked) {

        for (var i = 0; i < boxes.length; i++) {
            boxes[i].checked = true;
        }

    } else {

        for (var i = 0; i < boxes.length; i++) {
            boxes[i].checked = false;
        }

    }
}
function setValueHidden(e) {
    var writeTo = e.id + "-hidden";
    var name = e.id.substring(6);

    document.getElementById(writeTo).value = name + "," + e.checked;
}
