
const TOKEN = "pk.eyJ1Ijoic3N1bjAwMzUiLCJhIjoiY2tmeHd6ODdtMXhzdjJxbXRlMWNvdGFlbSJ9.en2VoCGA9navhRFRLVOM0A";
var locations = [];
// The first step is obtain all the latitude and longitude from the HTML
// The below is a simple jQuery selector
$(".coordinates").each(function () {
    var longitude = $(".longitude", this).text().trim();
    var latitude = $(".latitude", this).text().trim();
    var description = $(".description", this).text().trim();
    // Create a point data structure to hold the values.
    var point = {
        "latitude": latitude,
        "longitude": longitude,
        "description": description
    };
    // Push them all into an array.
    locations.push(point);
});
var data = [];
for (i = 0; i < locations.length; i++) {
    var feature = {
        "type": "Feature",
        "properties": {
            "description": locations[i].description,
            "icon": "circle-15"
        },
        "geometry": {
            "type": "Point",
            "coordinates": [locations[i].longitude, locations[i].latitude]
        }
    };
    data.push(feature)
}
mapboxgl.accessToken = TOKEN;
var instructions = document.getElementById('instructions');
var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v10',
    zoom: 11,
    center: [locations[0].longitude, locations[0].latitude]
});
var geocoder = new MapboxGeocoder({
    accessToken: mapboxgl.accessToken
});


map.on('load', function () {

    map.loadImage(
        '../Image/locationIcon.png',
        function (error, image) {
            if (error) throw error;
            // Replace the location marker
            map.addImage('icon', image);
            // Add geocoder
            geocoder.on('result', function (ev) {
                console.log(ev.result.center);
            });
            // Add a layer showing the places.
            map.addLayer({
                "id": "places",
                "type": "symbol",
                "source": {
                    "type": "geojson",
                    "data": {
                        "type": "FeatureCollection",
                        "features": data
                    }
                },
                "layout": {
                    "icon-image": "icon",
                    'icon-size': 0.1,
                    "icon-allow-overlap": true
                }
            });
        }
    );

    //================================================
    map.addControl(new MapboxGeocoder({
        accessToken: mapboxgl.accessToken
    }));
    map.addControl(new mapboxgl.NavigationControl());

    //================================================
    // draw function
    var draw = new MapboxDraw({
        displayControlsDefault: false,
        controls: {
            point: true,
            trash: true
        },
    });

    map.addControl(draw);


    //=================================================================================
    // When a click event occurs on a feature in the places layer, open a popup at the
    // location of the feature, with description HTML from its properties.
    map.on('click', 'places', function (e) {
        var coordinates = e.features[0].geometry.coordinates.slice();
        var description = e.features[0].properties.description;

        var combineLocation = selectLocation + ";" + coordinates;
        getMatch(combineLocation);

        // Ensure that if the map is zoomed out such that multiple
        // copies of the feature are visible, the popup appears
        // over the copy being pointed to.
        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
        }
        new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(description)
            .addTo(map);
    });
    // Change the cursor to a pointer when the mouse is over the places layer.
    map.on('mouseenter', 'places', function () {
        map.getCanvas().style.cursor = 'pointer';
    });
    // Change it back to a pointer when it leaves.
    map.on('mouseleave', 'places', function () {
        map.getCanvas().style.cursor = '';
    });



    //===================================================
    map.on('draw.create', updateRoute);
    map.on('draw.update', updateRoute);
    map.on('draw.delete', removeRoute);

    var selectLocation = "";

    function updateRoute() {
        removeRoute();
        var data = draw.getAll();
        selectLocation = data.features[0].geometry.coordinates;
    }

    function removeRoute() {
        if (map.getSource('route')) {
            map.removeLayer('route');
            map.removeSource('route');
            instructions.innerHTML = '';
        } else {
            return;
        }
    }

    function getMatch(e) {
        var url = 'https://api.mapbox.com/directions/v5/mapbox/driving/' + e
            + '?geometries=geojson&steps=true&access_token=' + mapboxgl.accessToken;
        var req = new XMLHttpRequest();
        req.responseType = 'json';
        req.open('GET', url, true);
        req.onload = function () {
            var jsonResponse = req.response;
            var distance = jsonResponse.routes[0].distance * 0.001;
            var duration = jsonResponse.routes[0].duration / 60;
            var steps = jsonResponse.routes[0].legs[0].steps;
            var coords = jsonResponse.routes[0].geometry;

            // get distance and duration
            instructions.insertAdjacentHTML('beforeend', '<p>' + 'Distance: ' + distance.toFixed(2) + 'km<br>Duration: ' + duration.toFixed(2) + ' min</p><br>');

            // get route directions on load map
            steps.forEach(function (step) {
                instructions.insertAdjacentHTML('beforeend', step.maneuver.instruction + ' -> ');
            });
            instructions.insertAdjacentHTML('beforeend', " Destination");

            // add the route to the map
            addRoute(coords);
        };
        req.send();
    }

    function addRoute(coords) {
        // check if the route is already loaded
        if (map.getSource('route')) {
            map.removeLayer('route');
            map.removeSource('route');
        } else {
            map.addLayer({
                "id": "route",
                "type": "line",
                "source": {
                    "type": "geojson",
                    "data": {
                        "type": "Feature",
                        "properties": {},
                        "geometry": coords
                    }
                },
                "layout": {
                    "line-join": "round",
                    "line-cap": "round"
                },
                "paint": {
                    "line-color": "#1db7dd",
                    "line-width": 8,
                    "line-opacity": 0.8
                }
            });
        }
    }
});