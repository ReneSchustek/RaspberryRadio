'use strict';

$(function () {
    $("#radiofavlist").sortable();
});

/**
 * Lautstärke verringern
 * */
function volumeDown() {

}

/**
 * Lautstärke erhöhen
 * */
function volumeUp() {
    var addVolumeStep = 0.05;


}

/**
 * Sender starten
 * */
function play() {

}


/**
 * Sender stoppen
 * */
function stop() {
    var addVolumeStep = 0.05;
}

/**
 * Liest das gewählte Land aus und lädt die Radiosender
 * @param {any} page Seite der Ergebnisse
 * */
function refreshRadioList(page) {

    if (page === undefined || page === null) {
        page = 0;
    }

    document.getElementById('nextpage').innerText = page + 1;
    document.getElementById('lastpage').innerText = page - 1;

    if (page === 0) {
        document.getElementById('lastpagespan').style.color = '#808080';
        document.getElementById('lastpagespan').onclick = '';
    }
    else {
        document.getElementById('lastpagespan').style.color = '#000000';
        document.getElementById('lastpagespan').onclick = 'lastPage()';
    }

    var country = document.getElementById('radioCountry').value;

    if (country.length < 3) { return; }

    var shortCountry = findShortCountry(country);

    if (shortCountry !== '') { loadCountryRadioStations(shortCountry, page); }
}

/**
 * Lädt die Radiosender
 * @param {any} shortCountry Länderkürzel
 * @param {any} page Seite der Ergebnisse
 */
function loadCountryRadioStations(shortCountry, page) {    
    $.ajax({
        url: '/api/radio/countrysearch/' + shortCountry + '/' + page,
        type: "GET",
        async: false,
        success: function (data) {           
            showStationTable(data, page);
        }
    });
}

/**
 * Sucht nach einem Sender
 * @param {any} page Seite
 */
function refreshRadioListBySearch(page) {

    if (document.getElementById('radioCountry').value === ''
        || document.getElementById('radioCountry').value === null
        || document.getElementById('radioCountry').value === undefined) {

        showModalMessage("Fehler", "Bitte ein Land auswählen");
    }

    var shortCountry = findShortCountry(document.getElementById('radioCountry').value);
    var searchString = document.getElementById('radioSearch').value;

    $.ajax({
        url: '/api/radio/search/' + searchString + '/' + shortCountry + '/' + page,
        type: "GET",
        async: false,
        success: function (data) {
            showStationTable(data, page);
        }
    });
}

/**
 * Aktualisiert die Senderliste bei der Ländersuche
 * @param {any} data empfangene Daten
 * @param {any} page Seite
 */
function showStationTable(data, page) {
    var tablebody = document.getElementById('radioStationTable');

    //Alte Daten löschen
    while (tablebody.firstChild) { tablebody.removeChild(tablebody.firstChild); }

    //Neue Daten hinzufügen
    if (data !== '' && data !== undefined && data !== null) {
           
        for (var x = 0; x < data.length; x++) {

            var inner = document.getElementById('radioStationTable').innerHTML;

            if (data[x].image.url !== null && data[x].image.url !== undefined && data[x].image.url !== '') {
                inner = inner + '<tr><td><span class="span-clickable" onclick="addRadioStation(' + data[x].id + ')"><img src="' + data[x].image.url + '" style="height: 25px"/></span></td>';
            }
            else {
                inner = inner + '<tr><td></td>';
            }
            inner = inner + '<td><span class="span-clickable" onclick="addRadioStation(' + data[x].id + ')">' + data[x].name + '</span></td>' +
                '<td><span class="fa fa-plus-circle span-clickable" onclick="addRadioStation(' + data[x].id + ')"></span></td></tr>';

            document.getElementById('radioStationTable').innerHTML = inner;
        }

        document.getElementById('pageDisplay').innerText = 'Seite ' + page + 1;
    }

    return;
}

/**
 * Zur letzten Seite zurückblättern
 * */
function lastPage() {
    var page = document.getElementById('lastpage').innerHTML;
    var country = document.getElementById('radioCountry').value;
    var shortCountry = findShortCountry(country);
    if (shortCountry !== '') { loadCountryRadioStations(shortCountry, page); }

    document.getElementById('lastpage').innerHTML = parseInt(page) - 1;
}

/**
 * Zur nächsten Seite weiterblättern
 * */
function nextPage() {    
    var page = document.getElementById('nextpage').innerHTML;
    var country = document.getElementById('radioCountry').value;
    var shortCountry = findShortCountry(country);

    if (document.getElementById('radioSearch').value.length > 2) {
        refreshRadioListBySearch(page);
    }
    else {
        if (shortCountry !== '') { loadCountryRadioStations(shortCountry, page); }
    }

    document.getElementById('nextpage').innerHTML = parseInt(page) + 1;
}

/**
 * Zur vorherigen Seite zurückblättern
 * */
function lastPageSearch() {
    var page = document.getElementById('lastpage').innerHTML;
    var country = document.getElementById('radioCountry').value;
    var shortCountry = findShortCountry(country);

    if (document.getElementById('radioSearch').value.length > 2) {
        refreshRadioListBySearch(page);
    }
    else {
        if (shortCountry !== '') { loadCountryRadioStations(shortCountry, page); }
    }

    document.getElementById('lastpage').innerHTML = parseInt(page) - 1;
}

/**
 * Findet die Kurzbeezeichnung des Landes
 * @param {any} country Ländername
 * @returns {any} Länderkürzel
 */
function findShortCountry(country) {
    //Eintrag in der Liste finden
    for (var i = 0; i < document.getElementById('selectCountries').options.length; i++) {
        if (document.getElementById('selectCountries').options[i].innerText !== country) { continue; }

        return document.getElementById('selectCountries').options[i].getAttribute('data-value');
    }
}

/**
 * Favoriten hinzufügen
 * @param {any} pos Position
 */
function addRadioFav(pos) {
    document.getElementById('saveposition').innerHTML = pos;
    $('#configModalRadioSearch').modal('show');
}

/**
 * Speichert einen Sender
 * @param {any} id Sender Id
 * @param {any} pos Favoriten-Position
 */
function addRadioStation(id) {
    //FavPos auslesen
    var favpos = document.getElementById('saveposition').innerHTML;

    $.ajax({
        url: '/api/radio/station/' + id + '/' + favpos,
        type: "GET",
        async: false,
        success: function (data) {
            saveFavPos(data, favpos);
        }
    });
}

function showError() {
    document.getElementById('responseModalAnswerLabel').innerHTML = 'Fehler';
    document.getElementById('').innerHTML = 'Der Sender konnte nicht gespeichert werden.';
    $('#responseModalAnswer').modal('show');
}

/**
 * Gibt die Rückmeldung vom Speichern aus
 * @param {any} data Rückmeldung
 */
function saveSuccess(data) {
    $('#waitingModal').modal('hide');

    //Modal Rückmeldung ausgeben
    if (data === 'true') {        
        document.getElementById('messageTitle').innerHTML = 'Gespeichert';
        document.getElementById('messageBody').innerHTML = '<span>' + jsonStations.name + ' wurde auf Position ' + favpos + ' gespeichert .</span >';

        $('#message').modal('show');
    } else {
        document.getElementById('messageTitle').innerHTML = 'Fehler';
        document.getElementById('messageBody').innerHTML = '<span>' + jsonStations.name + ' konnte nicht auf Position ' + favpos + ' gespeichert werden.</span >';

        $('#message').modal('show');
    }
}