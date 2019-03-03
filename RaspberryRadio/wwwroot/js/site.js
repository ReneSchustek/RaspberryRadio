'use strict';

/**
 * Beim Laden der Seite ausführen
 * */
$(function () {
    //Seitenspezifische Actions
    var curUrl = window.location.href.toLowerCase();
    //save?state=ok&saved=Deutsch
    if (curUrl.includes('configuration/save')) {
        var state = getUrlVars('state');
        var saved = getUrlVars('saved');
        var message, title = '';

        if (state === 'ok') {
            message = saved + ' wurde gespeichert.';
            title = 'Hinweis';
        }
        if (state === 'err') {
            message = saved + ' konnte nicht gespeichert werden.';
            title = 'Fehler';
        }

        showModalMessage(title, message);
    }
    setNavActive();
});

/**
 * Navbar anzeigen
 * */
function setNavActive() {
    //var url = window.location.pathname;

    //var elements = document.getElementsByClassName('nav-item');
    //var element;

    //for (var i = 0; i < elements.length; i++) {
    //    elements[i].classList.remove('active');
    //}

    //if (url.toLowerCase().includes('music')) {
    //    element = document.getElementById('music');
    //    element.classList.add('active');
    //} else if (url.toLowerCase().includes('config')) {
    //    element = document.getElementById('config');
    //    element.classList.add('active');
    //}
    //else {
    //    element = document.getElementById('index');
    //    element.classList.add('active');
    //}
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