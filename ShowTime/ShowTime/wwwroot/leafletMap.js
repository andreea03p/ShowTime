let map;
let markersGroup;


window.initMap = () => {
    const mapElement = document.getElementById('map');
    if (!mapElement) {
        console.error('Map element not found');
        throw new Error('Map element not found');
    }
    map = L.map('map').setView([51.505, -0.09], 2);
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors',
        maxZoom: 18
    }).addTo(map);
    markersGroup = L.layerGroup().addTo(map);
};

window.addFestivalMarkers = (festivals) => {
    console.log('Adding festival markers...', festivals);
    if (!map) {
        console.error('Map not initialized');
        throw new Error('Map not initialized');
    }
    if (!festivals || festivals.length === 0) {
        console.warn('No festivals fetched!!!');
        return;
    }
    if (markersGroup) {
        markersGroup.clearLayers();
    }

    const validMarkers = [];
    const urlParams = new URLSearchParams(window.location.search);
    const showFestivalId = urlParams.get('showFestival');

    festivals.forEach((festival, index) => {
        console.log(`Processing festival ${index + 1}:`, festival);
        const lat = festival.latitude || festival.Latitude;
        const lng = festival.longitude || festival.Longitude;
        const name = festival.name || festival.Name;
        const address = festival.address || festival.Address;
        const startDate = festival.startDate || festival.StartDate;
        const endDate = festival.endDate || festival.EndDate;
        const capacity = festival.capacity || festival.Capacity;
        const festivalId = festival.id || festival.Id || index;

        if (!lat || !lng || isNaN(lat) || isNaN(lng)) {
            console.warn(`Invalid coordinates for festival: ${name}`, { lat, lng });
            return;
        }

        try {
            const marker = L.marker([lat, lng]);
            const formatDate = (dateStr) => {
                if (!dateStr) return 'Not specified';
                try {
                    return new Date(dateStr).toLocaleDateString();
                } catch {
                    return 'Invalid date';
                }
            };

            const popupContent = `
                <div style="min-width: 200px;">
                    <h3 style="margin: 0 0 10px 0; color: #0F4D0F;">${name || 'Unnamed Festival'}</h3>
                    <p style="margin: 5px 0;"><strong>📍 Location:</strong> ${address || 'Not specified'}</p>
                    <p style="margin: 5px 0;"><strong>📅 Starts:</strong> ${formatDate(startDate)}</p>
                    <p style="margin: 5px 0;"><strong>📅 Ends:</strong> ${formatDate(endDate)}</p>
                    <a href="/festival-details/${festivalId}" style="display: inline-block; margin-top: 10px; background: #0F4D0F; color: white; padding: 5px 10px; border-radius: 4px; text-decoration: none;">Go to Festival Page</a>
                </div>
            `;

            marker.bindPopup(popupContent);
            markersGroup.addLayer(marker);
            validMarkers.push(marker);

            // Aauto open popup
            if (showFestivalId && festivalId.toString() === showFestivalId) {
                marker.openPopup();
                map.setView([lat, lng], 10);
            }

            console.log(`Added marker for: ${name}`);
        } catch (error) {
            console.error(`Error adding marker for festival ${name}:`, error);
        }
    });

    // Only fit bounds if we're not showing a specific festival
    if (validMarkers.length > 0 && !showFestivalId)
    {
        try {
            const group = new L.featureGroup(validMarkers);
            map.fitBounds(group.getBounds().pad(0.1));
            console.log('Map bounds adjusted to show all markers');
        } catch (error) {
            console.error('Error fitting bounds:', error);
        }
    }
};

window.disposeMap = () => {
    if (markersGroup) {
        markersGroup.clearLayers();
        markersGroup = null;
    }
    if (map) {
        map.remove();
        map = null;
        console.log('Map disposed successfully');
    }
};

function getFestivalIdFromUrl() {
    const params = new URLSearchParams(window.location.search);
    return params.get('festivalId');
}
