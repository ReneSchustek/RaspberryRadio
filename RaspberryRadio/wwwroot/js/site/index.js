'use strict';

var $mq;

/**
 * WebSocket Verbindungen herstellen
 * */

/* Tagestext WebSocket Verbindung */
const dailyScriptuerConnection = new signalR.HubConnectionBuilder()
    .withUrl('/dailyScriptureHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

dailyScriptuerConnection.start().catch(err => console.error(err.toString()));

/* Aktuelles Wetter WebSocket Verbindung */
const currentWeatherConnection = new signalR.HubConnectionBuilder()
    .withUrl('/currentWeatherHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

currentWeatherConnection.start().catch(err => console.error(err.toString()));

/* Wettervorhersage WebSocket Verbindung */
const forecastWeatherConnection = new signalR.HubConnectionBuilder()
    .withUrl('/forecastWeatherHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

forecastWeatherConnection.start().catch(err => console.error(err.toString()));

/* Kalendar WebSocket Verbindung */
const calendarConnection = new signalR.HubConnectionBuilder()
    .withUrl('/calendarHub')
    .configureLogging(signalR.LogLevel.Information)
    .build();

calendarConnection.start().catch(err => console.error(err.toString()));

/**
 * Tagestext empfangen
 * */
dailyScriptuerConnection.on("OnDailyScripturePublish", (dailyScripture) => {

    var dailyText = JSON.parse(dailyScripture);

    for (var i = 0; i < dailyText.length; i++) {
        document.getElementById("daily-title-" + dailyText[i].Language).innerText = dailyText[i].Title;
        document.getElementById("daily-text-" + dailyText[i].Language).innerText = dailyText[i].Text;
        document.getElementById("dailyFullTextBody-" + dailyText[i].Language).innerText = dailyText[i].Comment;
    }
});

/**
 * Termine empfangen
 * */
calendarConnection.on("OnEventPublish", (calendarEvents) => {
    console.log(calendarEvents);
    document.getElementById("calendarList").innerHTML = calendarEvents;

    $(".marquee-1").trigger('mouseenter');
    $(".marquee-1").trigger('mouseleave');
});


/**
 * Aktuelles Wetter empfangen
 * */
currentWeatherConnection.on("OnCurrentWeatherPublish", (currentWeather) => {

    var current = JSON.parse(currentWeather);

    for (var i = 0; i < current.length; i++) {
        document.getElementById("current-title-" + current[i].name).innerText = current[i].name;
        document.getElementById("current-image-" + current[i].name).src = "https://openweathermap.org/img/w/" + current[i].icon + ".png";
        document.getElementById("current-image-" + current[i].name).alt = current[i].description;
        document.getElementById("current-temp-" + current[i].name).innerText = current[i].temp + "&deg;C";
        document.getElementById("current-description-" + current[i].name).innerText = current[i].description;

        //Modal
        document.getElementById("currentWeatherTitle-" + current[i].name).innerText = current[i].name;
        document.getElementById("currentWeatherSunrise-" + current[i].name).innerText = current[i].sunrise;
        document.getElementById("currentWeatherSunset-" + current[i].name).innerText = current[i].sunset;
        document.getElementById("currentWeatherPressure-" + current[i].name).innerText = current[i].pressure;
        document.getElementById("currentWeatherHumidity-" + current[i].name).innerText = current[i].humidity;
        document.getElementById("currentWeatherSpeed-" + current[i].name).innerText = current[i].speed + " km/h";
        document.getElementById("currentWeatherDeg-" + current[i].name).innerText = current[i].deg;
        document.getElementById("currentWeatherCloudDescription-" + current[i].name).innerText = current[i].cloudinessdescr;
        document.getElementById("currentWeatherCloudPercent-" + current[i].name).innerText = current[i].cloudinessperc;
    }
});

/**
 * Wettervorhersage empfangen
 * */
forecastWeatherConnection.on("OnForecastWeatherPublish", (forecastWeather) => {
    document.getElementById("forecastWeatherList").innerHTML = forecastWeather;
});


/**
 * Wetter Modal laden
 * @param {any} cityName Städtename
 */
function showCurrentWeatherModal(cityName) {
    $("#currentWeatherInfo-" + cityName).modal("show");
}

/**
 * Wettervorhesage Ticker starten
 * */
$.simpleTicker($("#forecastWeather"), {
    'effectType': 'roll',
    speed: 2000,
    delay: 6000,
    easing: 'swing'
});

/**
 * Terminticker
 * */
$('.simple-marquee-container').SimpleMarquee({
    duration: 80000
});

$(".marquee-1").trigger('mouseenter');
$(".marquee-1").trigger('mouseleave');


/**
 * Vollbild betreten
 * */
$(enterFull());

/**
 * Aktuelles Datum und KW erzeugen
 * */
function currentDate() {
    var today = new Date();

    var months = ["Januar", "Februar", "März",
        "April", "Mai", "Juni",
        "Juli", "August", "September",
        "Oktober", "November", "Dezember"];
    var weekNumber = weeknumber(today);

    var dateString = today.getDate() + '. ' + months[today.getMonth()] + ' ' + today.getFullYear() + ' (KW ' + weekNumber + ')';

    document.getElementById("currentDate").innerText = dateString;
}

/**
 * KW erzeugen
 * @param {any} d Datum
 * @returns {any} Wochennummer
 */
var weeknumber = function (d) {
    d = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate()));
    d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
    var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
    return Math.ceil((((d - yearStart) / 86400000) + 1) / 7);
};

/**
 * Uhr Inhalt
 * */
function clock() {

    var months = ["Januar", "Februar", "März",
        "April", "Mai", "Juni",
        "Juli", "August", "September",
        "Oktober", "November", "Dezember"];

    var now = new Date();

    var day = now.getDate();
    var month = now.getMonth();
    var year = now.getFullYear();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var seconds = now.getSeconds();
    var day0 = ((day < 10) ? "0" : "");
    var hours0 = ((hours < 10) ? "0" : "");
    var minutes0 = ((minutes < 10) ? "0" : "");
    var seconds0 = ((seconds < 10) ? "0" : "");

    var weekNumber = weeknumber(now);
    var weekNumber0 = ((weekNumber < 10) ? "0" : "");

    var innerClockText = "<span class=\"clock-great\">" + hours0 + hours + ":" + minutes0 + minutes + "</span>" +
        "<span class=\"clock-small\">" + seconds0 + seconds + "</span>" +
        "<br>" +
        "<span class=\"date\">" + day0 + day + ". " + months[month] + " " + year + "&nbsp;&nbsp;(KW " + weekNumber0 + weekNumber + ")</span>";

    document.getElementById("clock").innerHTML = innerClockText;
}

/**
 * Uhr Interval laden
 * */
setInterval(function () {
    clock();
}, 500);

/**
 * Vollbild anzeigen und Elemente anpassen
 * */
function enterFull() {
    enterFullscreen(document.documentElement);
    document.getElementById("startFull").style.visibility = "visible";
    document.getElementById("exitFull").style.visibility = "hidden";
}

/**
 * Vollbild verlassen und Elemente anpassen
 * */
function exitFull() {
    exitFullscreen();
    document.getElementById("startFull").style.visibility = "hidden";
    document.getElementById("exitFull").style.visibility = "visible";
}

/**
 * Vollbild anzeigen
 * @param {any} element Element, das Vollbild sein soll
 */
function enterFullscreen(element) {
    if (element.requestFullscreen) {
        element.requestFullscreen();
    } else if (element.mozRequestFullScreen) {
        element.mozRequestFullScreen();
    } else if (element.msRequestFullscreen) {
        element.msRequestFullscreen();
    } else if (element.webkitRequestFullscreen) {
        element.webkitRequestFullscreen();
    }
}

/**
 * Vollbild verlassen
 * */
function exitFullscreen() {
    if (document.exitFullscreen) {
        document.exitFullscreen();
    } else if (document.mozCancelFullScreen) {
        document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) {
        document.webkitExitFullscreen();
    }
}