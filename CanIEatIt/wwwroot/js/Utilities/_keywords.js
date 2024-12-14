
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