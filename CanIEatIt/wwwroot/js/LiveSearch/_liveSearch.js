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
    let URLS = result.imageURLS;

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
                <img src="${URLS[i]}" class="card-img" />
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