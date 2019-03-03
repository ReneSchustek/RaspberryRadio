'use strict';

/* WebSocket Verbindung herstellen */
const connectionWeather = new signalR.HubConnectionBuilder()
    .withUrl('/openWeatherConfHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();
connectionWeather.start().catch(err => console.error(err.toString()));

/* ProgressBar empfangen */
connectionWeather.on('OnOpenWeatherRefresh', (message) => {
    showModalProgress(message);
    console.log("OnOpenWeatherRefresh: " + message);
});


/*  Seite fertig geladen */
$(document).ready(function () {

    refreshCitySelection();
    initializeMap();
    
});
    

/**
 * Zeigt die Rückmeldung für die Aktionen als Modal an
 * @param {any} title Titel der Meldung
 * @param {any} message Text der Meldung
 */
function showModalMessage(title, message) {

    document.getElementById("configModalAnswerLabel").innerHTML = title;
    document.getElementById("configModalAnswerBody").innerHTML = message;

    $("#configModalAnswer").modal("show");
}

/**
 * ---------------------------------------------------------------------------
 * DailyScripture Funktionen
 * ===========================================================================
 * */

/**
 * Zeige Formular für neue Sprache an
 * */
function addDailyScripture() {
    document.getElementById("dailyScriptureFormId").value = 0;
    document.getElementById("dailyScriptureFormLanguage").value = "";
    document.getElementById("dailyScriptureFormUrl").value = "";

    $("#configModalDailyScriptureForm").modal("show");
}

/**
 * Formular zum editieren füllen und anzeigen
 * @param {any} id ID der Sprache
 */
function editDailyScripture(id) {
    var url = "/api/config/dailyscripture/" + id;

    $.getJSON(url, function (data) {
        document.getElementById("dailyScriptureFormId").value = data.id;
        document.getElementById("dailyScriptureFormLanguage").value = data.language;
        document.getElementById("dailyScriptureFormUrl").value = data.url;
    });

   $("#configModalDailyScriptureForm").modal("show");
}

/**
 * ---------------------------------------------------------------------------
 * Token Funktionen
 * ===========================================================================
 * */

/**
 * Zeige Formular für neue Token an -> immer nur ein Eintrag möglich
 * */
function addToken() {
    document.getElementById("tokenFormId").value = 0;
    document.getElementById("tokenFormDirbleToken").value = "";
    document.getElementById("tokenFormOpenWeatherToken").value = "";

    $("#configModalTokenForm").modal("show");
}

/**
 * Formular zum editieren und füllen anzeigen
 * @param {any} id ID des Token
 */
function editToken(id) {
    var url = "/api/config/token/" + id;

    $.getJSON(url, function (data) {
        document.getElementById("tokenFormId").value = data.id;
        document.getElementById("tokenFormDirbleToken").value = data.DirbleToken;
        document.getElementById("tokenFormOpenWeatherToken").value = data.OpenWeatherToken;
    });

    $("#configModalTokenForm").modal("show");
}

/**
 * ---------------------------------------------------------------------------
 * OpenWeather Funktionen
 * ===========================================================================
 * */

/**
 * Progressbar anzeigen und anpassen
 * @param {any} message Daten für die ProgressBar
 */
function showModalProgress(message) {
    //Nachfrage schließen
    $("#configModalCitiesRefresh").modal("hide");

    //[0] = maximaler Wert, [1] = aktueller Wert, [2] = Prozent, [3] = Nachricht
    var valueArr = message.split(',');

    if (valueArr[3].includes('eingelesen')) {
        document.getElementById("showProgressBar").style.display = 'none';
        document.getElementById("showLoading").style.display = 'block';

        document.getElementById("loadingMessage").innerHTML = valueArr[3];
    }
    else {
        document.getElementById("showProgressBar").style.display = 'block';
        document.getElementById("showLoading").style.display = 'none';
    }

    //Fortschritt aktualisieren
    $("#configModalProgressBar")
        .css("width", valueArr[2])
        .attr("aria-valuenow", valueArr[1])
        .attr("aria-valuemax", valueArr[0]);

    document.getElementById("current-progress").innerHTML = "Fortschritt: " + valueArr[2] + " (" + valueArr[1] + "/" + valueArr[0] + ")";

    //Informationen senden
    var objDiv = document.getElementById("configModalOutput").innerHTML = valueArr[3];

    $("#configModalProgress").modal("show");

    //Modal am Ende schließen
    if (valueArr[3].includes('fertiggestellt')) {
        $("#configModalProgress").modal("hide");
    }

    if (valueArr[3].includes('Fehler')) {
        $("#configModalProgressButton").style.display = 'block';
    }
    else {
        $("#configModalProgressButton").style.display = 'none';
    }
}

/**
 * Nachfragen, ob Einlesen gestartet werden soll
 * */
function askForCityRefresh() {
    $("#configModalCitiesRefresh").modal("show");
}

/**
 * Zeigt Modal für neue Städte an
 * */
function addOpenWeather() {
    $("#configOpenWeatherCityForm").modal("show");
}

/**
 * Liest das gewählte Land aus und selectiert die Städte
 * */
function refreshCitySelection() {
    var country = document.getElementById("openWeatherCountry").value;

    if (country.length < 3) { return; }

    var shortCountry = '';

    //Eintrag in der Liste finden
    for (var i = 0; i < document.getElementById("selectCountries").options.length; i++) {
        if (document.getElementById("selectCountries").options[i].innerText !== country) { continue; }

        shortCountry = document.getElementById("selectCountries").options[i].getAttribute("data-value");
    }    

    if (shortCountry !== '') { loadOpenWeatherCities(shortCountry); }
}


/**
 * Suche nach Städten
 * @param {any} shortCountry Länderkürzel
 * */
function loadOpenWeatherCities(shortCountry) {
    var xhttp = new XMLHttpRequest();
    xhttp.open("GET", "/api/config/openWeather/search/" + shortCountry, false);
       
    xhttp.send();

    var result = xhttp.response;

    if (result !== '' && result !== undefined && result !== null) {
        var jsonCities = JSON.parse(result);

        var list = document.getElementById('selectCities');

        while (list.firstChild) { list.removeChild(list.firstChild); }

        for (var i = 0; i < jsonCities.length; i++) {
            var option = document.createElement('option');
            option.setAttribute("data-value", jsonCities[i].id);
            option.setAttribute("data-lon", jsonCities[i].lon);
            option.setAttribute("data-lat", jsonCities[i].lat);
            option.setAttribute("data-cityid", jsonCities[i].cityId);

            var shortLon = jsonCities[i].lon.toString();
            var shortLat = jsonCities[i].lat.toString();

            if (shortLat.length > 6) { shortLat = shortLat.substring(0, 6); }
            if (shortLon.length > 6) { shortLon = shortLon.substring(0, 6); }

            option.innerText = jsonCities[i].name + " (" + shortLat + ", " + shortLon + ")";
            list.appendChild(option);
        }
    }    
}

/**
 * OpenStreetMap mit Leaflet
 * */
var map, markerIcon;

/**
 * Karte initializieren
 * */
function initializeMap() {    
    map = L.map('mapcontainer').setView([52.520008, 13.404954], 8);

    L.tileLayer('https://{s}.tile.openstreetmap.de/tiles/osmde/{z}/{x}/{y}.png', {
        'attribution': 'Kartendaten &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        'useCache': true
    }).addTo(map);
}

/**
 * Karte aktualisieren und Id setzen
 * @param {any} lat Latitude
 * @param {any} lon Longitude
 */
function refreshCityMap(lat = 0, lon = 0) {

    if (document.getElementById("openWeatherCity").value.length < 3) { return; }


    //Lon / Lat auslesen
    var cityId;

    if (lon === 0 || lat === 0) {
        for (var i = 0; i < document.getElementById("selectCities").options.length; i++) {
            if (document.getElementById("selectCities").options[i].innerText !== document.getElementById("openWeatherCity").value) { continue; }

            lat = document.getElementById("selectCities").options[i].getAttribute("data-lat");
            lon = document.getElementById("selectCities").options[i].getAttribute("data-lon");
            cityId = document.getElementById("selectCities").options[i].getAttribute("data-cityid");
            
            break;
        }
    }

    if (lon === undefined || lat === undefined) { return; }
    if (lon === 0 && lat === 0) { return; }
    if (lon === null || lat === null) { return; }

    document.getElementById("openWeatherCityId").value = cityId;

    map.eachLayer(function (layer) {
        map.removeLayer(layer);
    });

    map.remove();
    map = null;

    var mapMarker = L.icon({
        iconUrl: '/images/marker-icon.png',
        shadowUrl: '/images/marker-shadow.png',
        iconSize: [38, 95],
        shadowSize: [50, 64],
        iconAnchor: [22, 94],
        shadowAnchor: [4, 62]
    });

    lon = lon.replace(',', '.');
    lat = lat.replace(',', '.');
    

    map = L.map('mapcontainer').setView([lat, lon], 12);        

    L.tileLayer('https://{s}.tile.openstreetmap.de/tiles/osmde/{z}/{x}/{y}.png', {
        'attribution': 'Kartendaten &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        'useCache': true
    }).addTo(map);       

    L.marker([lat, lon], { icon: mapMarker }).addTo(map);
    setTimeout(function () {
        map.invalidateSize();
    }, 100);
}

$('#configOpenWeatherCityForm').on('shown.bs.modal', function () {
    setTimeout(function () {
        map.invalidateSize();
    }, 100);
});

/**
 * ---------------------------------------------------------------------------
 * Calender Funktionen
 * ===========================================================================
 * */


function addCalender() {
    $("#configModalCalenderForm").modal("show");
}

function editCalender(id) {
    var url = "/api/config/calenders/" + id;

    $.getJSON(url, function (data) {
        document.getElementById("calenderFormId").value = data.id;
        document.getElementById("calenderFormName").value = data.name;
        document.getElementById("calenderFormUsername").value = data.username;
        document.getElementById("calenderFormPassword").value = data.password;
        document.getElementById("calenderFormUrl").value = data.url;
    });

    $("#configModalCalenderForm").modal("show");
}