

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


