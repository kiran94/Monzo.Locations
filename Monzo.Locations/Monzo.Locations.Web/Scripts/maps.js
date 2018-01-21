/* Google Maps Variable*/
var map;

var defaultLocation = {lat: 51.513413, lng: -0.088961}; 

/*
    Array of Markers added to the map
*/
var markers = []; 

/*
    Polygon drawn on the screen.
*/
var polygon; 

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

/* Adds a marker with the passed location to the map */
function addMarker(location, title)
{
	var infowindow = new google.maps.InfoWindow(title);
	var newMarker = new google.maps.Marker(
	{
		position : location,
		map : map,
        info : title,
        animation: google.maps.Animation.DROP
	});

	google.maps.event.addListener(newMarker, 'click', function() 
	{
		  infowindow.setContent(title);
		  infowindow.open(map, this);
		  infowindow.text(newMarker.info);
    });

    markers.push(newMarker);    
}

/*
  Generates a polygon
*/
function GeneratePolyline(polygonCoords)
{
  return new google.maps.Polyline({
            path: polygonCoords,
            geodesic: true,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2
        });
}


/* Clears the markers and polygon currently on the map */
function clearMarkers()
{
	if (markers)
	{
		for (var i = 0; i < markers.length; i++)
		{
			markers[i].setMap(null); 
        }
        
        markers = [];
	}
   
    if (polygon)
    {
        polygon.setMap(null);
    }    
}

/* Run after document load. */
$(function()
{   
    var retrieveButton = $('#retrievebutton'); 
    var resultInfo = $('#resultinfo'); 

    retrieveButton.click(function()
    {       
        retrieveButton.html("Loading..."); 
        retrieveButton.prop('disabled', true);
        clearMarkers();
        resultInfo.fadeIn(); 

        var bound = new google.maps.LatLngBounds(); 
        var polygonCoords = [];
        var startdate = $('#startdate').val(); 
        var enddate = $('#enddate').val(); 
        
        $.ajax(
        {
            url: 'http://127.0.0.1:8080/Home/GetTransactions',
            data: {startDate : startdate, endDate: enddate },
            success: function(data, status, xhr)
            {                            
                $.each(data.transactions, function(index, value)
                {
                    var address = value.merchant.address; 
                    var currentLocation = new google.maps.LatLng(address.Latitude, address.longitude); 

                    addMarker(currentLocation, value.merchant.name + "\n" + "(" + value.created + ")")

                    polygonCoords.push(currentLocation); 
                    bound.extend(currentLocation); 
                })

                polygon = GeneratePolyline(polygonCoords); 
                polygon.setMap(map); 
                map.setCenter(bound.getCenter()); 
			    map.fitBounds(bound); 

                retrieveButton.prop('disabled', false);               
                retrieveButton.html("Retrieve"); 

                resultInfo.css("color", "green"); 
                resultInfo.html("Retrieved " + data.transactions.length + " Transactions"); 

                resultInfo.fadeOut(3000, function()
                {
                    resultInfo.html(""); 
                });  
            },
            error: function(err, status, message)
            {                
                console.log(message);                 
                retrieveButton.prop('disabled', false);               
                retrieveButton.html("Retrieve"); 

                resultInfo.html(message); 
                resultInfo.css("color", "red"); 
            }, 
            dataType: 'json'
        });

        
    }); 
}); 

