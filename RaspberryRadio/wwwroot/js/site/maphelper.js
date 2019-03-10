'use strict';

var mapmargin = 50;
$('#mapcontainer').css("height", ($(window).height() - mapmargin));
$(window).on("resize", resize);
resize();

function resize() {

    if ($(window).width() >= 980) {
        $('#mapcontainer').css("height", ($(window).height() - mapmargin));
        $('#mapcontainer').css("margin-top", 50);
    } else {
        $('#mapcontainer').css("height", ($(window).height() - (mapmargin + 12)));
        $('#mapcontainer').css("margin-top", -21);
    }

}