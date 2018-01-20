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

    //markers.push(newMarker); 
    //addLegendItem(clusterNo, pinColor);
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

/* Initialises the map instance */
$(function()
{    
    $('#retrievebutton').click(function()
    {
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

                    addMarker(currentLocation, value.merchant.name + "(" + value.created + ")")

                    polygonCoords.push(currentLocation); 
                    bound.extend(currentLocation); 
                })

                var polygon = GeneratePolyline(polygonCoords); 
                polygon.setMap(map); 
                map.setCenter(bound.getCenter()); 
			    map.fitBounds(bound); 


            },
            error: function(err)
            {
                alert(err); 
            }, 
            dataType: 'json'
        });
//http://127.0.0.1:8080/Home/GetTransactions?startDate=20180101&endDate=20180110
    }); 
}); 

