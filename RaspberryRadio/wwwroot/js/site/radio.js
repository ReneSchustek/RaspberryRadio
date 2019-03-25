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
    var xhttp = new XMLHttpRequest();
    xhttp.open('GET', '/api/radio/countrysearch/' + shortCountry + '/' + page, false);

    xhttp.send();

    var result = xhttp.response;


    var tablebody = document.getElementById('radioStationTable');

    //Alte Daten löschen
    while (tablebody.firstChild) { tablebody.removeChild(tablebody.firstChild); }

    //Neue Daten hinzufügen
    if (result !== '' && result !== undefined && result !== null) {
        try {
            var jsonStations = JSON.parse(result);

            for (var i = 0; i < jsonStations.length; i++) {

                var inner = document.getElementById('radioStationTable').innerHTML;

                if (jsonStations[i].image.url !== null && jsonStations[i].image.url !== undefined && jsonStations[i].image.url !== '') {
                    inner = inner + '<tr><td><img src="' + jsonStations[i].image.url + '" style="height: 25px"/></td>';
                }
                else {
                    inner = inner + '<tr><td></td>';
                }
                inner = inner + '<td>' + jsonStations[i].name + '</td>' +
                    '<td><span class="fa fa-plus-circle span-clickable" onclick="addRadioStation(' + jsonStations[i].id +')"></span></td></tr>';

                document.getElementById('radioStationTable').innerHTML = inner;
            }

            document.getElementById('pageDisplay').innerText = 'Seite ' + page + 1;
        }
        catch (e) {
            console.log(e);
        }
    }
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
 * Sucht nach einem Sender
 * @param {any} page Seite
 */
function refreshRadioListBySearch(page) {
   
    if (document.getElementById('radioCountry').value === ''
        || document.getElementById('radioCountry').value === null
        || document.getElementById('radioCountry').value === undefined) {

        showModalMessage("Fehler", "Bitte ein Land auswählen");
    }
    
    var xhttp = new XMLHttpRequest();
    
    xhttp.open('GET', '/api/radio/search/' + document.getElementById('radioSearch').value + '/' + findShortCountry(document.getElementById('radioCountry').value) + '/' + page, false);

    xhttp.send();

    var result = xhttp.response;

    var tablebody = document.getElementById('radioStationTable');

    //Alte Daten löschen
    while (tablebody.firstChild) { tablebody.removeChild(tablebody.firstChild); }

    //Neue Daten hinzufügen
    if (result !== '' && result !== undefined && result !== null) {
        try {
            var jsonStations = JSON.parse(result);

            for (var i = 0; i < jsonStations.length; i++) {

                var inner = document.getElementById('radioStationTable').innerHTML;

                if (jsonStations[i].image.url !== null && jsonStations[i].image.url !== undefined && jsonStations[i].image.url !== '') {
                    inner = inner + '<tr><td><img src="' + jsonStations[i].image.url + '" style="height: 25px"/></td>';
                }
                else {
                    inner = inner + '<tr><td></td>';
                }
                inner = inner + '<td>' + jsonStations[i].name + '</td>' +
                    '<td><span class="fa fa-plus-circle span-clickable" onclick="addRadioStation(' + jsonStations[i].id + ')"></span></td></tr>';

                document.getElementById('radioStationTable').innerHTML = inner;
            }

            document.getElementById('pageDisplay').innerText = 'Seite ' + page;
        }
        catch (e) {
            console.log(e);
        }
    }
}


/**
 * Favoriten hinzufügen
 * @param {any} pos Position
 */
function addRadioFav(pos) {
    $('#configModalRadioSearch').modal('show');
}