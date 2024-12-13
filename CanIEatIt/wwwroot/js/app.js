
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
    if (e.value == "Keywords..." || e.value == "Name..." || e.value == "Edible Keyword(s)..." || e.value == "Cap Keyword(s)..." || e.value == "Stem Keyword(s)..."
        || e.value == "Gill Keyword(s)..." || e.value == "Spore Keyword(s)..." || e.value == "Microscopic Keyword(s)..." || e.value == "Note Keyword(s)..."
        || e.value == "Search mushrooms...") {
        e.value = "";
    }
}

function fillInputBox(e) {

    if ((e == document.getElementById("input-keywords")) && ("" == document.getElementById("input-keywords").value)) {
        e.value = "Keywords...";

    } else if (e == document.getElementById("input-name") && ("" == document.getElementById("input-name").value)) {
        e.value = "Name...";
    }
    else if (e == document.getElementById("input-edible") && ("" == document.getElementById("input-edible").value)) {
        e.value = "Edible Keyword(s)...";
    }
    else if (e == document.getElementById("input-cap") && ("" == document.getElementById("input-cap").value)) {
        e.value = "Cap Keyword(s)...";
    }
    else if (e == document.getElementById("input-stem") && ("" == document.getElementById("input-stem").value)) {
        e.value = "Stem Keyword(s)...";
    }
    else if (e == document.getElementById("input-gill") && ("" == document.getElementById("input-gill").value)) {
        e.value = "Gill Keyword(s)...";
    }
    else if (e == document.getElementById("input-spore") && ("" == document.getElementById("input-spore").value)) {
        e.value = "Spore Keyword(s)...";
    }
    else if (e == document.getElementById("input-micro") && ("" == document.getElementById("input-micro").value)) {
        e.value = "Microscopic Keyword(s)...";
    }
    else if (e == document.getElementById("searchbar-search") && ("" == document.getElementById("searchbar-search").value)) {
        e.value = "Search mushrooms...";
    }
    //else if (e == document.getElementById("input-note") && ("" == document.getElementById("input-note").value)) {
    //    e.value = "Note Keyword(s)...";
    //}
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

function setRangeAny(e) {
    if (e.value > 0) {
        e.previousElementSibling.value = e.value + 'cm';
    } else {
        e.previousElementSibling.value = 'Any';
    }
}

var chevronDown = false;
function changeChevron() {
    let e = document.getElementById('chevron-path');

    if (!chevronDown) {
        chevronDown = true;
        e.setAttribute('d', "M7.646 4.646a.5.5 0 0 1 .708 0l6 6a.5.5 0 0 1-.708.708L8 5.707l-5.646 5.647a.5.5 0 0 1-.708-.708z");
    } else {
        chevronDown = false;
        e.setAttribute('d', "M1.646 4.646a.5.5 0 0 1 .708 0L8 10.293l5.646-5.647a.5.5 0 0 1 .708.708l-6 6a.5.5 0 0 1-.708 0l-6-6a.5.5 0 0 1 0-.708");
    }
}

function grabSearchValue(e) {
    var val = e.value;

    // Perform an AJAX request to the server
    fetch('/Mushrooms/Search?searchValue=' + encodeURIComponent(val), {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json(); // Assuming the server returns JSON
    })
    .then(data => {
        updateMushroomCards(data);
        console.log(data); // Handle the response data (e.g., update your UI)
    })
    .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });
}

function updateMushroomCards(result) {
    // Clear existing cards
    const mushroomSlot1 = document.getElementById("mushroom-slot-1");
    const mushroomSlot2 = document.getElementById("mushroom-slot-2");
    const mushroomSlot3 = document.getElementById("mushroom-slot-3");

    mushroomSlot1.innerHTML = '';  // Clear existing cards
    mushroomSlot2.innerHTML = ''; 
    mushroomSlot3.innerHTML = '';

    let mushrooms = result.mushrooms;

    if (!mushrooms) return;

    for (var i = 0; i < mushrooms.length; i++) {

        //// Create mushroom card ////

        const card = document.createElement("a");
        card.classList.add("card-link");
        card.href = `/Mushrooms/Information/${mushrooms[i].id}`;

        const cardDiv = document.createElement("div");
        cardDiv.classList.add("card", "display-card", "text-black", "mb-3");

        const cardHeader = document.createElement("div");
        cardHeader.classList.add("card-header");
        cardHeader.innerHTML = `
            ${mushrooms[i].name}
            <div class="float-end">
                <span class="badge ${mushrooms[i].edible ? 'badge-success' : 'badge-danger'}">${mushrooms[i].edible ? 'Edible' : 'Inedible'}</span>
            </div>
        `;

        const cardBody = document.createElement("div");
        cardBody.classList.add("card-body");
        cardBody.innerHTML = `
            <div class="card-image mb-3">
                <img src="${mushrooms[i].imageUrl}" class="card-img" />
            </div>
            <p class="card-text mb-1"><b>Family:</b> ${mushrooms[i].family}</p>
            <p class="card-text mb-1"><b>Location:</b> ${mushrooms[i].location}</p>
            <p class="card-text mb-1"><b>Dimensions:</b> Cap ${mushrooms[i].capDiameter} diameter, Stem ${mushrooms[i].stemHeight} tall</p>
            <p class="card-text mb-1"><b>Edible:</b> ${mushrooms[i].edible ? 'Yes' : 'No'}</p>
            <p class="card-text mb-1"><b>Cap:</b> ${mushrooms[i].capDescription}</p>
            <p class="card-text"><b>Stem:</b> ${mushrooms[i].stemDescription}</p>
        `;

        cardDiv.appendChild(cardHeader);
        cardDiv.appendChild(cardBody);
        card.appendChild(cardDiv);

        /////

        if (i % 3 == 0) {
            mushroomSlot1.appendChild(card);
        } else if (i % 3 == 1) {
            mushroomSlot2.appendChild(card);

        } else if (i % 3 == 2) {
            mushroomSlot3.appendChild(card);

        }
    }
}

