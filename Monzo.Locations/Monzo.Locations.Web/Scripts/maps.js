/* Google Maps Variable*/
var map;

/* Default location to set the map. */
var defaultLocation = { lat: 51.513413, lng: -0.088961 };

/*
    Array of Markers added to the map
*/
var markers = [];

/*
    Polygon drawn on the screen.
*/
var polygon;

/* Date Regular Expression */
var dateReg = RegExp("^(19[5-9][0-9]|20[0-4][0-9]|2050)(0?[1-9]|1[0-2])(0?[1-9]|[12][0-9]|3[01])$");

/* Initialises the map instance */
function initMap() {
    map = new google.maps.Map(document.getElementById('map'),
        {
            center: defaultLocation,
            scrollwheel: false,
            zoom: 12
        });
}

/* Adds a marker with the passed location to the map */
function addMarker(location, title) {
    var infowindow = new google.maps.InfoWindow(title);
    var newMarker = new google.maps.Marker(
        {
            position: location,
            map: map,
            info: title,
            animation: google.maps.Animation.DROP
        });

    google.maps.event.addListener(newMarker, 'click', function () {
        infowindow.setContent(title);
        infowindow.open(map, this);
        infowindow.text(newMarker.info);
    });

    markers.push(newMarker);
}

/*
  Generates a polygon
*/
function GeneratePolyline(polygonCoords) {
    return new google.maps.Polyline({
        path: polygonCoords,
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2
    });
}


/* Clears the markers and polygon currently on the map */
function clearMarkers() {
    if (markers) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }

        markers = [];
    }

    if (polygon) {
        polygon.setMap(null);
    }
}

/* Run after document load. */
$(function () {
    var retrieveButton = $('#retrievebutton');
    var resultInfo = $('#resultinfo');
    var startDate = $('#startdate');
    var endDate = $('#enddate');
    var table = $('#transactiontable');

    /* Disables the button */
    function disableButton() {
        retrieveButton.html("Loading...");
        retrieveButton.prop('disabled', true);
    }

    /* Reset the button to default. */
    function resetButton() {
        retrieveButton.prop('disabled', false);
        retrieveButton.html("Retrieve");
    }

    /* Set the result information panel with a message and colour. */
    function setResultInfo(message, color) {
        resultInfo.css("color", color);
        resultInfo.html(message);

        resultInfo.fadeOut(5000, function () {
            resultInfo.html("");
        });
    }

    /*  
        Generates the table of transactions. 
    */
    function generateTable(transactions) {
        var allrows =
            `
            <thead>
            <tr>
                <th>Merchant</th>
                <th>Amount</th>
                <th>Date</th>
                <th>Latitude</th>
                <th>Longitude</th>
            </tr>
            </thead>
            <tbody>
        `;

        $.each(transactions, function (index, value) {
            var currentRow =
                `<tr>
                <td>` + value.merchant.name + `</td>
                <td>` + Math.abs(value.amount / 100) + `</td>
                <td>` + value.created + `</td>
                <td>` + value.merchant.address.Latitude + `</td>
                <td>` + value.merchant.address.longitude + `</td>
            </tr>`

            allrows += currentRow;
        });

        allrows += "</tbody>"

        table.html(allrows);
    }

    /* When the retrieve button is clicked, make an AJAX call to the server to load the transactions. */
    retrieveButton.click(function () {
        disableButton();
        clearMarkers();
        resultInfo.fadeIn();

        var bound = new google.maps.LatLngBounds();
        var polygonCoords = [];
        var startdate = startDate.val();
        var enddate = endDate.val();

        if (!dateReg.test(startdate)) {
            resetButton();
            setResultInfo("Invalid start date", "red");
            return;
        }

        if (!dateReg.test(enddate)) {
            resetButton();
            setResultInfo("Invalid end date", "red");
            return;
        }

        $.ajax(
            {
                url: 'http://127.0.0.1:8080/Home/GetTransactions',
                data: { startDate: startdate, endDate: enddate },
                success: function (data, status, xhr) {
                    $.each(data.transactions, function (index, value) {
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

                    resetButton();
                    setResultInfo("Retrieved " + data.transactions.length + " Transactions", "green");

                    generateTable(data.transactions);
                },
                error: function (err, status, message) {
                    console.log(message);
                    resetButton();
                    setResultInfo(message, "red");
                },
                dataType: 'json'
            });
    });
}); 