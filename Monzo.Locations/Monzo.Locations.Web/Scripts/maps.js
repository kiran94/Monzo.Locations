/* Google Maps Variable*/
var map;

var defaultLocation = {lat: 51.513413, lng: -0.088961}; 


/* Initialises the map instance */
function initMap()
{
    map = new google.maps.Map(document.getElementById('map'), 
    {
        center: defaultLocation,
        scrollwheel: false,
        zoom: 12
    });
}