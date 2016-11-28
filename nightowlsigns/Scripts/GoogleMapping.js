﻿if (GBrowserIsCompatible()) {
    var startLat = $("#startLat").val();
    var startLng = $("#startLng").val();
    // Build up the map 
    var map = new GMap(document.getElementById("map"));
    map.setCenter(new google.maps.LatLng(startLat, startLng), 11);
    map.addControl(new GLargeMapControl());
    map.addControl(new GMapTypeControl());

    //var map = new google.maps.Map(document.getElementById('map'), {
    //    center: { lat: -27.5, lng: 153.1 },
    //    scrollwheel: false,
    //    zoom: 8
    //});

    



    var gmarkers = [];
    var htmls = [];
    var i = 0;

    // A function to create the marker and set up the event window
    function createMarker(point, name, html) {
      var marker = new GMarker(point);
    ////    var marker = new google.maps.Marker({
      //      map: map,
     //       position: point,
      //      title: html
     //   });


        GEvent.addListener(marker, "click", function() {
            marker.openInfoWindowHtml(html);
        });
        // save the info we need to use later for the side_bar
        gmarkers[i] = marker;
        htmls[i] = html;
        
        i++;
        return marker;
    }

    // This function picks up the click and opens the corresponding info window
    function myclick(i) {
        gmarkers[i].openInfoWindowHtml(htmls[i]);
    }

    // Process the Json file
    processJson = function(doc) {
        var jsonData = eval('(' + doc + ')');

        // Plot the markers
  

        for (var i = 0; i < jsonData.markers.length; i++) {
           var point = new GLatLng(jsonData.markers[i].lat, jsonData.markers[i].lng);
           // var point = { lat: jsonData.markers[i].lat, lng: jsonData.markers[i].lng };
            var marker = createMarker(point, jsonData.markers[i].label, jsonData.markers[i].html);
            map.addOverlay(marker);
        }
    }

    // Read from the hidden value and fetch the Json file.
    var markerUrl = document.getElementById("MarkerUrl").value;
    GDownloadUrl(markerUrl, processJson);

}

else {
    alert("Sorry, the Google Maps API is not compatible with this browser");
}