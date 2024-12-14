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