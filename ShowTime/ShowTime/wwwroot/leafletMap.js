let map;

window.initMap = () => {
    if (document.getElementById('map')) {
        map = L.map('map').setView([51.505, -0.09], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);

        L.marker([51.505, -0.09]).addTo(map)
            .bindPopup('Hello World!')
            .openPopup();
    }
};

//window.addMarker = () => {
//    if (map) {
//        const lat = 51.505 + (Math.random() - 0.5) * 0.01;
//        const lng = -0.09 + (Math.random() - 0.5) * 0.01;

//        L.marker([lat, lng]).addTo(map)
//            .bindPopup(`Random marker at ${lat.toFixed(4)}, ${lng.toFixed(4)}`);
//    }
//};

window.disposeMap = () => {
    if (map) {
        map.remove();
        map = null;
    }
};