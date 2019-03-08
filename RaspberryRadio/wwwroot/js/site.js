'use strict';

/**
 * Beim Laden der Seite ausführen
 * */
$(function () {
    //Seitenspezifische Actions
    var curUrl = window.location.href;
    curUrl = curUrl.toLowerCase();

    //Navigation
    if (curUrl.includes('conf')) { mainMenueShow('areaConf'); }
    else { mainMenueShow('areaHome'); }

    //Modal
    //response=...&state=...&message= ... &area=...
    if (curUrl.includes('response')) {
        var response = getUrlVars('response');
        var state = getUrlVars('state');
        var message = getUrlVars('message');
        var area = getUrlVars('area');

        area = area.toLowerCase();

        var text, title = '';

        if (response === 'save') {
            if (state === 'ok') {
                title = 'Gespeichert';
                text = message + ' wurde gespeichert.';
            }
            else if (state === 'err') {
                title = 'Fehler';
                text = message + ' konnte nicht gespeichert werden.';
            }
        }       

        if (state === 'delete') {
            if (state === 'ok') {
                title = 'Gelöscht';
                text = message + ' wurde gelöscht.';
            }
            else if (state === 'err') {
                title = 'Fehler';
                text = message + ' konnte nicht gelöscht werden.';
            }
        }

        if (state === 'cancel') {
            if (state === 'ok') {
                title = 'Abbruch';
                text = message + ' wurde abgebrochen.';
            }
            else if (state === 'err') {
                title = 'Fehler';
                text = message + ' konnte nicht abgebrochen werden.';
            }
        }

        //Area anzeigen
        if (area === 'token') {
            mainMenueShow('areaConf');
            showConfArea('confAreaToken');
        }
        else if (area === 'weather') {
            mainMenueShow('areaConf');
            showConfArea('confAreaWeather');
        }
        else if (area === 'calendar') {
            mainMenueShow('areaConf');
            showConfArea('confAreaCalendar');
        }
        else if (area === 'radio') {
            mainMenueShow('areaConf');
            showConfArea('confAreaRadio');
        }
        else if (area === 'dailyscripture') {
            mainMenueShow('areaConf');
            showConfArea('confAreaDailyScripture');
        }
        else {
            mainMenueShow('areaHome');
            hideConfAreas();
        }

        showModalMessage(title, text);
    }
});

/**
 * Bereiche verstecken
 * */
function hideConfAreas() {
    document.getElementById('confAreaDailyScripture').style.display = 'none';
    document.getElementById('confAreaToken').style.display = 'none';
    document.getElementById('confAreaWeather').style.display = 'none';
    document.getElementById('confAreaCalendar').style.display = 'none';
    document.getElementById('confAreaRadio').style.display = 'none';

    document.getElementById('dailyLine').style.display = 'none';
    document.getElementById('tokenLine').style.display = 'none';
    document.getElementById('weatherLine').style.display = 'none';
    document.getElementById('calendarLine').style.display = 'none';
    document.getElementById('radioLine').style.display = 'none';
    document.getElementById('musicLine').style.display = 'none';
}

/**
 * Bereiche anzeigen
 * @param {any} name Bereichsname
 */
function showConfArea(name) {
    hideConfAreas();
    document.getElementById(name).style.display = 'block';

    name = name.toLowerCase();

    if (name.includes('token')) {
        document.getElementById('tokenLine').style.display = 'block';
    } else if (name.includes('weather')) {
        document.getElementById('weatherLine').style.display = 'block';
    } else if (name.includes('calendar')) {
        document.getElementById('calendarLine').style.display = 'block';
    } else if (name.includes('radio')) {
        document.getElementById('radioLine').style.display = 'block';
    } else if (name.includes('music')) {
        document.getElementById('musicLine').style.display = 'block';
    } else {
        document.getElementById('dailyLine').style.display = 'block';
    }
}

/**
 * Zeigt die Response Message
 * @param {any} title Titel
 * @param {any} message Inhalt
 */
function showModalMessage(title, message) {
    document.getElementById('responseModalAnswerLabel').innerText = title;
    document.getElementById('responseModalAnswerBody').innerHTML = message;

    $('#responseModalAnswer').modal('show');
}

/**
 * Steuerung des Menüs
 * */
function mainMenueHide() {
    
    document.getElementById('areaHome').style.display = 'none';
    document.getElementById('areaConf').style.display = 'none';

    document.getElementById('homeLine').style.display = 'none';
    document.getElementById('confLine').style.display = 'none';
}

/**
 * Bereich anzeigen
 * @param {any} name Name des Bereichs
 */
function mainMenueShow(name) {
    mainMenueHide();
    document.getElementById(name).style.display = 'block';

    name = name.toLowerCase();

    if (name.includes('conf')) { document.getElementById('confLine').style.display = 'block'; }
    else { document.getElementById('homeLine').style.display = 'block'; }
}

/**
 * Url auslesen
 * @param {any} name Name der Variabel, die ausgelesen werden soll
 * @returns {any} Inhalt der Variabel
 */
function getUrlVars(name) {
    var searchString = window.location.search.substring(1);
    var vars = searchString.split('&');

    for (var i = 0; i < vars.length; i++) {
        var varPair = vars[i].split('=');
        if (varPair[0] === name) { return varPair[1]; }
    }
}